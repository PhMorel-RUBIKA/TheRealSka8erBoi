using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BouncingOrbBehaviour : MonoBehaviour
{
    public Transform target;
    public int damage = 1;
    public Transform actual;
    public Collider2D[] enemiesInRange;
    public float speed = 0.1f;
    public int bounceNumber = 10;

    public void FixedUpdate()
    {
        if (target == null)
        {
            enemiesInRange = Physics2D.OverlapCircleAll(transform.position,10);
            
            if (enemiesInRange.Length < 2)
            {
                Destroy(gameObject);
            }

            float maxDist = Mathf.Infinity;
            foreach(Collider2D hit in enemiesInRange)
            {
                if (hit.transform.CompareTag("Target") || hit.transform.CompareTag("Player"))
                {
                    if (Vector3.Distance(transform.position,hit.transform.position) < maxDist)
                    {
                        maxDist = Vector3.Distance(transform.position, hit.transform.position);
                        target = hit.transform;
                    }
                    
                }
            }
        }
        else
        {
            if (target.CompareTag("Player"))
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0f,0.8f,0), speed);  
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed);  
            }
             
        }

        if (transform ==null) return;
        
        if ((target.CompareTag("Player") && Vector3.Distance(transform.position,target.position + new Vector3(0f,0.8f,0)) < 0.1f ) || Vector3.Distance(transform.position,target.position) < 0.1f)
        {
            if (target.CompareTag("Target"))
            {
                if (target.GetComponent<DamageManager>() != null) target.GetComponent<DamageManager>().TakeDamage(damage);
                 else target.GetComponent<handScript>().TakeDamage(damage);
                bounceNumber -= 1;
                
                if (bounceNumber < 1)
                    Destroy(gameObject);
            }
            
            actual = target;


            enemiesInRange = Physics2D.OverlapCircleAll(transform.position,10);
            
            if (enemiesInRange.Length < 2)
            {
                Destroy(gameObject);
            }

            float maxDist = Mathf.Infinity;
            foreach(Collider2D hit in enemiesInRange)
            {
                if ((hit.transform.CompareTag("Target") || hit.transform.CompareTag("Player")) && hit.transform != actual)
                {
                    if (Vector3.Distance(transform.position,hit.transform.position) < maxDist)
                    {
                        maxDist = Vector3.Distance(transform.position, hit.transform.position);
                        target = hit.transform;
                    }
                    
                }
            }
        }
    }
}
