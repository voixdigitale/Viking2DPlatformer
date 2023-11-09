using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    PlayerMovement _movement;
    AxeThrow _axeThrow;
    private float _moveSpeedX;
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isAiming;

    private void Awake() {
        _movement = GetComponent<PlayerMovement>();
        _axeThrow = GetComponent<AxeThrow>();
    }

    private void Update() {
        GetValues();
        Animate();
    }

    private void GetValues() {
        _moveSpeedX = _movement.GetMoveX();
        _isJumping = _movement.GetJump();
        _isGrounded = _movement.GetGround();
        _isAiming = _axeThrow.IsAiming();
    }

    private void Animate() {
        SetMoveX(_moveSpeedX);
        CorrectSpriteOrientation(_moveSpeedX);
        SetGround(_isGrounded);
        SetJump(_isJumping);
        SetAiming(_isAiming);
    }

    private void SetMoveX(float x) {
        _animator.SetFloat("MoveSpeedX", Mathf.Abs(x));
    }

    private void SetGround(bool isGrounded) {
        _animator.SetBool("isGrounded", isGrounded);
    }

    private void SetJump(bool isJumping) {
        _animator.SetBool("isJumping", isJumping);
    }

    private void SetAiming(bool isAiming) {
        _animator.SetBool("isAiming", isAiming);
    }

    private void CorrectSpriteOrientation(float x) {
        if (x < 0) {
            _animator.transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (x > 0) {
            _animator.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
