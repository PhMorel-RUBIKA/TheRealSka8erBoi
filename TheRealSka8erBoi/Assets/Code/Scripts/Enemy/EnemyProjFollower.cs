using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;

public class EnemyProjFollower : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int damages = 3;
    [SerializeField] private float speedModifier;
    [SerializeField] private float factorFollow;
    public GameObject playerImpact;
    public MMFeedbacks perfection;


    private void Start()
    {
        target = PlayerBehaviour.playerBehaviour.gameObject.transform;

        Vector3 direction = target.transform.position - transform.position;
        transform.forward = direction;

        StartCoroutine(Evaporate());
    }
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().TakeDamage(damages);
            gameObject.SetActive(false);
            Instantiate(playerImpact, transform.position, quaternion.identity);
        }
    }
    

    private void FixedUpdate()
    {
        Vector2 direction = Vector2.MoveTowards(rb.velocity, (target.position - transform.position), 0.25f);
        Vector2 adjust = direction.normalized;
        
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, target.position);
        rb.velocity = adjust * factorFollow;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedModifier);
    }

    IEnumerator Evaporate()
    {
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }
}
