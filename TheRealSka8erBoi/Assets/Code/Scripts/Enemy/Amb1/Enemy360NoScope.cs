using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy360NoScope : AbstComp

{

    [SerializeField] private bool s2;
    [SerializeField] private Rigidbody2D rb;
    public float cooldown = 0.5f;
    public float initcooldown = 0.5f;
    public bool facingRight = true;
    private bool canshoot = true;
    
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private Vector2 movement;
    private Vector2 direction;
    [SerializeField] private bool motionless;
    
    [Space]
    [Header("Shooting")]
    
    public Rigidbody2D projectiles;
    [SerializeField] private EnemyBulletPool ebp;
    [SerializeField] public int bulletsAmount = 4;
    [SerializeField] private float greatAngle=360f;
    [SerializeField] private float fireForce;
    
    void Start()
    {
        canshoot = false;
        pj = PlayerBehaviour.playerBehaviour.gameObject;
        target = pj.transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if (s2)
        {
            bulletsAmount *= 2;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        BehaviourAggro();
    }

    private void BehaviourAggro()
    {
        if (!CheckPlayerInSight()) return;
        lineOfSight = 100;
        if (!motionless)
        {
            GoToPlayer();  
        }

        if (!CheckPlayerInRange()) return;
        if (canshoot && !s2)
        {
            NoScope();
        }
        else
        {
            StartCoroutine("NoNoNoScope");
        }
    }
    
    private void GetAngle(Vector2 from, Vector2 to, out float angle) 
    { 
        float angleRad = Mathf.Atan2(to.y - from.y, to.x - from.x); 
        angle = (180 / Mathf.PI) * angleRad;
    }

    private void NoScope()
    {
        canshoot = false;
        var intervalle = greatAngle / bulletsAmount;
        for (int i = 0; i < bulletsAmount; i++)
        {
            var index = i - bulletsAmount / 2;
            GameObject bul = ebp.enemyBulletPoolInstance.GetBullet();
            bul.transform.position = transform.position;
            bul.SetActive(true);
            GetAngle(pj.transform.position, transform.position, out float angle);
            bul.transform.rotation = Quaternion.Euler(0, 0, angle+index*intervalle); 
            bul.GetComponent<Rigidbody2D>().velocity = -bul.transform.right;
        }
    }


    IEnumerator NoNoNoScope()
    {
        canshoot = false;
        var intervalle = greatAngle / bulletsAmount;
        for (int x = 0; x < 3; x++)
        {
            yield return new WaitForSeconds(0.2f);
            for (int y = 0; y < bulletsAmount; y++)
            {
                var index = y - bulletsAmount / 2;
                GameObject bul = ebp.enemyBulletPoolInstance.GetBullet();
                bul.transform.position = transform.position;
                bul.SetActive(true);
                GetAngle(pj.transform.position, transform.position, out float angle);
                bul.transform.rotation = Quaternion.Euler(0, 0, angle+index*intervalle); 
                bul.GetComponent<Rigidbody2D>().velocity = -bul.transform.right;
            }                
        }  
    }
    
     public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
         if (hp <= 0)
        {
            //animator.SetTrigger("Ded");
            Destroy(gameObject,1f);
        }
    }
     
         void GoToPlayer()
    {
        agent.SetDestination(target.position);
     //   animator.SetTrigger("onmov");
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
}
