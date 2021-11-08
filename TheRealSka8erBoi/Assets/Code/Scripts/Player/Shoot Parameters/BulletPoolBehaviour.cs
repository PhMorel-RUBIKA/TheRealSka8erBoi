using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class BulletPoolBehaviour : MonoBehaviour
{
    public string bulletName;
    public float waitForDestruction;
    public Vector2 force;
    [SerializeField] private int speed;
    private Rigidbody2D rb;
    public int damage;
    [SerializeField] private Transform damagePopUp;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        if (waitForDestruction > 0)
        {
            StartCoroutine(DestroyPooledObject());
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            other.GetComponent<DamageManager>().TakeDamage(damage);
            Transform damageUI = Instantiate(damagePopUp, new Vector3(other.transform.position.x,other.transform.position.y,other.transform.position.z - 2),Quaternion.identity);
            damageUI.gameObject.GetComponent<TextMeshPro>().text = damage.ToString();
            Destroy(damageUI.gameObject,1);
        }
    }
    
    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        rb.velocity = force * speed;
    }

    IEnumerator DestroyPooledObject()
    {
        yield return new WaitForSeconds(waitForDestruction); 
        PoolObjectManager.Instance.DestroyBullet(bulletName, this.gameObject);
    }
}