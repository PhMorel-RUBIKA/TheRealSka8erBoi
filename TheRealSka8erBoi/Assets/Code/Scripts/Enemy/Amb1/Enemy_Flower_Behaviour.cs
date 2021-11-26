using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class Enemy_Flower_Behaviour : AbstComp
{
    [SerializeField] private Animator animator;
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

    private NavMeshAgent agent;

    public float offset;

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
        Shoot();
    }
    
    protected void BehaviourAggro()
    {
        if(CheckPlayerInSight())
        {
            lineOfSight = 100;
            if (!motionless)
            {
              GoToPlayer();  
            }
            
            if(CheckPlayerInRange())
            {
                if(canshoot)
                { 
                    animator.SetTrigger("Atk");
                   StartCoroutine(LongShot()); 
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
         if (hp <= 0)
        {
            animator.SetTrigger("Ded");
            Destroy(gameObject,1f);
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
         
   

   private void GoToPlayer()
    {
       
        agent.SetDestination(target.position);

        Vector2 direction = pj.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle; 
        //Vector2.angle
    /*
        direction.Normalize();
        movement = direction;
 
        transform.position = Vector2.MoveTowards(this.transform.position, pj.transform.position, moveSpeed * Time.deltaTime);
    */
    }

    //Version modifiée 

    IEnumerator LongShot()
    {

        canshoot = false;
        yield return new WaitForSeconds(0.7f);

        animator.SetTrigger("Stop");
        Rigidbody2D bullet = Instantiate(projectiles, new Vector2(firePoint.transform.position.x, firePoint.transform.position.y),transform.rotation);

        Vector2 toplayer = (pj.transform.position - bullet.transform.position).normalized;

        float rotZ = Mathf.Atan2(toplayer.y, toplayer.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        bullet.AddForce(toplayer.normalized * fireForce);


    }

    
}
