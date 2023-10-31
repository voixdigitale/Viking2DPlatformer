using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _fallMultiplier;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Transform _floorDetector;
    [SerializeField] private float _groundDetectorRadius;
    [Header("Related GameObjects")]
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rb2d;
    private Vector2 _direction;
    private bool _isJumping;
    private bool _isGrounded = true;

    private void Awake() {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _direction.x = Input.GetAxisRaw("Horizontal") * _moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_isJumping)
        {
            _isGrounded = false;
            _animator.SetBool("isGrounded", false);
            _isJumping = true;
            _animator.SetBool("isJumping", true);
        }

        SpriteOrientation();
        GroundDetection();

        _animator.SetFloat("MoveSpeedX", Mathf.Abs(_direction.x));
    }

    private void SpriteOrientation()
    {
        if (_direction.x < 0)
        {
            _animator.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (_direction.x > 0)
        {
            _animator.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void GroundDetection()
    {
        Collider2D _groundDetector =
            Physics2D.OverlapCircle(_floorDetector.position, _groundDetectorRadius, _groundLayerMask);

        _isGrounded = _groundDetector != null;
        _animator.SetBool("isGrounded", _isGrounded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(_floorDetector.position, _groundDetectorRadius);
    }

    private void FixedUpdate()
    {
        _direction.y = _rb2d.velocity.y;

        if (_isJumping)
        {
            _isJumping = false;
            _animator.SetBool("isJumping", false);
            _direction.y = _jumpHeight * Time.fixedDeltaTime;
        }

        if (_rb2d.velocity.y < 0)
        {
            _rb2d.gravityScale = _fallMultiplier;
        }
        else
        {
            _rb2d.gravityScale = 1f;
        }

        _rb2d.velocity = _direction;
    }
}
