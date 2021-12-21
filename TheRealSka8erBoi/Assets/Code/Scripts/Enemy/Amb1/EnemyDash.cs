using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDash : AbstComp
{

    //public Animator animator;
    public float moveSpeed = 3.5f;
    private bool canhit = true;
    public float hitcd = 0.5f;
    public float inithitcd=0.5f;
    public Rigidbody2D rb;
    //public SpriteRenderer myspriterenderer;
    
    public bool facingRight = true;
    
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private Vector2 movement;
    private Vector2 direction;
    private Vector2 dashPointPos;
    
    [SerializeField] private BoxCollider2D boxCo;
    [SerializeField] private int dashSpeed;
    private bool attackFinished = true;
    [SerializeField] private int damages = 10;
    [SerializeField] private bool s2;

    void Start()
    { 
        pj = PlayerBehaviour.playerBehaviour.gameObject;
        target = pj.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        BehaviourAggro();
        AbleToAttack();
    }


    private void BehaviourAggro()
    {
        if(CheckPlayerInSight())
        {
            lineOfSight = 100;
            GoToPlayer();
            if(CheckPlayerInRange())
            {
                if (canhit && !s2)
                {
                    //attackFinished = false;
                    //animator.SetTrigger("inrange");
                    canhit = !canhit;
                    attackFinished = false;
                    StartCoroutine(DashAttck());
                }
                else if(canhit && s2)
                {
                    canhit = !canhit;
                    attackFinished = false;
                    StartCoroutine(MultipleDashAttack());
                }

            }
        }
    }

    void GoToPlayer()
    {
        agent.SetDestination(target.position);
        //animator.SetTrigger("onmov");
    }



     public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
         if (hp <= 0)
        {
            //animator.SetTrigger("Ded");
            Destroy(gameObject,1f);
        }
    }

    private void FixedUpdate()
    {
        if (pj.transform.position.x-transform.position.x < 0)
        {
            //myspriterenderer.flipX = false;
        }
        else
        {
           // myspriterenderer.flipX = true;
        }
           
        if(hp<=0)
        {
            //animator.SetTrigger("isded");
            Destroy(gameObject, 0.35f);
        }
        
    }

    IEnumerator StopMotion()
    {
        agent.SetDestination(transform.position);
        rb.velocity=Vector2.zero;
        yield return new WaitForSeconds(0.8f);
    }

    IEnumerator StopMotion2()
    {
        agent.SetDestination(transform.position);
        rb.velocity=Vector2.zero;
        yield return new WaitForSeconds(0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().TakeDamage(damages);
        }

        if (other.CompareTag("Border"))
        {
            rb.velocity=Vector2.zero;
        }
        
    }

    private const float upToFitPlayer = 0.1f;
    IEnumerator DashAttck()
    {
        agent.SetDestination(transform.position);
        rb.velocity=Vector2.zero;
        boxCo.isTrigger = true;
        //Debug.Log("je commence le dash");
        dashPointPos = (target.position - transform.position);
        dashPointPos.Normalize();
        yield return new WaitForSeconds(.5f);

        rb.velocity = new Vector2(dashPointPos.x, dashPointPos.y+upToFitPlayer) * dashSpeed;
        yield return new WaitForSeconds(.6f);
        rb.velocity=Vector2.zero;
        attackFinished = true;
        boxCo.isTrigger = false;
        agent.SetDestination(target.position);
        
        //Debug.Log("Je fini le dash");
    }

    
    
    IEnumerator MultipleDashAttack()
    {
        agent.SetDestination(transform.position);
        rb.velocity=Vector2.zero;
        boxCo.isTrigger = true;
        for (int i = 0; i < 3; i++)
        {
            dashPointPos = (target.position - transform.position);
            dashPointPos.Normalize();
            yield return new WaitForSeconds(.5f);

            rb.velocity = new Vector2(dashPointPos.x, dashPointPos.y+upToFitPlayer) * dashSpeed;
            yield return new WaitForSeconds(.6f);
            rb.velocity=Vector2.zero;

        }

        agent.SetDestination(target.position);
        boxCo.isTrigger = false;
        attackFinished = true;

    }

    void AbleToAttack()
    {
        if (!canhit && attackFinished)
        {
            hitcd -= Time.deltaTime;
            if (hitcd <= 0)
            {
                canhit = true;
                hitcd = inithitcd;
            }
        }
    }
}
