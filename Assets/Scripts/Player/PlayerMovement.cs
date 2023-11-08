using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float GetMoveX() => _direction.x;
    public bool GetJump() => _isJumping;
    public bool GetGround() => _isGrounded;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _fallMultiplier;
    [SerializeField] private float _gravityDelay;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Transform _floorDetector;
    [SerializeField] private float _groundDetectorRadius;

    private PlayerAnimation _animation;

    private Rigidbody2D _rb2d;
    private Vector2 _direction;
    private bool _isJumping;
    private bool _isGrounded = true;
    private float _timeInAir = 0;

    private void Awake() {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GroundDetection();
        GravityDelay();
    }

    private void FixedUpdate()
    {
        if (_isJumping)
        {
            _isJumping = false;
            _timeInAir = 0;
            _rb2d.AddForce(Vector2.up * _jumpHeight, ForceMode2D.Impulse);
        }

        _direction.y = _rb2d.velocity.y;
        _rb2d.velocity = _direction;
        ExtraGravity();
    }

    public void Jump() {
        if (_isGrounded && !_isJumping) {
            _isGrounded = false;
            _isJumping = true;
        }
    }

    public void SetDirectionX(float x) {
        _direction.x = x * _moveSpeed;
    }

    private void GroundDetection()
    {
        Collider2D _groundDetector =
            Physics2D.OverlapCircle(_floorDetector.position, _groundDetectorRadius, _groundLayerMask);

        _isGrounded = _groundDetector != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(_floorDetector.position, _groundDetectorRadius);
    }


    private void GravityDelay() {
        if (!_isGrounded) {
            _timeInAir += Time.deltaTime;
        } else {
            _timeInAir = 0f;
        }
    }

    private void ExtraGravity() {
        if (_timeInAir > _gravityDelay) {
            _rb2d.AddForce(new Vector2(0f, -_fallMultiplier * Time.deltaTime));
        }
    }
}
