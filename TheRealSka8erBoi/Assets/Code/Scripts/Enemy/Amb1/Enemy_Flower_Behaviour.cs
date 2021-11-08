using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class Enemy_Flower_Behaviour : AbstComp
{
    [SerializeField] private Animator animator;
    private Vector2 movement;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    public GameObject projectiles;
    private bool canshoot=false;
    public GameObject firePoint;
    [SerializeField] float cooldown;
    [SerializeField] float initcooldown;
    [SerializeField] private float fireForce=100f;
    
    [SerializeField] private Transform target;

    [SerializeField] private bool motionless;

    private NavMeshAgent agent;
    
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


    IEnumerator LongShot()
    {
        canshoot=false;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Stop");
        GameObject bullet = Instantiate(projectiles, new Vector2(firePoint.transform.position.x, firePoint.transform.position.y), Quaternion.identity);
        

        
        Vector2 toplayer = new Vector2(pj.transform.position.x - bullet.transform.position.x, pj.transform.position.y - bullet.transform.position.y);
        bullet.GetComponent<Rigidbody2D>().AddForce(toplayer * fireForce);
        
    }
}
