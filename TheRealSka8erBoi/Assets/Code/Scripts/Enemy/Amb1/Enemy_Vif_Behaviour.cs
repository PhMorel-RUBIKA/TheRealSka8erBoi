using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Vif_Behaviour : AbstComp
{
    [Header("Animator PArameters")]
    [SerializeField] private Animator animator;
    private int animValue;
    [SerializeField] private SpriteRenderer mySprite;
    [Space]
    [Header("Behaviour")]
    private Vector2 movement;
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    [Space]
    [Header("Attack Parameters")]
    public GameObject bul;
    private bool canshoot=false;
    public Transform firePoint;
    [SerializeField] float cooldown;
    [SerializeField] float initcooldown;
    [SerializeField] private float fireForce=100f;
    
    
    [SerializeField] private EnemyBulletPool ebp;


    void Start()
    {
        pj = PlayerBehaviour.playerBehaviour.gameObject;
        target = pj.transform;
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected void BehaviourAggro()
    {
        if (hp<maxhp)
        {
            lineOfSight = 100;
        }
        if(CheckPlayerInSight())
        {
            if (pj.transform.position.x-transform.position.x>-3 & pj.transform.position.x-transform.position.x < 3 & pj.transform.position.y-transform.position.y <= 0)
            {
                animator.SetTrigger("Front");
                animValue = 1;
            }
            else if(pj.transform.position.x-transform.position.x<-3 & pj.transform.position.y-transform.position.y <= 0)
            {
                animator.SetTrigger("FrontLeft");
                animValue = 2;
            }
            else if(pj.transform.position.x-transform.position.x > 3 & pj.transform.position.y-transform.position.y <= 0)
            {
                animator.SetTrigger("FrontRight");
                animValue = 3;
            }
            else if(pj.transform.position.x-transform.position.x > 3 & pj.transform.position.y-transform.position.y > 0)
            {
                animator.SetTrigger("BackRight");
                animValue = 4;
            }
            else if(pj.transform.position.x-transform.position.x < -3 & pj.transform.position.y-transform.position.y > 0)
            {
                animator.SetTrigger("BackLeft");
                animValue = 5;
            }
            else if(pj.transform.position.x-transform.position.x>-3 & pj.transform.position.x-transform.position.x < 3 & pj.transform.position.y-transform.position.y > 0)
            {
                animator.SetTrigger("Back");
                animValue = 6;
            }
            lineOfSight = 100;
            GoToPlayer();
            if(CheckPlayerInRange())
            {
                if(canshoot)
                { 
                    switch(animValue)
                    {
                        case 1 :
                            animator.SetTrigger("Atk");
                        break;
                        case 2 :
                            animator.SetTrigger("AtkFL");
                            break;
                        case 3 :
                            animator.SetTrigger("AtkFR");
                            break;
                        case 4 :
                            animator.SetTrigger("AtkBR");
                            break;
                        case 5 :
                            animator.SetTrigger("AtkBL");
                            break;
                        case 6 :
                            animator.SetTrigger("AtkBack");
                            break;
                    } 
                    StartCoroutine(LongShot()); 
                }
            }
        }
    }
    void Shoot() 
    { 
        if(canshoot==false) 
        {
            cooldown = cooldown - Time.deltaTime; 
            //Debug.Log(Time.deltaTime);
            if(cooldown <= 0)
            {
                canshoot = true; 
                cooldown = initcooldown;
            }
        }  
    }    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
        if (hp <= 0)
        {
            switch(animValue)
            {
                case 1 :
                    mySprite.flipX = false;
                    animator.SetTrigger("Ded");
                    break;
                case 2 :
                    mySprite.flipX = true;
                    animator.SetTrigger("DeadFR");
                    break;
                case 3:
                    mySprite.flipX = false;
                    animator.SetTrigger("DeadFR");
                    break;
                case 4 :
                    mySprite.flipX = false;
                    animator.SetTrigger("DeadBL");
                    break;
                case 5 :
                    mySprite.flipX = true;
                    animator.SetTrigger("DeadBL");
                    break;
                case 6 :
                    mySprite.flipX = false;
                    animator.SetTrigger("DeadBack");
                    break;
                    
            }
            
            Destroy(gameObject,1f);
        }
    }
    
   private void GoToPlayer()
    {
       
        agent.SetDestination(target.position);

    }

   private void LateUpdate()
   {
        BehaviourAggro();
        Shoot();
        
   }

    
    IEnumerator LongShot()
    {

        canshoot = false;
        yield return new WaitForSeconds(0.75f);
        animator.SetTrigger("Stop");

        //GameObject bul = ebp.GetFollowBullet();
        Instantiate(bul, firePoint.position, Quaternion.identity);
        //bul.SetActive(true);
        
        //bul.GetComponent<Rigidbody2D>().AddForce(toplayer.normalized * fireForce);
    }

}
