using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Vif_Behaviour : AbstComp
{
   [SerializeField] private Animator animator;
    private int animValue;
    [SerializeField] private SpriteRenderer mySprite;
    
    private Vector2 movement;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    public Rigidbody2D projectiles;
    private bool canshoot=false;
    public GameObject firePoint;
    [SerializeField] float cooldown;
    [SerializeField] float initcooldown;
    [SerializeField] private float fireForce=100f;
    
    [SerializeField] private Transform target;

    [SerializeField] private bool motionless;

    public float offset;

    private NavMeshAgent agent;
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
            if (!motionless)
            {
              GoToPlayer();  
            }
            
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
    /*
        Vector2 direction = pj.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle; 
        //Vector2.angle
    
        direction.Normalize();
        movement = direction;
 
        transform.position = Vector2.MoveTowards(this.transform.position, pj.transform.position, moveSpeed * Time.deltaTime);
    */
    }

   private void LateUpdate()
   {
        BehaviourAggro();
        Shoot();
        
   }

    
    IEnumerator LongShot()
    {

        canshoot = false;
        yield return new WaitForSeconds(0.7f);
        animator.SetTrigger("Stop");
        Rigidbody2D bullet = Instantiate(projectiles, new Vector2(firePoint.transform.position.x, firePoint.transform.position.y), transform.rotation);

        Vector2 toplayer = (pj.transform.position - bullet.transform.position).normalized;
        float rotZ = Mathf.Atan2(toplayer.y, toplayer.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        bullet.AddForce(toplayer.normalized * fireForce);


    }

}
