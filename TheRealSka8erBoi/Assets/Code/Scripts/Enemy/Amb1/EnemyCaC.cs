using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Random;

public class EnemyCaC : AbstComp
{
    [Header("Animator Parameters")]
    //public Animator animator;
    [Space]
    
    [Header("Atck Parameters")]
    private bool canhit = true;
    public float hitcd = 0.5f;
    public float inithitcd=0.5f;
    [SerializeField]private float areaSize=2;
    [SerializeField] private int damage=10;
    [Space]  
    
    [Header("Behaviour Parameters")]
    [SerializeField] private Transform target;
    [SerializeField] private bool s2;
    
    private NavMeshAgent agent;
    private Vector2 movement;


    
    
    

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
        if (!canhit)
        {
            hitcd -= Time.deltaTime;
            if (hitcd <= 0)
            {
                canhit = true;
                hitcd = inithitcd;
            }
        }
    }
    

    protected void BehaviourAggro()
    {
        if(CheckPlayerInSight())
        {
            lineOfSight = 100;
            GoToPlayer();
            if(CheckPlayerInRange())
            {
                if (canhit)
                {
                    if (s2==false)
                    {
                      ConeAtk();  
                    }
                    else
                    {
                        StartCoroutine(Teleport());
                    }
                    //animator.SetTrigger("inrange");
                    agent.SetDestination(target.position);
                    canhit =false;
                }

            }
        } 
        
        
    }

    void GoToPlayer()
    {
       
        agent.SetDestination(target.position);
      //  animator.SetTrigger("onmov");
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {

    }

    private void GetAngle(Vector2 from, Vector2 to, out float angle)
        {
            float angleRad = Mathf.Atan2(to.y - from.y, to.x - from.x);
            angle = (180 / Mathf.PI) * angleRad;
        }
    private void ConeAtk()
    {
        Vector2 direction = transform.position - pj.transform.position;
        GetAngle(pj.transform.position, transform.position, out float angle);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(3,1), angle);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                PlayerBehaviour.playerBehaviour.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position,new Vector3(2,2,0));
        Gizmos.color=Color.yellow;
    }

    IEnumerator Teleport()
    {
        int posX = UnityEngine.Random.Range(-1, 1);
        int posY = UnityEngine.Random.Range(-1, 1);
        transform.position = new Vector3(target.transform.position.x+posX,target.transform.position.y+posY,transform.position.z);
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(.33f);
        ConeAtk();
    }
    
}
