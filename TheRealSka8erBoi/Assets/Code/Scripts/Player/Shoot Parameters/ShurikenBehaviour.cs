using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class ShurikenBehaviour : MonoBehaviour
{
    public GameObject VFX;
    public int damage;
    public float endExplosion;

    private void OnEnable()
    {
        Invoke("Explode", GetComponent<BulletPoolBehaviour>().waitForDestruction+0.04f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            Invoke("Explode",0.04f);
        }
    }

    public void Explode()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 2.5f);
        Instantiate(VFX, transform.position, quaternion.identity);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Target"))
            {
                enemy.GetComponent<DamageManager>().TakeDamage(damage);
            }
        }
    }
}
