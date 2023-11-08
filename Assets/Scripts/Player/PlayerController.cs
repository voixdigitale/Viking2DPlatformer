using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
        Attacking();
        Throwing();
    }

    private void GatherInput() {
        _frameInput = _playerInput.FrameInput;
    }

    private void Movement() {
        _movement.SetDirectionX(_frameInput.MoveX);
    }

    private void Jumping() {
        if (_frameInput.Jump) _movement.Jump();
    }

    private void Throwing() {
        if (_frameInput.Attack)
            _axeThrow.Throw();
    }

    private void Attacking() {
        //if (_frameInput.Attack) { }
        
        /*
        if (_frameInput.Attack && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Smash") {
            _animator.SetTrigger("Attack");
        }*/
    }
}
