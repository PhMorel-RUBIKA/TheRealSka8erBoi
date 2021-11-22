using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kamikazee : AbstComp
{

    public Animator animator;
    public float moveSpeed = 3.5f;
    private bool canhit = true;
    public float hitcd = 0.5f;
    public float inithitcd = 0.5f;
    public Rigidbody2D rb;
    public SpriteRenderer myspriterenderer;

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
        if (!CheckPlayerInSight()) return;
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
        animator.SetTrigger("onmov");
    }

    

    private void FixedUpdate()
    {
        myspriterenderer.flipX = !(pj.transform.position.x - transform.position.x < 0);

        if (!(hp <= 0)) return;
        animator.SetTrigger("isded");
        Destroy(gameObject, 0.35f);
        Explosion();
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,areaSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,areaSize);
        yield return new WaitForSeconds(0f);
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
    
}