using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Run();
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        Debug.Log(_moveInput);
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * speed, _rigidbody.velocity.y);
        _rigidbody.velocity = playerVelocity;
    }
}