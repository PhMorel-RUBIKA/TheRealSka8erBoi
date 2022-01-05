using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class BulletPoolBehaviour : MonoBehaviour
{
    public string bulletName;
    public float waitForDestruction;
    public Vector2 force;
    public float speed;
    private Rigidbody2D rb;
    public int damage;
    //[SerializeField] private Transform damagePopUp;
    public GameObject impactTir;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            other.GetComponent<DamageManager>().TakeDamage(damage);
            //Transform damageUI = Instantiate(damagePopUp, new Vector3(other.transform.position.x,other.transform.position.y + 1,other.transform.position.z - 2),Quaternion.identity);
            //damageUI.gameObject.GetComponent<TextMeshPro>().text = damage.ToString();
            //Destroy(damageUI.gameObject,1);
            Instantiate(impactTir, transform.position, quaternion.identity);
        }
    }
    
    private void FixedUpdate()
    {
        StartCoroutine(DestroyPooledObject());
        rb.velocity = Vector2.zero;
        rb.velocity = force * speed;
    }

    IEnumerator DestroyPooledObject()
    {
        yield return new WaitForSeconds(waitForDestruction); 
        PoolObjectManager.Instance.DestroyBullet(bulletName, this.gameObject);
    }
}