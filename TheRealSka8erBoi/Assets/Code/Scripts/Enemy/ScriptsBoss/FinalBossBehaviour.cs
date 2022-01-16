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
    private bool leftArmReady=true;
    private bool rightArmReady=true;
    [SerializeField] private float leftArmRecover = 10;
    [SerializeField] private float rightArmRecover = 10;
    [SerializeField] private float armRecoverInit = 10;

    [Space]
    [Header("Crush Parameters")]
    //[SerializeField] private Rigidbody2D Crush;

    [SerializeField] public GameObject armLeft;

    [SerializeField] public GameObject armRight;
    
    [SerializeField] private float areaSize;
    [SerializeField] private int damage;
    private int punchDamage = 10;
    
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
    //public GameObject hugeCracksStorms;
    public GameObject firstHugeCracksStorms;
    //public Transform jeSuisLaGauche;
    //public Transform jeSuisLaDroite;
    
    

    void Start()
    {
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
        if (!leftArmReady)
        {
            leftArmRecover= -Time.deltaTime;
            if (leftArmRecover <= 0)
            {
                leftArmReady = true;
                leftArmRecover = armRecoverInit;
            }
        }

        if (!rightArmReady)
        {
            rightArmRecover = -Time.deltaTime;
            if (rightArmRecover <= 0)
            {
                rightArmReady = true;
                rightArmRecover = armRecoverInit;
            }
        }
        if (Input.GetKeyDown(KeyCode.L)&leftArmReady)
        {
            StartCoroutine(LeftArmAtck());
        }

        if (Input.GetKeyDown(KeyCode.M) & rightArmReady)
        {
            StartCoroutine(RightArmAtck());
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
        define = Range(1, 6);
        //if (selectorELENNA != 0)
          //  define = selectorELENNA;
        //StartCoroutine("BossShooting");
        switch(define)
        {
            case 1:
                if (leftArmReady)
                {
                   StartCoroutine(LeftArmAtck()); 
                }
                else if (rightArmReady)
                {
                    StartCoroutine(RightArmAtck());
                }
                else
                {
                    StartCoroutine(Crush());
                }
                break;
            case 2:
                if (rightArmReady)
                {
                    StartCoroutine(RightArmAtck());
                }
                else if (leftArmReady)
                {
                    StartCoroutine(LeftArmAtck());
                }
                else
                {
                    StartCoroutine(Crush());
                }
                break;
            case 3 :
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
        define = Range(1, 9);
        //if (selectorAAAAAHHHH != 0)
          //  define = selectorAAAAAHHHH;
        switch (define)
        {
            case 1:
                if (leftArmReady)
                {
                    StartCoroutine(LeftArmAtck());
                    StartCoroutine(Crush());
                }
                else if (rightArmReady)
                {
                    StartCoroutine(RightArmAtck());
                    StartCoroutine(Crush());
                }
                else
                {
                    BossMakesEnemiesSpawn();
                    StartCoroutine(Crush());
                }

                break;
            case 2:
                if (leftArmReady)
                {
                    StartCoroutine(LeftArmAtck());
                    if (rightArmReady)
                    {
                        StartCoroutine(RightArmAtck());
                    }
                    else
                    {
                        StartCoroutine(BossShooting());
                    }
                }
                else
                {
                    StartCoroutine(Crush());
                    StartCoroutine(BossShooting());
                }

                break;
            case 3:
                BossMakesEnemiesSpawn();
                if (leftArmReady)
                {
                    StartCoroutine(LeftArmAtck());
                    if (rightArmReady)
                    {
                        StartCoroutine(RightArmAtck());
                    }
                }

                break;
            case 4:
                BossMakesEnemiesSpawn();
                StartCoroutine(BossShooting());
                break;
            case 5:
                StartCoroutine(BossShooting());
                StartCoroutine(Crush());
                break;
            case 6:
                StartCoroutine(RightArmAtck());
                StartCoroutine(LeftArmAtck());
                break;
            case 7 :
                if (leftArmReady)
                {
                    StartCoroutine(LeftArmAtck());
                }
                else if (rightArmReady)
                {
                    StartCoroutine((RightArmAtck()));
                }
                BossMakesEnemiesSpawn();
                break;
            case 8 :
                StartCoroutine(Crush());
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
        Instantiate(chargeLeftBoss, new Vector3(-5, 15, 0), Quaternion.identity);
        Instantiate(energyballLeftBoss, new Vector3(-3, 18, 0), Quaternion.identity);
        leftArmReady = false;
        yield return new WaitForSeconds(.9f);
        leftHandCollider.enabled = true;
        lArmAtckInstance=Instantiate(armAtck, leftHand.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.6f);
        Debug.Log("C re moi");
        //Instantiate pour la paume de main gauche avec la charge et la boule d'energie 2nd
        Instantiate(chargeLeftBoss, new Vector3(-5, 15, 0), Quaternion.identity);
        Instantiate(energyballLeftBoss, new Vector3(-3, 18, 0), Quaternion.identity);
        leftHandCollider.enabled = false;
        leftHand.SetBool("Atk", false);
        leftHand.SetBool("Slam", false);

    }
    IEnumerator RightArmAtck()
    {
        rightHand.SetBool("Atk",true);
        rightHand.SetBool("Slam",true);
        //Instantiate pour la paume de main droite avec la charge et la boule d'energie 1rst
        Instantiate(chargeRightBoss, new Vector3(11, 15, 0), quaternion.identity);
        Instantiate(energyballRightBoss, new Vector3(9, 18, 0), quaternion.identity);
        rightArmReady = false;
        yield return new WaitForSeconds(.9f);
        rightHandCollider.enabled = true;
        rArmAtckInstance=Instantiate(armAtck, rightHand.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.6f);
        //Instantiate pour la paume de main droite avec la charge et la boule d'energie 2nd
        Instantiate(chargeRightBoss, new Vector3(11, 15, 0), quaternion.identity);
        Instantiate(energyballRightBoss, new Vector3(9, 18, 0), quaternion.identity);
        rightHandCollider.enabled = false;
        rightHand.SetBool("Atk", false);
        rightHand.SetBool("Slam", false);
    }

    IEnumerator Crush()
    {
        leftHand.SetBool("Atk", true);
        rightHand.SetBool("Atk",true);
        rightHand.SetBool("Crush",true);
        leftHand.SetBool("Crush",true);
        //Debug.Log("Premier");
        
        armRight.transform.position = (target.transform.position + (new Vector3(11, 2,0)));
        armLeft.transform.position = target.transform.position + (new Vector3(-9, 2,0));
        //Instantiate hugeCracks gauche et droite 1rst
        Instantiate(firstHugeCracksStorms, new Vector3(14, 5, 0), quaternionFx);
        Instantiate(firstHugeCracksStorms, new Vector3(-6, 5, 0), quaternionFx);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.transform.position,areaSize);
        
        yield return new WaitForSeconds(1.5f);
        //Debug.Log("Second");
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                PlayerBehaviour.playerBehaviour.TakeDamage(damage);
            }

            if (enemy.gameObject.CompareTag("Target"))
            {
                enemy.GetComponent<DamageManager>().TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(1.1f);
        //Debug.Log("Third");
        leftHandCollider.enabled = true;
        rightHandCollider.enabled = true;
        yield return new WaitForSeconds(1.5f);
        //Instantiate hugeCracks gauche et droite 2nd
        Instantiate(firstHugeCracksStorms, new Vector3(14, 5, 0), quaternionFx);
        Instantiate(firstHugeCracksStorms, new Vector3(-6, 5, 0), quaternionFx);
        //Debug.Log("Quatre");
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        leftHand.SetBool("Atk", false);
        rightHand.SetBool("Atk",false);
        rightHand.SetBool("Crush",false);
        leftHand.SetBool("Crush",false);
        
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

    }

    private void EyesAnimated()
    {
        int rand = Random.Range(0, 14);
        Animator current = eyesArray[rand];
        current.SetTrigger("Wink");
    }

}
