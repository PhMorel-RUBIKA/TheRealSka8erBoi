using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handScript : MonoBehaviour
{
    [SerializeField] private FinalBossBehaviour fb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        fb.hpBoss -= (damage*2);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehaviour.playerBehaviour.TakeDamage(5);
        }
        
        else if (other.CompareTag("Target"))
        {
            other.GetComponent<DamageManager>().TakeDamage(5);
        }
    }
}
