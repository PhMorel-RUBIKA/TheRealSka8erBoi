using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Random;
using Random = UnityEngine.Random;

public class FinalBossBehaviour : MonoBehaviour
{
    
    [Header("Show Value")]
    [SerializeField] private int inSecond;
    [SerializeField] private int _frameCounter;
    [Space(20)]
    public int refreshTime = 1500;

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

    [Space] [Header("Shoot Parameters")] 
    [SerializeField] private Animator headAnim;
    [SerializeField] private GameObject bossProjectile;
    public Transform[] firePoints;
    private List<Transform> firePointsUsed = new List<Transform>();
    [SerializeField] private float fireForce = 100;

    [Space] [Header("Animation Parameters")]
    public Animator crown;
    public Animator leftHand;
    public Animator rightHand;
    public Animator Head;
    public Animator BigEye;
    public Animator EyeB;
    public Animator EyeC;
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

    void Start()
    {
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



        crown.SetFloat("BossHp",hpBoss);
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
        if (regulation==15)
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
        define = Range(1, 5);
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
        define = Range(1, 8);
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
        leftHand.SetTrigger("Slam");
        leftArmReady = false;
        yield return new WaitForSeconds(.9f);
        leftHandCollider.enabled = true;
        lArmAtckInstance=Instantiate(armAtck, new Vector2(target.position.x-6,target.position.y), Quaternion.identity);
        Destroy(lArmAtckInstance,3f);
        yield return new WaitForSeconds(1.6f);
        leftHandCollider.enabled = false;
        leftHand.SetBool("Atk", false);
        
    }
    IEnumerator RightArmAtck()
    {
        rightHand.SetBool("Atk",true);
        rightHand.SetTrigger("Slam");
        rightArmReady = false;
        yield return new WaitForSeconds(.9f);
        rightHandCollider.enabled = true;
        rArmAtckInstance=Instantiate(armAtck, new Vector2(target.position.x+6,target.position.y), Quaternion.identity);
        Destroy(rArmAtckInstance,3f);
        yield return new WaitForSeconds(1.6f);
        rightHandCollider.enabled = false;
        rightHand.SetBool("Atk", false);
    }

    IEnumerator Crush()
    {
        leftHand.SetBool("Atk", true);
        rightHand.SetBool("Atk",true);
        rightHand.SetTrigger("Crush");
        leftHand.SetTrigger("Crush");
        armLeft.transform.position = (target.transform.position * (new Vector2(5, 0)));
        armRight.transform.position = target.transform.position * (new Vector2(-5, 0));
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.transform.position,areaSize);
        yield return new WaitForSeconds(1.5f);
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
        leftHandCollider.enabled = true;
        rightHandCollider.enabled = true;
        yield return new WaitForSeconds(1.5f);
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        leftHand.SetBool("Atk", false);
        rightHand.SetBool("Atk",false);
        
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

    Transform GetRandomFirePoint()
    {
        int index = Range(0, firePoints.Length);

        while (firePointsUsed.Contains(firePoints[index]))
        {
            index = Range(0, firePoints.Length);
        }
        
        Transform randomFirePoint = firePoints[index];
        firePointsUsed.Add(firePoints[index]);

        return randomFirePoint;
    }
    IEnumerator BossShooting()
    {
        headAnim.SetBool("fire",true);
        yield return new WaitForSeconds(.7f);
        if (bossIsMidLife)
        {
            for (int p = 0; p < 2; p++)
            { 
                Transform randomFirePosition = GetRandomFirePoint();
                Vector2 toplayer = (target.transform.position - randomFirePosition.position).normalized;
                Instantiate(bossProjectile, randomFirePosition);
                bossProjectile.GetComponent<Rigidbody2D>().AddForce(toplayer.normalized*fireForce);
                yield return new WaitForSeconds(.2f);
            }
        }
        else
        {
            Transform randomFirePosition = GetRandomFirePoint();
            Vector2 toplayer = (target.transform.position - randomFirePosition.position).normalized;
            float rotZ = Mathf.Atan2(toplayer.y, toplayer.x) * Mathf.Rad2Deg;
            Instantiate(bossProjectile, randomFirePosition);
            bossProjectile.GetComponent<Rigidbody2D>().AddForce(toplayer.normalized*fireForce);
        }

        yield return new WaitForSeconds(.1f);
        headAnim.SetBool("fire",false);

    }

    private void EyesAnimated()
    {
        int rand = Random.Range(0, 15);
        Animator current = eyesArray[rand];
        current.SetTrigger("Wink");
    }

}
