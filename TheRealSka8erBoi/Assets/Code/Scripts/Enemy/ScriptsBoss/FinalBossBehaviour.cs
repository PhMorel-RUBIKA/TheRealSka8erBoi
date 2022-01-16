using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Random;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class FinalBossBehaviour : MonoBehaviour
{
    
    [Header("Show Value")]
    [SerializeField] private int inSecond;
    [SerializeField] private int _frameCounter;
    [Space(20)]
    public int refreshTime = 1500;

    private Quaternion quaternionFx;
    private  Vector3 rotationFx = new Vector3(-90, 0, 0); 
    public List<int> keyFrames=new List<int>();

    public int define;

    [Space] [Header("Behaviour")] 
    [SerializeField] private Animation intro;
    private Transform target;
    public int hpBoss;
    [SerializeField] private int maxHPBoss;
    public bool bossIsMidLife=false;
    
    [Space] [Header("ArmAttack Parameters")]
    [SerializeField] private GameObject armAtck;
    private GameObject lArmAtckInstance;
    
    [SerializeField] public CircleCollider2D leftHandCollider;
    [SerializeField] public CircleCollider2D rightHandCollider;
    
    private GameObject rArmAtckInstance;

    [SerializeField] private GameObject rightDamageFeeler;
    [SerializeField] private GameObject leftDamageFeeler;

    [SerializeField] private GameObject shockInstance;

    [Space]
    [Header("Crush Parameters")]
    //[SerializeField] private Rigidbody2D Crush;

    [SerializeField] public GameObject armLeft;

    [SerializeField] public GameObject armRight;
    
    [SerializeField] private float areaSize;
    [SerializeField] private int damage;
    private int punchDamage = 7;
    
    [Space] [Header("EnemySpawnParameters")]
    private int spawningFactor;
    [SerializeField] List<GameObject> enemyPool;
    private int enemySelection;
    public Transform[] spawnPoints;
    private List<Transform> waypointUsed = new List<Transform>();
    //[SerializeField]
    //private int selectorELENNA;
    //[SerializeField]
    //private int selectorAAAAHHH; 
    [Space] [Header("Shoot Parameters")] 
    [SerializeField] private Animator headAnim;
    [SerializeField] private GameObject bossProjectile;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private float fireForce = 100;

    [Space] [Header("Animation Parameters")]
    public Animator crown;
    public Animator leftHand;
    public Animator rightHand;
    public Animator Head;
    public Animator BigEye;
    public Animator EyeB;
    public Animator EyeD;
    public Animator EyeE;
    public Animator EyeF;
    public Animator EyeG;
    public Animator EyeH;
    public Animator EyeI;
    public Animator EyeJ;
    public Animator EyeK;
    public Animator EyeL;
    public Animator EyeM;
    public Animator EyeN;
    public Animator EyeO;
    public Animator[] eyesArray;
    private int regulation = 0;
    

    [Space] [Header("VFX")] 
    public GameObject chargeLeftBoss;
    public GameObject chargeRightBoss;
    public GameObject energyballLeftBoss;
    public GameObject energyballRightBoss;
    public GameObject firstHugeCracksStorms;



    void Start()
    {
        intro.Play();
        quaternionFx = Quaternion.Euler(rotationFx);
        crown.SetInteger("BossHp", hpBoss);
        leftHandCollider.enabled = false;
        hpBoss = maxHPBoss;
        target = PlayerBehaviour.playerBehaviour.transform;

        leftHand.SetBool("Atk", false);
        rightHand.SetBool("Atk", false);
    }

    // Update is called once per frame
    void Update()
    {
        
        crown.SetInteger("BossHp",hpBoss);
        if (hpBoss<=maxHPBoss/2)
        {
            bossIsMidLife = true;
        }
        
    }

    private void FixedUpdate()
    {
        regulation+=1;
        if (regulation==30)
        {
            regulation = 0;
            EyesAnimated();
        }
        if (_frameCounter < refreshTime) _frameCounter += 1;
        else _frameCounter = 0;
        inSecond = (int)(_frameCounter / 50);
        for (int i = 0; i < keyFrames.Count; i++)
        {
            if (_frameCounter != keyFrames[i]) continue;
            switch (bossIsMidLife)
            {
                case false:
                    BehaviourSelector();
                    break;
                case true:
                    EnragedBehaviour();
                    break;
            }
        }
    }

    void BehaviourSelector()
    {
        define = 5; // Range(1, 6);
        //if (selectorELENNA != 0)
          //  define = selectorELENNA;
        //StartCoroutine("BossShooting");
        switch(define)
        {
            case 1:
                
                   StartCoroutine(LeftArmAtck());
                   StartCoroutine(BossShooting());
                   break;
            case 2:
                
                StartCoroutine(RightArmAtck());
                StartCoroutine(BossShooting());
                break;
            
            case 3 :
                StartCoroutine(BossShooting());
                StartCoroutine(BossShooting());
                break;
            case 4 : 
                BossMakesEnemiesSpawn();
                break;
            case 5 :
                StartCoroutine(Crush());
                break;
        }
    }

    void EnragedBehaviour()
    {
        define = Range(1, 6);
        //if (selectorAAAAAHHHH != 0)
          //  define = selectorAAAAAHHHH;
          switch (define)
          {
              case 1:

                  StartCoroutine(BossShooting());
                  StartCoroutine(Crush());

                  break;
              case 2:
                    StartCoroutine(LeftArmAtck());
                      StartCoroutine(RightArmAtck());
                  break;
              
              case 3:
                  BossMakesEnemiesSpawn();
                  StartCoroutine(BossShooting());
                  break;
              case 4:
                  StartCoroutine(RightArmAtck());
                  StartCoroutine(BossShooting());
                  break;
              case 5:
                  StartCoroutine(BossShooting());
                  StartCoroutine(BossShooting());
                  break;
          }
    }

    IEnumerator LeftArmAtck()
    {
        leftHand.SetBool("Atk",true);
        leftHand.SetBool("Slam",true);
        Debug.Log("Je mactiv");
        //Instantiate pour la paume de main gauche avec la charge et la boule d'energie 1rst
        Instantiate(chargeLeftBoss, leftHand.transform.position- new Vector3(0,6,0), Quaternion.identity);
        
        yield return new WaitForSeconds(.9f);
        Instantiate(energyballLeftBoss, leftHand.transform.position- new Vector3(0,6,0), Quaternion.identity);
        Instantiate(shockInstance, leftHand.transform.position - new Vector3(0, 6, 0), Quaternion.identity);
        leftHand.SetBool("Atk", false);
        leftHand.SetBool("Slam", false);
        leftDamageFeeler.SetActive(true);
        yield return new WaitForSeconds(2.6f);
        Debug.Log("C re moi");
        //Instantiate pour la paume de main gauche avec la charge et la boule d'energie 2nd
        leftDamageFeeler.SetActive(false);
      

    }
    IEnumerator RightArmAtck()
    {
        rightHand.SetBool("Atk",true);
        rightHand.SetBool("Slam",true);
        //Instantiate pour la paume de main droite avec la charge et la boule d'energie 1rst
        Instantiate(chargeRightBoss, rightHand.transform.position- new Vector3(0,6,0), quaternion.identity);
        
        yield return new WaitForSeconds(.9f);
        Instantiate(energyballRightBoss, rightHand.transform.position- new Vector3(0,6,0), quaternion.identity);
        Instantiate(shockInstance, rightHand.transform.position- new Vector3(0,6,0), quaternion.identity);
        leftHand.SetBool("Atk", false);
        leftHand.SetBool("Slam", false);
        rightDamageFeeler.SetActive(true);
        yield return new WaitForSeconds(2.6f);
        //Instantiate pour la paume de main droite avec la charge et la boule d'energie 2nd
        rightDamageFeeler.SetActive(false);

    }

    IEnumerator Crush()
    {
        Vector3 originArmRight = armRight.transform.position;
        Vector3 originArmLeft = armLeft.transform.position;
        leftHand.SetBool("Atk", true);
        rightHand.SetBool("Atk",true);
        rightHand.SetBool("Crush",true);
        leftHand.SetBool("Crush",true);
        //Debug.Log("Premier");
        
        armRight.transform.position = (target.transform.position + (new Vector3(3.5f, 10,0)));
        armLeft.transform.position = target.transform.position + (new Vector3(-3.5f, 10,0));
        //Instantiate hugeCracks gauche et droite 1rst
        Instantiate(firstHugeCracksStorms, armRight.transform.position+new Vector3(0,-9.5f,0), quaternionFx);
        Instantiate(firstHugeCracksStorms, armLeft.transform.position+new Vector3(0,-9.5f,0), quaternionFx);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(armRight.transform.position+new Vector3(0,-9.5f,0),areaSize);
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(armLeft.transform.position+new Vector3(0,-9.5f,0),areaSize);
        
        yield return new WaitForSeconds(.75f);
        //Debug.Log("Second");
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                PlayerBehaviour.playerBehaviour.TakeDamage(punchDamage);
            }

            if (enemy.gameObject.CompareTag("Target"))
            {
                enemy.GetComponent<DamageManager>().TakeDamage(punchDamage);
            }
        }

        foreach (Collider2D enemy in hitEnemies2)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                PlayerBehaviour.playerBehaviour.TakeDamage(punchDamage);
            }

            if (enemy.gameObject.CompareTag("Target"))
            {
                enemy.GetComponent<DamageManager>().TakeDamage(punchDamage);
            }
        }

        leftHand.SetBool("Atk", false);
        rightHand.SetBool("Atk",false);
        rightHand.SetBool("Crush",false);
        leftHand.SetBool("Crush",false);

        yield return new WaitForSeconds(1.25f);
        //Debug.Log("Third");
        rightDamageFeeler.SetActive(true);
        leftDamageFeeler.SetActive(true);
        yield return new WaitForSeconds(2.8f);
        rightDamageFeeler.SetActive(false);
        leftDamageFeeler.SetActive(false);


        armRight.transform.position = originArmRight;
        armLeft.transform.position = originArmLeft;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaSize);
    }

    public void TakeDamage(int damage)
    {
        hpBoss -= damage;

    }

    void BossMakesEnemiesSpawn()
    {
        if(bossIsMidLife)
        {
            spawningFactor = Range(2, 4);
            for (int e = 0; e < spawningFactor; e++)
            {
                enemySelection = Range(0, 3);
                Instantiate(enemyPool[enemySelection].gameObject,GetRandomPoint().position, Quaternion.identity);
            }
        }
        else
        {
            spawningFactor = Range(1, 2);
            for (int e = 0; e < spawningFactor; e++)
            {
                enemySelection = Range(0, 3);
                Instantiate(enemyPool[enemySelection].gameObject,GetRandomPoint().position, Quaternion.identity); 
            }
        }
        
    }
    Transform GetRandomPoint()
    {
        Transform randomPoint = null;
        int index = Range(0, spawnPoints.Length);

        while (waypointUsed.Contains(spawnPoints[index]))
        {
            index = Range(0, spawnPoints.Length);
        }
        
        randomPoint = spawnPoints[index];
        waypointUsed.Add(spawnPoints[index]);

        return randomPoint;
    }
    
    IEnumerator BossShooting()
    {
        headAnim.SetBool("fire",true);
        yield return new WaitForSeconds(.8f);
        if (bossIsMidLife)
        {
            foreach (Transform q in firePoints)
            {
                for (int p = 0; p < 2; p++)
                {
                    Vector2 toplayer = (target.transform.position - q.position).normalized;
                    Instantiate(bossProjectile, q.position, gameObject.transform.parent.rotation);
                    bossProjectile.GetComponent<Rigidbody2D>().AddForce(toplayer.normalized*fireForce);
                    yield return new WaitForSeconds(.2f);
                }
            }
            
        }
        else
        {
            foreach (Transform q in firePoints)
            {
                    Vector2 toplayer = (target.transform.position - q.position).normalized;
                    
                    GameObject currentProjectile = Instantiate(bossProjectile, q.position, q.rotation, q);
                    currentProjectile.transform.rotation = currentProjectile.transform.parent.rotation;
                    //bossProjectile.GetComponent<Rigidbody2D>().AddForce(toplayer*fireForce, ForceMode2D.Impulse);
            }
        }

        yield return new WaitForSeconds(.1f);
        headAnim.SetBool("fire",false);
        yield return new WaitForSeconds(1f);

    }

    private void EyesAnimated()
    {
        int rand = Random.Range(0, 14);
        Animator current = eyesArray[rand];
        current.SetTrigger("Wink");
    }

}
