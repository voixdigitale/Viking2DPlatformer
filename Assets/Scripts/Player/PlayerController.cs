using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static Action OnJump;

    [SerializeField] UIManager _uiManager;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private PlayerMovement _movement;
    private AxeThrow _axeThrow;
    

    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        _axeThrow = GetComponent<AxeThrow>();
    }

    private void Update() {
        GatherInput();
        Movement();
        Jumping();
        Aiming();
        Throwing();
        Pause();
    }

    private void GatherInput() {
        _frameInput = _playerInput.FrameInput;
    }

    private void Movement() {
        _movement.SetDirectionX(_axeThrow.IsAiming() ? 0 : _frameInput.MoveX);
    }

    private void Jumping() {
        if (_frameInput.Jump) OnJump?.Invoke();
    }

    private void Aiming() {
        if (_frameInput.Attack)
            _axeThrow.Aim(new Vector2(_frameInput.MoveX, _frameInput.AimShot));
    }

    private void Throwing() {
        if (_frameInput.ReleaseAttack)
            _axeThrow.Throw();
    }

    private void Pause()
    {
        if (_frameInput.Pause)
        {
            if (!_uiManager.IsPaused)
            {
                _uiManager.Pause();
            }
            else
            {
                _uiManager.Resume();
            }
        }
    }

}
