using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new(25f, 25f);
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;

    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private BoxCollider2D _feetCollider;
    private Animator _animator;
    private Animation _animation;
    private LayerMask _groundLayer;
    private LayerMask _climbingLayer;
    private LayerMask _enemiesLayer;
    private LayerMask _hazardsLayer;
    private float _originalGravityScale;
    private bool _isAlive = true;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
    private static readonly int Dying = Animator.StringToHash("Dying");

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _animation = GetComponent<Animation>();
        _groundLayer = LayerMask.GetMask("Ground");
        _climbingLayer = LayerMask.GetMask("Climbing");
        _enemiesLayer = LayerMask.GetMask("Enemies");
        _hazardsLayer = LayerMask.GetMask("Hazards");
        _originalGravityScale = _rigidbody.gravityScale;
    }

    private void Update()
    {
        if (!_isAlive)
        {
            return;
        }

        Run();
        ClimbLadder();
        FlipSprite();
        Die();
    }

    private void OnMove(InputValue value)
    {
        if (!_isAlive)
        {
            return;
        }

        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!_isAlive)
        {
            return;
        }

        if (!_collider.IsTouchingLayers(_groundLayer))
        {
            return;
        }

        if (value.isPressed && _feetCollider.IsTouchingLayers(_groundLayer))
        {
            _rigidbody.velocity = Vector2.up * jumpForce;
        }
    }

    private void OnFire(InputValue value)
    {
        if (!_isAlive)
        {
            return;
        }
        
        Instantiate(bullet, gun.position, transform.rotation);
    }

    private void ClimbLadder()
    {
        if (!_collider.IsTouchingLayers(_climbingLayer))
        {
            _rigidbody.gravityScale = _originalGravityScale;
            _animator.SetBool(IsClimbing, false);
            return;
        }

        Vector2 playerVelocity = new Vector2(_rigidbody.velocity.x, _moveInput.y * climbSpeed);
        _rigidbody.velocity = playerVelocity;
        _rigidbody.gravityScale = 0f;

        _animator.SetBool(IsClimbing, playerVelocity.y != 0);
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * runSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = playerVelocity;

        _animator.SetBool(IsRunning, playerVelocity.x != 0);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody.velocity.x), 1f);
        }
    }

    private void Die()
    {
        if (_collider.IsTouchingLayers(_enemiesLayer) || _collider.IsTouchingLayers(_hazardsLayer))
        {
            _isAlive = false;
            _animator.SetTrigger(Dying);
            _rigidbody.AddForce(deathKick, ForceMode2D.Impulse);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}