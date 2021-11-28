using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyProjFollower : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Rigidbody2D rb;


    private void Start()
    {
        target = PlayerBehaviour.playerBehaviour.gameObject.transform;
        Vector2 toplayer = (target.transform.position - transform.position).normalized;
        float rotZ = Mathf.Atan2(toplayer.y, toplayer.x) * Mathf.Rad2Deg;
        transform.rotation= Quaternion.Euler(0f, 0f, rotZ);
        StartCoroutine(Evaporate());
    }

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Border"))
        {
            gameObject.SetActive(false);
        }

        else if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().TakeDamage(5);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Vector2 ajust = ((target.position-transform.position).normalized);
        transform.rotation=Quaternion.FromToRotation(Vector3.forward, target.position);
        rb.velocity=ajust*3;
    }

    IEnumerator Evaporate()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
