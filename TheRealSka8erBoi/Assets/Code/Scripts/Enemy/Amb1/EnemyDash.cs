using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDash : AbstComp
{
    [Header("Comportment Parameters")]
    public Animator animator;
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
    [Space]
    [Header("Atck Parameters")]
    [SerializeField] private BoxCollider2D boxCo;
    [SerializeField] private int dashSpeed;
    private bool attackFinished = true;
    [SerializeField] private int damages = 10;
    [SerializeField] private bool s2;

    private bool canMove;
    
    //VFX
    public GameObject previewDash;

    void Start()
    { 
        animator.SetBool("Atk",false);
        pj = PlayerBehaviour.playerBehaviour.gameObject;
        target = pj.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rb = this.GetComponent<Rigidbody2D>();
        canMove = true;
    }

    void Update()
    {
        BehaviourAggro();
        AbleToAttack();
    }


    private void BehaviourAggro()
    {
        if (hp<maxhp)
        {
            lineOfSight = 100;
        }
        if(CheckPlayerInSight())
        {
            lineOfSight = 100;
            if (canMove) GoToPlayer();
            if(CheckPlayerInRange())
            {
                if (pj.transform.position.x - transform.position.x > -3 &
                    pj.transform.position.x - transform.position.x < 3 &
                    pj.transform.position.y - transform.position.y <= 0)
                {
                    animator.SetTrigger("S");
                    
                }
                else if (pj.transform.position.x - transform.position.x < -3 &
                         pj.transform.position.y - transform.position.y <= 0)
                {
                    animator.SetTrigger("SW");
                }
                else if (pj.transform.position.x - transform.position.x > 3 &
                         pj.transform.position.y - transform.position.y <= 0)
                {
                    animator.SetTrigger("SE");
                  
                }
                else if (pj.transform.position.x - transform.position.x > 3 &
                         pj.transform.position.y - transform.position.y > 0)
                {
                    animator.SetTrigger("NW");
                }
                else if (pj.transform.position.x - transform.position.x < -3 &
                         pj.transform.position.y - transform.position.y > 0)
                {
                    animator.SetTrigger("NW");
                }
                else if (pj.transform.position.x - transform.position.x > -3 &
                         pj.transform.position.x - transform.position.x < 3 &
                         pj.transform.position.y - transform.position.y > 0)
                {
                    animator.SetTrigger("N");
                }
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
            if (s2)
            {
                BonusManager.instance.GainScore(315);
            }
            else
            {
                BonusManager.instance.GainScore(255);
            }
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
        canMove = false;
        animator.SetBool("Atk",true);
        dashPointPos = (target.position - transform.position);
        dashPointPos.Normalize();
        float rotZ = Mathf.Atan2(dashPointPos.y, dashPointPos.x) * Mathf.Rad2Deg;
        previewDash.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        Instantiate(previewDash, transform.position, Quaternion.Euler(0f, 0f, rotZ));
        agent.SetDestination(transform.position);
        rb.velocity=Vector2.zero;
        boxCo.isTrigger = true;
        //Debug.Log("je commence le dash");
        dashPointPos = (target.position - transform.position);
        dashPointPos.Normalize();
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("Atk",false);
        rb.velocity = new Vector2(dashPointPos.x, dashPointPos.y+upToFitPlayer) * dashSpeed;
        yield return new WaitForSeconds(.7f);
        rb.velocity=Vector2.zero;
        attackFinished = true;

        canMove = true;
        agent.SetDestination(target.position);
        boxCo.isTrigger = false;
        
        //Debug.Log("Je fini le dash");
    }

    
    
    IEnumerator MultipleDashAttack()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("Atk",true);
        rb.velocity=Vector2.zero;
        boxCo.isTrigger = true;
        for (int i = 0; i < 3; i++)
        {
            dashPointPos = (target.position - transform.position);
            dashPointPos.Normalize();
            yield return new WaitForSeconds(1.3f);

            rb.velocity = new Vector2(dashPointPos.x, dashPointPos.y+upToFitPlayer) * dashSpeed;
            yield return new WaitForSeconds(.7f);
            rb.velocity=Vector2.zero;

        }

        agent.SetDestination(target.position);
        boxCo.isTrigger = false;
        attackFinished = true;
        animator.SetBool("Atk",false);

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
