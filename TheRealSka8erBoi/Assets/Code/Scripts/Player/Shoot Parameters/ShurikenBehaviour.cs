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
    public Transform target;
    

    private void OnEnable()
    {
        StartCoroutine(Explode());
        GetComponent<BulletPoolBehaviour>().speed = 10;
        target = null;
    }
    

    private void FixedUpdate()
    {
        GetComponent<BulletPoolBehaviour>().speed -= 4 * Time.deltaTime;
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position,target.position,0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ui");
        if (other.gameObject.CompareTag("Target"))
        {
            GetComponent<BulletPoolBehaviour>().speed = 0;
            target = other.transform;
        }
    }


    IEnumerator Explode()
    {
        yield return new WaitForSeconds(2.5f);
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
