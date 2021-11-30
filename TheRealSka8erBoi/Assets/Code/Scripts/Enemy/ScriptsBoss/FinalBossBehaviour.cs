using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour
{
    [Header("Behaviour")]
    private Transform target;
    public int hpBoss;
    [SerializeField] private int maxHPBoss;
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

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerBehaviour.playerBehaviour.transform;
    }

    // Update is called once per frame
    void Update()
    {
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

    IEnumerator LeftArmAtck()
    {
        leftArmReady = false;
        yield return new WaitForSeconds(.3f);
        lArmAtckInstance=Instantiate(armAtck, new Vector2(target.position.x-6,target.position.y), quaternion.identity);
        Destroy(lArmAtckInstance,3f);
        
    }

    IEnumerator RightArmAtck()
    {
        rightArmReady = false;
        yield return new WaitForSeconds(.3f);
        rArmAtckInstance=Instantiate(armAtck, new Vector2(target.position.x+6,target.position.y), quaternion.identity);
        Destroy(rArmAtckInstance,3f);
    }

    IEnumerator PunchAtck()
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
        
}
