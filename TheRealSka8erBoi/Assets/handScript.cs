using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class handScript : MonoBehaviour
{
    [SerializeField] private FinalBossBehaviour fb;

    public MMFeedbacks mmf;


    public void TakeDamage(int damage)
    {
        fb.hpBoss -= (damage);
        mmf.PlayFeedbacks();
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
