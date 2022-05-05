using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] int coinValue = 100;

    private bool _wasCollected = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_wasCollected)
        {
            _wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(coinValue);
            Destroy(gameObject);
        }
    }
}