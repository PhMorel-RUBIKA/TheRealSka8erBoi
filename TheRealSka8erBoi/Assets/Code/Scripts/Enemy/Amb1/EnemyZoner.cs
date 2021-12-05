using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.AI;
using UnityEngine;
using static UnityEngine.Random;
using Random = UnityEngine.Random;

public class EnemyZoner : AbstComp
{
    //[Header("Animator Parameters")]
    //[SerializeField] private Animator animator;
    //[Space]
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
        if (!CheckPlayerInSight()) return;
        lineOfSight = 100;

        if (!CheckPlayerInRange()) return;
        if (!canshoot) return;
       //animator.SetTrigger("Atk");
        if (s2 == false)
        {
            SoloZone();
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
        Debug.Log(hp);
         if (hp <= 0)
        {
            Destroy(gameObject);
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


   private void GetAngle(Vector2 from, Vector2 to, out float angle)
        {
            float angleRad = Mathf.Atan2(to.y - from.y, to.x - from.x);
            angle = (180 / Mathf.PI) * angleRad;
        }
            
    private void SoloZone()
    {
        canshoot = false;
        implosionZone = Instantiate(zone, target.transform.position, Quaternion.identity);
        implosionZone.transform.localScale=Vector3.zero;
        implosionZone.transform.DOScale(new Vector3(3, 3, 3), .25f).SetEase(Ease.OutBack);
    }

    IEnumerator MultiZone()
    {
        canshoot = false;
        agent.SetDestination(transform.position);
        float intervalle = rayonAngle / bulletAmount;
        GetAngle(pj.transform.position, transform.right, out float angle);
        SoloZone();
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
        
    }



}
