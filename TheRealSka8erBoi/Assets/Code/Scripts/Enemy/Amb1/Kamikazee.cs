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
            Debug.Log("triggerAnim");
            switch (animValue)
            {
            case 1:
                animator.SetTrigger("STrigger");
                Debug.Log("triggerAnim");
                break;
            case 2:
                animator.SetTrigger("SWTrigger");
                Debug.Log("triggerAnim");
                break;
            case 3 : 
                animator.SetTrigger("SETrigger");
                Debug.Log("triggerAnim");
                break;
            case 4 :
                animator.SetTrigger("NETrigger");
                Debug.Log("triggerAnim");
                break;
            case 5 :
                animator.SetTrigger("NWTrigger");
                Debug.Log("triggerAnim");
                break;
            case 6 :
                animator.SetTrigger("NTrigger");
                Debug.Log("triggerAnim");
                break;
            }           
            StartCoroutine("Explosion");
            canhit = false;
        }
        
        Debug.Log("triggerAnim");
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

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2f);
        FinalBoom();
        
        /*Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,areaSize);*/
        yield return new WaitForSeconds(.54f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,areaSize);
        
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
        agent.SetDestination(transform.position);
        Debug.Log("exploAnim");
        switch (animValue)
        {
            
            case 1 :
                animator.SetTrigger("SExplo");
                Debug.Log("exploAnim");
                break;
            case 2 :
                animator.SetTrigger("SEExplo");
                Debug.Log("exploAnim");
                break;
            case 3 :
                animator.SetTrigger("SWExplo");
                Debug.Log("exploAnim");
                break;
            case 4 :
                animator.SetTrigger("NEExplo");
                Debug.Log("exploAnim");
                break;
            case 5 :
                animator.SetTrigger("NWExplo");
                Debug.Log("exploAnim");
                break; 
            case 6 :
                animator.SetTrigger("NExplo");
                Debug.Log("exploAnim");
                break;
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,areaSize);
        
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
        Destroy(gameObject,.9f);
        Debug.Log("exploAnim");
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