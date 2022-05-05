using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    
    private Rigidbody2D _rigidbody;
    private PlayerMovement _player;
    private float _xSpeed;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerMovement>();
        _xSpeed = _player.transform.localScale.x * bulletSpeed;
    }
    
    private void Update()
    {
        _rigidbody.velocity = new Vector2(_xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
