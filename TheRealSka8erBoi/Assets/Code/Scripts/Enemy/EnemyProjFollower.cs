using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyProjFollower : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int damages = 3;


    private void Start()
    {
        target = PlayerBehaviour.playerBehaviour.gameObject.transform;

        Vector3 direction = target.transform.position - transform.position;
        transform.forward = direction;

        /*Vector2 toplayer = (target.transform.position - transform.position).normalized;
        float rotZ = Mathf.Atan2(toplayer.y, toplayer.x) * Mathf.Rad2Deg;
        transform.rotation= Quaternion.Euler(0f, 0f, rotZ);*/
        
        
        StartCoroutine(Evaporate());
    }
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().TakeDamage(damages);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = Vector2.MoveTowards(rb.velocity, (target.position - transform.position), 0.37f);
        Vector2 adjust = direction.normalized;
        
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, target.position);
        rb.velocity = adjust*5.5f;
    }

    IEnumerator Evaporate()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }
}
