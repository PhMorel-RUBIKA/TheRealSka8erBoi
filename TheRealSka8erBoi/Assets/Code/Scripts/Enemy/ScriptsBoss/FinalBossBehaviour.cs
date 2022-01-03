using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Random;
using Random = UnityEngine;

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
    private GameObject rArmAtckInstance;
    private bool leftArmReady=true;
    private bool rightArmReady=true;
    [SerializeField] private float leftArmRecover = 10;
    [SerializeField] private float rightArmRecover = 10;
    [SerializeField] private float armRecoverInit = 10;
    [Space] [Header("Crush Parameters")]
    //[SerializeField] private Rigidbody2D Crush;
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
    [SerializeField] private GameObject bossProjectile;
    public Transform[] firePoints;
    private List<Transform> firePointsUsed = new List<Transform>();
    [SerializeField] private float fireForce = 100;
    



    void Start()
    {
        hpBoss = maxHPBoss;
        target = PlayerBehaviour.playerBehaviour.transform;
    }

    // Update is called once per frame
    void Update()
    {
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
        leftArmReady = false;
        yield return new WaitForSeconds(.3f);
        lArmAtckInstance=Instantiate(armAtck, new Vector2(target.position.x-6,target.position.y), Quaternion.identity);
        Destroy(lArmAtckInstance,3f);
        
    }
    IEnumerator RightArmAtck()
    {
        rightArmReady = false;
        yield return new WaitForSeconds(.3f);
        rArmAtckInstance=Instantiate(armAtck, new Vector2(target.position.x+6,target.position.y), Quaternion.identity);
        Destroy(rArmAtckInstance,3f);
    }

    IEnumerator Crush()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.transform.position,areaSize);
        yield return new WaitForSeconds(1f);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                PlayerBehaviour.playerBehaviour.TakeDamage(damage);
            }
        }
    }
    
        void BossMakesEnemiesSpawn()
    {
        if(bossIsMidLife)
        {
            spawningFactor = Range(2, 4);
            for (int e = 0; e < spawningFactor; e++)
            {
                enemySelection = Range(0, 5);
                Instantiate(enemyPool[enemySelection].gameObject,GetRandomPoint().position, Quaternion.identity);
            }
        }
        else
        {
            spawningFactor = Range(1, 2);
            for (int e = 0; e < spawningFactor; e++)
            {
                enemySelection = Range(0, 5);
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
        if (bossIsMidLife)
        {
            for (int p = 0; p < 2; p++)
            { 
                Transform randomFirePosition = GetRandomFirePoint();
                Vector2 toplayer = (target.transform.position - randomFirePosition.position).normalized;
                float rotZ = Mathf.Atan2(toplayer.y, toplayer.x) * Mathf.Rad2Deg;
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
      
    }
    
}
