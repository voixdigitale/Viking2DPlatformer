using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    PlayerMovement _movement;
    private float _moveSpeedX;
    private bool _isGrounded;
    private bool _isJumping;

    private void Awake() {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        GetValues();
        Animate();
    }

    private void GetValues() {
        _moveSpeedX = _movement.GetMoveX();
        _isJumping = _movement.GetJump();
        _isGrounded = _movement.GetGround();
    }

    private void Animate() {
        SetMoveX(_moveSpeedX);
        CorrectSpriteOrientation(_moveSpeedX);
        SetGround(_isGrounded);
        SetJump(_isJumping);
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

    private void CorrectSpriteOrientation(float x) {
        if (x < 0) {
            _animator.transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (x > 0) {
            _animator.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
