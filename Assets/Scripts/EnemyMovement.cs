using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    private Rigidbody2D _enemyRigidbody;
    private BoxCollider2D _groundCheckCollider;
    
    private void Start()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _groundCheckCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_enemyRigidbody.velocity.x)), 1f);
    }
}
