using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine;


public class EnemyZoner : AbstComp
{
    [Header("Animator Parameters")]
    [SerializeField] private Animator animator;

    private int animValue = 0;
    [Space]
    [Header("Behaviour")]
    private Vector2 movement;
    public Rigidbody2D rb;
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    [SerializeField] private bool s2 = false;
    [Space]
    
    [Header("Attack Parameters")]
    public GameObject zone;
    private bool canshoot=false;
    [SerializeField] float cooldown;
    [SerializeField] float initcooldown;
    [SerializeField] private float rayonAngle=360;
    [SerializeField] private int bulletAmount;
    private GameObject implosionZone;
    private GameObject sndImplosionZone;
    
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
        if (hp<maxhp)
        {
            lineOfSight = 100;
        }
        if (!CheckPlayerInSight()) return;
        lineOfSight = 100;
        GoToPlayer();
        if (!CheckPlayerInRange()) return;
        if (pj.transform.position.x - transform.position.x > -3 &
                pj.transform.position.x - transform.position.x < 3 &
                pj.transform.position.y - transform.position.y <= 0)
            {
                animator.SetBool("Front",true);
                animator.SetBool("FrontLeft",false);
                animator.SetBool("FrontRight",false);
                animator.SetBool("BackLeft",false);
                animator.SetBool("BackRight",false);
                animator.SetBool("Back",false);
                
                animValue = 1;
            }
            else if (pj.transform.position.x - transform.position.x < -3 &
                     pj.transform.position.y - transform.position.y <= 0)
            {
                animator.SetBool("Front",false);
                animator.SetBool("FrontLeft",true);
                animator.SetBool("FrontRight",false);
                animator.SetBool("BackLeft",false);
                animator.SetBool("BackRight",false);
                animator.SetBool("Back",false);
                animValue = 2;
            }
            else if (pj.transform.position.x - transform.position.x > 3 &
                     pj.transform.position.y - transform.position.y <= 0)
            {
                animator.SetBool("Front",false);
                animator.SetBool("FrontLeft",false);
                animator.SetBool("FrontRight",true);
                animator.SetBool("BackLeft",false);
                animator.SetBool("BackRight",false);
                animator.SetBool("Back",false);
                animValue = 3;
            }
            else if (pj.transform.position.x - transform.position.x > 3 &
                     pj.transform.position.y - transform.position.y > 0)
            {
                animator.SetBool("Front",false);
                animator.SetBool("FrontLeft",false);
                animator.SetBool("FrontRight",false);
                animator.SetBool("BackLeft",false);
                animator.SetBool("BackRight",true);
                animator.SetBool("Back",false);
                animValue = 4;
            }
            else if (pj.transform.position.x - transform.position.x < -3 &
                     pj.transform.position.y - transform.position.y > 0)
            {
                animator.SetBool("Front",false);
                animator.SetBool("FrontLeft",false);
                animator.SetBool("FrontRight",false);
                animator.SetBool("BackLeft",true);
                animator.SetBool("BackRight",false);
                animator.SetBool("Back",false);
                animValue = 5;
            }
            else if (pj.transform.position.x - transform.position.x > -3 &
                     pj.transform.position.x - transform.position.x < 3 &
                     pj.transform.position.y - transform.position.y > 0)
            {
                animator.SetBool("Front",false);
                animator.SetBool("FrontLeft",false);
                animator.SetBool("FrontRight",false);
                animator.SetBool("BackLeft",false);
                animator.SetBool("BackRight",false);
                animator.SetBool("Back",true);
                animValue = 6;
            }
        if (!canshoot) return;

        if (s2 == false)
        {
            StartCoroutine(SoloZone());
        }
        else
        {
            StartCoroutine(MultiZone());
            agent.SetDestination(target.transform.position);
        }
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
             //animator.SetTrigger("Ded");
            Destroy(gameObject,1f);
            if (s2)
            {
                BonusManager.instance.GainScore(275);
            }
            else
            {
                BonusManager.instance.GainScore(195);
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
         
   

   private void GoToPlayer()
    {
       
        agent.SetDestination(target.position);

        Vector2 direction = pj.transform.position - transform.position;
       // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //rb.rotation = angle;
    }


   private void GetAngle(Vector2 from, Vector2 to, out float angle)
        {
            float angleRad = Mathf.Atan2(to.y - from.y, to.x - from.x);
            angle = (180 / Mathf.PI) * angleRad;
        }
            
    IEnumerator SoloZone()
    {
        animator.SetBool("TriggerLaunched",true);
        agent.SetDestination(transform.position);
        switch(animValue)
        {
            case 1 :
                animator.SetTrigger("SAtk");
                break;
            case 2 :
                animator.SetTrigger("SWAtk");
                break;
            case 3 :
                animator.SetTrigger("SEAtk");
                break;
            case 4 :
                animator.SetTrigger("NEAtk");
                break;
            case 5 :
                animator.SetTrigger("NWAtk");
                break;
            case 6 :
                animator.SetTrigger("NTAtk");
                break;
        }
        
        
        canshoot = false;
        yield return new WaitForSeconds(.9f);
        animator.SetBool("TriggerLaunched", false);
        
        implosionZone = Instantiate(zone, target.transform.position, Quaternion.identity);
        implosionZone.transform.localScale=Vector3.zero;
        implosionZone.transform.DOScale(new Vector3(3, 3, 3), .25f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(.2f);
        agent.SetDestination(target.position);
    }
 
    IEnumerator MultiZone()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("TriggerLaunched",true);
        switch(animValue)
        {
            case 1 :
                animator.SetTrigger("SAtk");
                break;
            case 2 :
                animator.SetTrigger("SWAtk");
                break;
            case 3 :
                animator.SetTrigger("SEAtk");
                break;
            case 4 :
                animator.SetTrigger("NEAtk");
                break;
            case 5 :
                animator.SetTrigger("NWAtk");
                break;
            case 6 :
                animator.SetTrigger("NTAtk");
                break;
        }
        canshoot = false;
        yield return new WaitForSeconds(.9f);
        animator.SetBool("TriggerLaunched", false);
        
        float intervalle = rayonAngle / bulletAmount;
        GetAngle(pj.transform.position, transform.right, out float angle);
        implosionZone = Instantiate(zone, target.transform.position, Quaternion.identity);
        implosionZone.transform.localScale=Vector3.zero;
        implosionZone.transform.DOScale(new Vector3(3, 3, 3), .25f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < bulletAmount; i++)
        {
            int index = i - bulletAmount / 2;
            Vector3 particular = implosionZone.transform.position;
            var q = Quaternion.AngleAxis(angle+40*i, Vector3.forward);
            Vector3 newPosition = particular + q * Vector3.right * 1.5f;
            sndImplosionZone= Instantiate(zone, newPosition, Quaternion.identity);
            sndImplosionZone.transform.localScale=Vector3.zero;
            sndImplosionZone.transform.DOScale(new Vector3(3, 3, 3), .25f).SetEase(Ease.OutBack);
            //sndImplosionZone.transform.DOScale(new Vector3(3, 3, 3), .25f).SetEase(Ease.OutBack).SetDelay(Random.Range(0,0.35f));
        }

        yield return new WaitForSeconds(.2f);
        agent.SetDestination(target.position);

    }



}
