using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    [SerializeField] private int damages = 5;
    public GameObject playerImpact;
    
    void Start()
    {
        Invoke("SetFalse",5);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().TakeDamage(damages);
            gameObject.SetActive(false);
            Instantiate(playerImpact, transform.position, Quaternion.identity);
        }
    }

    void SetFalse()
    {
        gameObject.SetActive(false);
    }
}
