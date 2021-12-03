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
    [Space]
    
    [Header("Behaviour")]
    private Transform target;
    public int hpBoss;
    [SerializeField] private int maxHPBoss;
    public bool bossIsMidLife=false;
    [Space]
    [Header("ArmAttack Parameters")]
    [SerializeField] private GameObject armAtck;
    private GameObject lArmAtckInstance;
    private GameObject rArmAtckInstance;
    private bool leftArmReady=true;
    private bool rightArmReady=true;
    [SerializeField] private float leftArmRecover = 10;
    [SerializeField] private float rightArmRecover = 10;
    [SerializeField] private float armRecoverInit = 10;
    [Space]
    
    [Header("Punch Parameters")]
    [SerializeField] private Rigidbody2D punch;
    [SerializeField] private float areaSize;
    [SerializeField] private int damage;
    private int punchDamage = 15;
    
    

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
                BossShooting();
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
                else if(rightArmReady)
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
                        BossShooting();
                    }
                }
                else
                {
                    StartCoroutine(Crush());
                    BossShooting();
                }
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
        
    }

    void BossShooting()
    {
        
    }
}
