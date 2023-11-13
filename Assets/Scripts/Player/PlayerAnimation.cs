using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Android;
using UnityEngine.Tilemaps;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _runParticles;
    [SerializeField] private ParticleSystem _jumpVFX;
    [SerializeField] private float _yLandVelocityCheck = -10f;

    PlayerMovement _movement;
    AxeThrow _axeThrow;
    Vector2 _velocityBeforePhysicsUpdate;
    Rigidbody2D _rb2d;
    CinemachineImpulseSource _impulseSource;
    private float _moveSpeedX;
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isAiming;

    private void Awake() {
        _movement = GetComponent<PlayerMovement>();
        _axeThrow = GetComponent<AxeThrow>();
        _rb2d = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void FixedUpdate() {
        _velocityBeforePhysicsUpdate = _rb2d.velocity;
    }

    private void Update() {
        GetValues();
        Animate();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (_velocityBeforePhysicsUpdate.y < _yLandVelocityCheck) {
            PlayJumpDust();
            _impulseSource.GenerateImpulse();
        }
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

        if (Mathf.Abs(_moveSpeedX) > 0 && _isGrounded)
        {
            var emission = _runParticles.emission;
            emission.rateOverDistance = 1;
        }
        else
        {
            var emission = _runParticles.emission;
            emission.rateOverDistance = 0;
        }
    }

    private void SetMoveX(float x) {
        _animator.SetFloat("MoveSpeedX", Mathf.Abs(x));
    }

    private void SetGround(bool isGrounded) {
        _animator.SetBool("isGrounded", isGrounded);
    }

    private void SetJump(bool isJumping) {
        _animator.SetBool("isJumping", isJumping);
        if (isJumping)
        {
            PlayJumpDust();
        }
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
    private void PlayJumpDust() {
        _jumpVFX.Play();
    }
}
