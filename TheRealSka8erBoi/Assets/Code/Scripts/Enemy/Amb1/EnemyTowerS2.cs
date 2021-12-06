using System;
using System.Collections;
using DG.Tweening;
using UnityEditor.AnimatedValues;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTowerS2 : AbstComp
{
    [Header("Animator Parmeters")]
    [SerializeField] private Animator animator;
    private int animValue;
    [Space]
    [Header("NavMesh Parameters")]
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    [SerializeField] private bool motionless;
    private Vector2 movement;
    public Rigidbody2D rb;
    [Space]
    [Header("Atk Parameters")]
    public Transform firePoint;
    [SerializeField] float cooldown;
    [SerializeField] float initcooldown;
    [SerializeField] private float fireForce=100f;
    //public GameObject projectiles;
    private bool canshoot=false;
    public float offset;
    [SerializeField] private bool s2 = false;
    [SerializeField] private EnemyBulletPool ebp;
    [SerializeField] public int bulletsAmount = 8;
    [SerializeField] private float rayonAngle=90;
    
    
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
        if (!CheckPlayerInSight()) return;
        lineOfSight = 100;
        if (!motionless)
        {
            GoToPlayer();  
        }

        if (!CheckPlayerInRange()) return;
        if (!canshoot) return;
       //animator.SetTrigger("Atk");
        if (s2 == false)
        {
            StartCoroutine(LongShot());
        }
        else
        {
            SpreadShot();
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
    }
   

    IEnumerator LongShot()
    {

        canshoot = false;
        float distanceX = pj.transform.position.x - transform.position.x;
        float distanceY = pj.transform.position.y - transform.position.y;
        if (distanceY <= 0)
        {
            if (distanceX >= -4 & distanceX < 4)
            {
                animator.SetTrigger("AtkS");
                animValue = 1;
            }
            else if (distanceX <= -4)
            {
                animator.SetTrigger("AtkSE");
                animValue = 2;
            }
            else if (distanceX >= 4)
            {
                animator.SetTrigger("AtkSE");
                animValue = 3;
            }
        }
        else if (distanceY > 0)
        {
            if (distanceX >= -4 & distanceX < 4)
            {
                animator.SetTrigger("AtkN");
                animValue = 4;
            }
            else if (distanceX > -4)
            {
                animator.SetTrigger("AtkNW");
                animValue = 5;
            }
            else if (distanceX <= -4)
            {
                animator.SetTrigger("AtkNW");
                animValue = 6;
            }
        }

        yield return new WaitForSeconds(0.7f);

        GameObject bul = ebp.enemyBulletPoolInstance.GetBullet();
        Vector2 toplayer = (pj.transform.position - firePoint.transform.position).normalized;
        float rotZ = Mathf.Atan2(toplayer.y, toplayer.x) * Mathf.Rad2Deg;
        bul.transform.position = firePoint.transform.position;
        bul.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        bul.SetActive(true);
        
        bul.GetComponent<Rigidbody2D>().AddForce(toplayer.normalized * fireForce);
    }
    
            private void GetAngle(Vector2 from, Vector2 to, out float angle)
        {
            float angleRad = Mathf.Atan2(to.y - from.y, to.x - from.x);
            angle = (180 / Mathf.PI) * angleRad;
        }
            
    private void SpreadShot()
    {
        canshoot = false;
        var intervalle = rayonAngle / bulletsAmount;
        for (int i = 0; i < bulletsAmount; i++)
        {
            var index = i - bulletsAmount / 2;
            Vector2 toplayer = (pj.transform.position - firePoint.transform.position).normalized;
            float rotZ = Mathf.Atan2(toplayer.y, toplayer.x) * Mathf.Rad2Deg;
            GameObject bul = ebp.enemyBulletPoolInstance.GetBullet();
            bul.transform.position = firePoint.transform.position;
            bul.SetActive(true);

            GetAngle(pj.transform.position, firePoint.position, out float angle); 
            bul.transform.rotation = Quaternion.Euler(0, 0, -(angle+index*intervalle)); 
            bul.GetComponent<Rigidbody2D>().AddForce(bul.transform.right.normalized*fireForce);
            //bul.transform.DOScale(new Vector3(3, 3, 3), 0.5f);
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(firePoint.transform.position,pj.transform.position);
    }
}
