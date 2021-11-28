using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kamikazee : AbstComp
{

    public Animator animator;
    private int animValue=1;
    public float moveSpeed = 3.5f;
    private bool canhit = true;
    public Rigidbody2D rb;

    public bool facingRight = true;

    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private Vector2 movement;
    private Vector2 direction;


    /// <summary>
    /// For the Explosion
    /// </summary>
    
    [SerializeField] private float areaSize;

    [SerializeField] private int damage = 10;

    [SerializeField] private float delay;

    void Start()
    {
        pj = PlayerBehaviour.playerBehaviour.gameObject;
        target = pj.transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        BehaviourAggro();
        
    }
    
    protected void BehaviourAggro()
    {
        float distanceX = pj.transform.position.x - transform.position.x;
        float distanceY = pj.transform.position.y - transform.position.y;
        
        if (!CheckPlayerInSight()) return;
            if (distanceX>-4 & distanceX < 4 & distanceY <= 0)
            {
                animator.SetTrigger("Front");
                animValue = 1;
            }
            else if(distanceX<=-4 & distanceY <= 0)
            {
                animator.SetTrigger("FrontLeft");
                animValue = 2;
            }
            else if(distanceX >= 4 & distanceY <= 0)
            {
                animator.SetTrigger("FrontRight");
                animValue = 3;
            }
            else if(distanceX >= 4 & distanceY > 0)
            {
                animator.SetTrigger("BackRight");
                animValue = 4;
            }
            else if(distanceX <= -4 & distanceY > 0)
            {
                animator.SetTrigger("BackLeft");
                animValue = 5;
            }
            else if(distanceX > -4 & distanceX < 4 & distanceY > 0)
            {
                animator.SetTrigger("Back");
                animValue = 6;
            }
        lineOfSight = 100;
        GoToPlayer();
        if (!CheckPlayerInRange()) return;
        if (canhit)
        {
            StartCoroutine("Explosion");
            canhit = false;
        }
        

    }

    void GoToPlayer()
    {
        agent.SetDestination(target.position);
    }

    

    private void FixedUpdate()
    {
        if (!(hp <= 0)) return;
        animator.SetTrigger("isded");
        Destroy(gameObject, 0.35f);
        StartCoroutine(Explosion());
    }

    IEnumerator StopMotion()
    {
        moveSpeed = 0;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator StopMotion2()
    {
        moveSpeed = 0;
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Explosion()
    {
        switch (animValue)
        {
            case 1:
                animator.SetTrigger("STrigger");
                break;
            case 2:
                animator.SetTrigger("SWTrigger");
                break;
            case 3 : 
                animator.SetTrigger("SETrigger");
                break;
            case 4 :
                animator.SetTrigger("NETrigger");
                break;
            case 5 :
                animator.SetTrigger("NWTrigger");
                break;
            case 6 :
                animator.SetTrigger("NTrigger");
                break;
        }
        
        FinalBoom();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,areaSize);
        /*Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,areaSize);*/
        yield return new WaitForSeconds(.54f);
        agent.SetDestination(transform.position);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                PlayerBehaviour.playerBehaviour.TakeDamage(damage);
            }

            else if (enemy.gameObject.CompareTag("Target"))
            {
                DamageManager.instance.TakeDamage(damage);
            }
        }
        
    }

    void FinalBoom()
    {
        switch (animValue)
        {
            case 1 :
                animator.SetTrigger("SExplo");
                break;
            case 2 :
                animator.SetTrigger("SExplo");
                break;
            case 3 :
                animator.SetTrigger("SExplo");
                break;
            case 4 :
                animator.SetTrigger("SExplo");
                break;
            case 5 :
                animator.SetTrigger("SExplo");
                break; 
            case 6 :
                animator.SetTrigger("SExplo");
                break;
        }
        Destroy(gameObject,1);
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
        if (hp <= 0)
        {
            FinalBoom();
        }
    }
}