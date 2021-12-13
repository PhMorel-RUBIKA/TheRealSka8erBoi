using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    void Start()
    {
        Invoke("SetFalse",5);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().TakeDamage(5);
            gameObject.SetActive(false);
        }
    }

    void SetFalse()
    {
        gameObject.SetActive(false);
    }
}
