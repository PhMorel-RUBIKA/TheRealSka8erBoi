using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Random;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UIElements.Slider;

public class FinalBossBehaviour : MonoBehaviour
{

    [Header("Show Value")] 
    [SerializeField] private Image bossBar;
    [SerializeField] private int inSecond;
    [SerializeField] private int _frameCounter;
    [Space(20)]
    public int refreshTime = 1500;

    private Quaternion quaternionFx;
    private  Vector3 rotationFx = new Vector3(-90, 0, 0); 
    public List<int> keyFrames=new List<int>();

    public int define;
    public int decrementalValue;

    [Space] [Header("Behaviour")] 
    [SerializeField] private Animation intro;
    private Transform target;
    public int hpBoss;
    [SerializeField] private int maxHPBoss;
    public bool bossIsMidLife=false;
    public List<BossAttack> bossAttacks = new List<BossAttack>();

    [Space] [Header("ArmAttack Parameters")]
    [SerializeField] private GameObject armAtck;
    private GameObject lArmAtckInstance;
    public int frameDelayBetweenAttack;
    
    [SerializeField] public CircleCollider2D leftHandCollider;
    [SerializeField] public CircleCollider2D rightHandCollider;
    
    private GameObject rArmAtckInstance;

    [SerializeField] private GameObject rightDamageFeeler;
    [SerializeField] private GameObject leftDamageFeeler;

    [SerializeField] private GameObject shockInstance;

    [Space]
    [Header("Crush Parameters")]

    [SerializeField] public GameObject armLeft;

    [SerializeField] public GameObject armRight;
    
    [SerializeField] private float areaSize;
    [SerializeField] private int damage;
    [SerializeField] private int punchDamage = 7;
    
    [Space] [Header("EnemySpawnParameters")]
    private int spawningFactor;
    [SerializeField] List<GameObject> enemyPool;
    private int enemySelection;
    public Transform[] spawnPoints;
    private List<Transform> waypointUsed = new List<Transform>();

    [Space] [Header("Shoot Parameters")] 
    [SerializeField] private Animator headAnim;
    [SerializeField] private GameObject bossProjectile;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private float fireForce = 100;

    [Space] [Header("Animation Parameters")]
    public Animator crown;
    public Animator leftHand;
    public Animator rightHand;

    public Animator[] eyesArray;
    private int regulation = 0;
    

    [Space] [Header("VFX")] 
    public GameObject chargeLeftBoss;
    public GameObject chargeRightBoss;
    public GameObject energyballLeftBoss;
    public GameObject energyballRightBoss;
    public GameObject firstHugeCracksStorms;

    private int currentKeyframe;
    public Light2D haloLumière;
    public List<float> stepLifeBossList;
    public SpriteRenderer couronne;
    
    void Start()
    {
        float stepLifeBoss = (float)maxHPBoss / 6;
        stepLifeBossList = new List<float>();
        for (int i = 1; i < 7; i++) stepLifeBossList.Add(stepLifeBoss*i);

        intro.Play();
        quaternionFx = Quaternion.Euler(rotationFx);
        crown.SetInteger("BossHp", hpBoss);
        leftHandCollider.enabled = false;
        hpBoss = maxHPBoss;
        target = PlayerBehaviour.playerBehaviour.transform;

        leftHand.SetBool("Atk", false);
        rightHand.SetBool("Atk", false);
    }
    
    void Update()
    {
        float division = (float)hpBoss/maxHPBoss;
        float value = Mathf.Clamp(division, 0, 1);
        bossBar.fillAmount = value;
        crown.SetInteger("BossHp",hpBoss);
        if (hpBoss<=maxHPBoss/2)
        {
            bossBar.color = new Color(255,119,117,255);
            bossIsMidLife = true;
        }

        ChangeLifeInformation();
        if (hpBoss<=0) { PlayerBehaviour.playerBehaviour.WinningCharacter(); }
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
        if (_frameCounter != keyFrames[0]) return;
        
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

     private void ChangeLifeInformation()
     {
         haloLumière.intensity = Mathf.Lerp(haloLumière.intensity, hpBoss*2.5f / maxHPBoss, 0.01f);
     }

    void BehaviourSelector()
    {
        define = GetRandomAttack();
        switch(define)
        {
            case 1: 
                StartCoroutine(LeftArmAtck());
                bossAttacks[0].probability -= decrementalValue;
                bossAttacks[1].probability += decrementalValue / 4;
                bossAttacks[2].probability += decrementalValue / 4;
                bossAttacks[3].probability += decrementalValue / 4;
                bossAttacks[4].probability += decrementalValue / 4;
                foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                keyFrames[0] += bossAttacks[0].frameLength + frameDelayBetweenAttack;
                break;
            case 2: 
                StartCoroutine(RightArmAtck());
                bossAttacks[0].probability += decrementalValue / 4;
                bossAttacks[1].probability -= decrementalValue;
                bossAttacks[2].probability += decrementalValue / 4;
                bossAttacks[3].probability += decrementalValue / 4;
                bossAttacks[4].probability += decrementalValue / 4;
                foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                keyFrames[0] += bossAttacks[1].frameLength + frameDelayBetweenAttack;
                break;
            case 3 :
                StartCoroutine(BossShooting());
                bossAttacks[0].probability += decrementalValue / 4;
                bossAttacks[1].probability += decrementalValue / 4;
                bossAttacks[2].probability -= decrementalValue;
                bossAttacks[3].probability += decrementalValue / 4;
                bossAttacks[4].probability += decrementalValue / 4;
                foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                keyFrames[0] += bossAttacks[2].frameLength + frameDelayBetweenAttack;
                break;
            case 4 : 
                BossMakesEnemiesSpawn();
                bossAttacks[0].probability += decrementalValue / 4;
                bossAttacks[1].probability += decrementalValue / 4;
                bossAttacks[2].probability += decrementalValue / 4;
                bossAttacks[3].probability -= decrementalValue;
                bossAttacks[4].probability += decrementalValue / 4;
                foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                keyFrames[0] += bossAttacks[3].frameLength + frameDelayBetweenAttack;
                break;
            case 5 :
                StartCoroutine(Crush());
                bossAttacks[0].probability += decrementalValue / 4;
                bossAttacks[1].probability += decrementalValue / 4;
                bossAttacks[2].probability += decrementalValue / 4;
                bossAttacks[3].probability += decrementalValue / 4;
                bossAttacks[4].probability -= decrementalValue;
                foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                keyFrames[0] += bossAttacks[4].frameLength + frameDelayBetweenAttack;
                break;
        }
    }

    void EnragedBehaviour()
    {
        define = GetRandomAttack();
        switch (define)
          {
              case 1:
                  StartCoroutine(BossShooting());
                  StartCoroutine(Crush());
                  bossAttacks[0].probability -= decrementalValue;
                  bossAttacks[1].probability += decrementalValue / 4;
                  bossAttacks[2].probability += decrementalValue / 4;
                  bossAttacks[3].probability += decrementalValue / 4;
                  bossAttacks[4].probability += decrementalValue / 4;
                  foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                  keyFrames[0] += bossAttacks[4].frameLength + frameDelayBetweenAttack;
                  break;
              case 2:
                  StartCoroutine(LeftArmAtck());
                  StartCoroutine(RightArmAtck());
                  bossAttacks[0].probability += decrementalValue / 4;
                  bossAttacks[1].probability -= decrementalValue;
                  bossAttacks[2].probability += decrementalValue / 4;
                  bossAttacks[3].probability += decrementalValue / 4;
                  bossAttacks[4].probability += decrementalValue / 4;
                  foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                  keyFrames[0] += bossAttacks[0].frameLength + frameDelayBetweenAttack;
                  break;
              case 3:
                  BossMakesEnemiesSpawn();
                  StartCoroutine(BossShooting());
                  bossAttacks[0].probability += decrementalValue / 4;
                  bossAttacks[1].probability += decrementalValue / 4;
                  bossAttacks[2].probability -= decrementalValue;
                  bossAttacks[3].probability += decrementalValue / 4;
                  bossAttacks[4].probability += decrementalValue / 4;
                  foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                  keyFrames[0] += bossAttacks[2].frameLength + frameDelayBetweenAttack;
                  break;
              case 4:
                  StartCoroutine(RightArmAtck());
                  StartCoroutine(BossShooting());
                  bossAttacks[0].probability += decrementalValue / 4;
                  bossAttacks[1].probability += decrementalValue / 4;
                  bossAttacks[2].probability += decrementalValue / 4;
                  bossAttacks[3].probability -= decrementalValue;
                  bossAttacks[4].probability += decrementalValue / 4;
                  foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                  keyFrames[0] += bossAttacks[1].frameLength + frameDelayBetweenAttack;
                  break;
              case 5:
                  StartCoroutine(BossShooting());
                  StartCoroutine(BossShooting());
                  bossAttacks[0].probability += decrementalValue / 4;
                  bossAttacks[1].probability += decrementalValue / 4;
                  bossAttacks[2].probability += decrementalValue / 4;
                  bossAttacks[3].probability += decrementalValue / 4;
                  bossAttacks[4].probability -= decrementalValue;
                  foreach (BossAttack bossAttack in bossAttacks) bossAttack.probability = Mathf.Clamp(bossAttack.probability, 0, 100);
                  keyFrames[0] += bossAttacks[2].frameLength + frameDelayBetweenAttack;
                  break;
          }
    }

    int GetRandomAttack()
    {
        int i = 0;
        int value = 0;
        int random = Random.Range(0,100);

        while ( random > bossAttacks[i].probability + value)
        {
            value += bossAttacks[i].probability;
            i++;
        }

        Debug.Log(bossAttacks[i].id);
        return bossAttacks[i].id;
    }

    IEnumerator LeftArmAtck()
    {
        leftHand.SetBool("Atk",true);
        leftHand.SetBool("Slam",true);
        yield return new WaitForSeconds(0.50f);
        leftHand.speed = 0;
        Instantiate(chargeLeftBoss, leftHand.transform.position- new Vector3(0,6,0), Quaternion.identity);
        yield return new WaitForSeconds(.7f);
        leftHand.speed = 1;
        
        
        yield return new WaitForSeconds(.3f);
        Instantiate(energyballLeftBoss, leftHand.transform.position- new Vector3(0,6,0), Quaternion.identity);
        Instantiate(shockInstance, leftDamageFeeler.transform.position - new Vector3(0, 6, 0), Quaternion.identity);
        leftHand.SetBool("Atk", false);
        leftHand.SetBool("Slam", false);
        leftDamageFeeler.SetActive(true);
        yield return new WaitForSeconds(1f);
        leftHand.speed = 0;
        yield return new WaitForSeconds(1.5f);
        leftHand.speed = 1;
        yield return new WaitForSeconds(1.6f);

        leftDamageFeeler.SetActive(false);

    }
    IEnumerator RightArmAtck()
    {
        rightHand.SetBool("Atk",true);
        rightHand.SetBool("Slam",true);
        yield return new WaitForSeconds(0.50f);
        rightHand.speed = 0;
        Instantiate(chargeRightBoss, rightHand.transform.position- new Vector3(0,6,0), quaternion.identity);
        yield return new WaitForSeconds(.7f);
        rightHand.speed = 1;
        
        
        yield return new WaitForSeconds(.3f);
        Instantiate(energyballRightBoss, rightHand.transform.position- new Vector3(0,6,0), quaternion.identity);
        Instantiate(shockInstance, rightHand.transform.position- new Vector3(0,6,0), quaternion.identity);
        rightHand.SetBool("Atk", false);
        rightHand.SetBool("Slam", false);
        rightDamageFeeler.SetActive(true);
        yield return new WaitForSeconds(1f);
        rightHand.speed = 0;
        yield return new WaitForSeconds(1.5f);
        rightHand.speed = 1;
        yield return new WaitForSeconds(1.6f);
        rightDamageFeeler.SetActive(false);

    }

    IEnumerator Crush()
    {
        Vector3 originLeftFeel = leftDamageFeeler.transform.position;
        Vector3 originRightFeel = rightDamageFeeler.transform.position;
        Vector3 originArmRight = armRight.transform.position;
        Vector3 originArmLeft = armLeft.transform.position;
        leftHand.SetBool("Atk", true);
        rightHand.SetBool("Atk",true);
        rightHand.SetBool("Crush",true);
        leftHand.SetBool("Crush",true);
        yield return new WaitForSeconds(0.30f);
        rightHand.speed = 0;
        leftHand.speed = 0;
        armRight.transform.position = (target.transform.position + (new Vector3(3.5f, 10,0)));
        armLeft.transform.position = target.transform.position + (new Vector3(-3.5f, 10,0));
        yield return new WaitForSeconds(1f);
        rightHand.speed = 1;
        leftHand.speed = 1;

        Instantiate(firstHugeCracksStorms, armRight.transform.position+new Vector3(0,-9.5f,0), quaternionFx);
        Instantiate(firstHugeCracksStorms, armLeft.transform.position+new Vector3(0,-9.5f,0), quaternionFx);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(armRight.transform.position+new Vector3(0,-9.5f,0),areaSize);
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(armLeft.transform.position+new Vector3(0,-9.5f,0),areaSize);
        leftDamageFeeler.transform.position = armLeft.transform.position+new Vector3(0,-7.5f,0);
        rightDamageFeeler.transform.position= armRight.transform.position+new Vector3(0,-7.5f,0);
        
        yield return new WaitForSeconds(.75f);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player")) PlayerBehaviour.playerBehaviour.TakeDamage(punchDamage);
            if (enemy.gameObject.CompareTag("Target")) enemy.GetComponent<DamageManager>().TakeDamage(punchDamage);
        }

        foreach (Collider2D enemy in hitEnemies2)
        {
            if (enemy.gameObject.CompareTag("Player")) PlayerBehaviour.playerBehaviour.TakeDamage(punchDamage);
            if (enemy.gameObject.CompareTag("Target")) enemy.GetComponent<DamageManager>().TakeDamage(punchDamage);
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
        rightDamageFeeler.transform.position = originRightFeel;
        leftDamageFeeler.transform.position = originLeftFeel;
        
        armRight.transform.position = originArmRight;
        armLeft.transform.position = originArmLeft;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(leftDamageFeeler.transform.position, areaSize);
    }

    public void TakeDamage(int damage)
    {
        hpBoss -= damage;
    }
    void BossMakesEnemiesSpawn()
    {
        int spawningFactor = 1;

        if (hpBoss >= maxHPBoss- stepLifeBossList[0]) spawningFactor = 1;
        if (hpBoss <  maxHPBoss-stepLifeBossList[0] && hpBoss >=  maxHPBoss-stepLifeBossList[1]) spawningFactor = 2;
        if (hpBoss <  maxHPBoss-stepLifeBossList[1] && hpBoss >=  maxHPBoss-stepLifeBossList[2]) spawningFactor = 3;
        if (hpBoss <  maxHPBoss-stepLifeBossList[2] && hpBoss >=  maxHPBoss-stepLifeBossList[3]) spawningFactor = 4;
        if (hpBoss < maxHPBoss- stepLifeBossList[3] && hpBoss >=  maxHPBoss-stepLifeBossList[4]) spawningFactor = 5;
        if (hpBoss < maxHPBoss-stepLifeBossList[4] && hpBoss >= maxHPBoss-stepLifeBossList[5]) spawningFactor = 6;
        //if (hpBoss < maxHPBoss - stepLifeBossList[5] && hpBoss >= maxHPBoss - stepLifeBossList[6]) spawningFactor = 7;
        //if (hpBoss < maxHPBoss - stepLifeBossList[6] && hpBoss >= maxHPBoss - stepLifeBossList[7]) spawningFactor = 8;

        for (int e = 0; e < spawningFactor; e++)
        {
            enemySelection = Range(0, 4);
            Instantiate(enemyPool[enemySelection].gameObject,GetRandomPoint().position, Quaternion.identity);
        }
    }
    Transform GetRandomPoint()
    {
        Transform randomPoint = null;
        int index = Range(0, spawnPoints.Length);

        while (waypointUsed.Contains(spawnPoints[index])) { index = Range(0, spawnPoints.Length); }
        
        randomPoint = spawnPoints[index];
        waypointUsed.Add(spawnPoints[index]);

        return randomPoint;
    }
    
    IEnumerator BossShooting()
    {
        yield return new WaitForSeconds(1f);
        headAnim.SetBool("fire",true);
        yield return new WaitForSeconds(.8f);
        headAnim.SetBool("fire",false);

        int spawningFactor = 1;
        
        if (hpBoss >= maxHPBoss- stepLifeBossList[0]) spawningFactor = 1;
        if (hpBoss <  maxHPBoss-stepLifeBossList[0] && hpBoss >=  maxHPBoss-stepLifeBossList[1]) spawningFactor = 2;
        if (hpBoss <  maxHPBoss-stepLifeBossList[1] && hpBoss >=  maxHPBoss-stepLifeBossList[2]) spawningFactor = 3;
        if (hpBoss <  maxHPBoss-stepLifeBossList[2] && hpBoss >=  maxHPBoss-stepLifeBossList[3]) spawningFactor = 4;
        if (hpBoss < maxHPBoss- stepLifeBossList[3] && hpBoss >=  maxHPBoss-stepLifeBossList[4]) spawningFactor = 5;
        if (hpBoss < maxHPBoss-stepLifeBossList[4] && hpBoss >= maxHPBoss-stepLifeBossList[5]) spawningFactor = 6;
        //if (hpBoss < maxHPBoss - stepLifeBossList[5] && hpBoss >= maxHPBoss - stepLifeBossList[6]) spawningFactor = 7;
        //if (hpBoss < maxHPBoss - stepLifeBossList[6] && hpBoss >= maxHPBoss - stepLifeBossList[7]) spawningFactor = 8;
        
        for (int i = 0; i < spawningFactor; i++)
        {
            foreach (Transform q in firePoints)
            {
                Vector2 toplayer = (target.transform.position - q.position).normalized;
                    
                GameObject currentProjectile = Instantiate(bossProjectile, q.position, q.rotation, q);
                currentProjectile.transform.rotation = currentProjectile.transform.parent.rotation;
                bossProjectile.GetComponent<Rigidbody2D>().AddForce(toplayer*fireForce, ForceMode2D.Impulse);
            }
            
            yield return new WaitForSeconds(0.5f);
        }
            
        yield return new WaitForSeconds(.1f);
        

    }

    private void EyesAnimated()
    {
        int rand = Random.Range(0, 14);
        Animator current = eyesArray[rand];
        current.SetTrigger("Wink");
    }

}

[Serializable]
public class BossAttack
{
    public int id;
    public string name;
    public int frameLength;
    [Range(0, 100)] public int probability;
}
