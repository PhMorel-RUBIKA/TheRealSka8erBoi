using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kamikazee : AbstComp
{
    [Header("Animation Parameters")]
    public Animator animator;
    private int animValue=1;
    public bool facingRight = true;
    [Space]
    
    [Header("NavMesh Parameters")]
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    private Vector2 movement;
    private Vector2 direction;
    [Space]
    
    [Header("Comportment Parameters")]
    private bool canhit = true;
    [SerializeField] private float areaSize;
    [SerializeField] private int damages = 10;
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
            StartCoroutine("Explosion");
            canhit = false;
        }
        
    }

    void GoToPlayer()
    {
        agent.SetDestination(target.position);
    }

    

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FinalBoom());
        
        //Gizmos.color = Color.yellow;
    }

    IEnumerator FinalBoom()
    {
        agent.SetDestination(transform.position);
        switch (animValue)
        {
            
            case 1 :
                animator.SetTrigger("SExplo");
                break;
            case 2 :
                animator.SetTrigger("SEExplo");
                break;
            case 3 :
                animator.SetTrigger("SWExplo");
                break;
            case 4 :
                animator.SetTrigger("NEExplo");
                break;
            case 5 :
                animator.SetTrigger("NWExplo");
                break; 
            case 6 :
                animator.SetTrigger("NExplo");
                break;
        }
        yield return new WaitForSeconds(.84f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,areaSize);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                PlayerBehaviour.playerBehaviour.TakeDamage(damages);
            }

            else if (enemy.gameObject.CompareTag("Target"))
            {
                DamageManager.instance.TakeDamage(damages);
            }
        }
        Destroy(gameObject,0f);
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
        if (hp <= 0)
        {
            animator.SetBool("Instantkill", true);
            StartCoroutine(FinalBoom());
        }
    }
}