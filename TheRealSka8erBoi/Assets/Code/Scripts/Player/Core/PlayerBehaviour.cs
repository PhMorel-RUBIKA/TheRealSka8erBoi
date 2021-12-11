using MoreMountains.NiceVibrations;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBehaviour : MonoBehaviour
{
    //Declaration Movement
    private Rigidbody2D playerRigid;
    private SpriteRenderer playerRender;
    [SerializeField]private Vector2 leftJoy;
    private float floatManipulation;
    [HideInInspector]public Vector2 latestDirection;
    
    [Header("Player Control")]
    [SerializeField] int _playerSpeed;

    public int playerSpeed => (int) (_playerSpeed + (BonusManager.instance.greenStat * GreenStatModifier));
    
    [SerializeField] private float deadzoneController = 0.3f;
    [Space]
    
    //Declaration Dash
    [Header("Dash Tweaking")]
    [SerializeField] private float _dashCd;

    public float dashCd => _dashCd - (BonusManager.instance.greenStat * GreenStatModifier);
    public float dashSpeed;
    public float dashDuration;
    private float dashOngoingCd;
    private float dashGoingFor;
    private bool dash = true;
    public bool dashSpellActive = false;
    public int dashSpellactivation;
    public GameObject dashNode;
    public List<GameObject> dashNodeList;
    public GameObject spellDashLaser;
    [Space]
    //Declaration Animation
    private Animator animatorPlayer;
    public List<int> animatorID;

    //Declaration Shoot
    [Header("Shoot Tweaking")]
    [SerializeField] private float timeMaxCharge = 0.9f;
    [SerializeField] private float perfectShootValue;
    [SerializeField] private HapticTypes _hapticTypesForPerfectShoot = HapticTypes.Success;
    [SerializeField] private float _baseDamage;
    public float baseDamage => _baseDamage + (BonusManager.instance.blueStat * BlueStatModifier);
    
    [Space]
    private bool isAiming;
    private float charge;

    private GameObject spawnedProj;

    //Declaration VFXShoot
    [Header("FX Declaration")]
    public GameObject cylindre;
    public GameObject bigMuzzle;
    public GameObject muzzle;
    public GameObject giantMuzzle;

    [Space] [Header("Feedback Declaration")]
    public MMFeedbacks DamageFeedbacks;
    
    [Space]

    //Declaration UI
    [SerializeField] private int _maxHealth = 5;

    public int maxHealth => _maxHealth + (BonusManager.instance.redStat / _maxHealth * 100);
    [HideInInspector] public int currentHealth;
    public Slider healthBar;
    
    [Space(20)]
    
    [Header("BonusManager Attributs")]
    [TextArea] public string descriptonRouge;
    public float RedStatModifier;
    [Space]
    [TextArea] public string descriptonBleu;
    public float BlueStatModifier;
    [Space]
    [TextArea] public string descriptonVert;
    public float GreenStatModifier;

    public static PlayerBehaviour playerBehaviour;


    private void Awake()
    {
        if (playerBehaviour == null)
        {
            playerBehaviour = this;
        }
    }

    private void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerRender = GetComponent<SpriteRenderer>();
        animatorPlayer = GetComponent<Animator>();
        dashGoingFor = dashDuration;
        currentHealth = maxHealth;
        
        
        //Set animatorID
        animatorID.Add(Animator.StringToHash("GoingUp"));
        animatorID.Add(Animator.StringToHash("GoingDown"));
        animatorID.Add(Animator.StringToHash("GoingRightFront"));
        animatorID.Add(Animator.StringToHash("GoingRightBack"));
        animatorID.Add(Animator.StringToHash("GoingLeftFront"));
        animatorID.Add(Animator.StringToHash("GoingLeftBack"));
        animatorID.Add(Animator.StringToHash("Release"));
        animatorID.Add(Animator.StringToHash("Death"));
        animatorID.Add(Animator.StringToHash("IsRunning"));
        animatorID.Add(Animator.StringToHash("IsAiming"));
        animatorID.Add(Animator.StringToHash("Horizontal"));
        animatorID.Add(Animator.StringToHash("Vertical"));
        
    }

    void Update()
    {
        MyInput();
    }

   void FixedUpdate()
    {
        playerRender.sortingOrder = -(int) transform.position.y;
        Ongoing();
        AnimatorManagement();
    }

    private void MyInput()
    {
        leftJoy.x = Input.GetAxisRaw("Horizontal");
        leftJoy.y = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(leftJoy.x) > deadzoneController || Mathf.Abs(leftJoy.y) > deadzoneController)
        {
            latestDirection = leftJoy;
        }
        
        if (Input.GetButtonDown("BowShot"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isAiming = true;
            animatorPlayer.SetBool(animatorID[9], isAiming);
            charge = 0;
        }
        if (Input.GetButtonUp("BowShot") && isAiming)
        {
            Shoot(charge, GetComponentInChildren<CursorBehaviour>().AimDirection().Item2);
            transform.GetChild(0).gameObject.SetActive(false);
            isAiming = false;
            animatorPlayer.SetTrigger(animatorID[6]);//Release
            animatorPlayer.SetBool(animatorID[9], isAiming);
            
        }

        dashOngoingCd -= Time.deltaTime;
        if (Input.GetAxisRaw("Dash") > 0 && !isAiming)
        {
            if (dashOngoingCd <= 0)
            {
                dashOngoingCd = dashCd ;
                dashGoingFor = 0;
                dash = true;
                if (dashSpellActive && dashSpellactivation == 4)
                {
                    dashNodeList.Add(Instantiate(dashNode, transform.position, quaternion.identity));
                }
                    
            }
        }
    }

    private void Ongoing()
    {
        playerRigid.velocity = Vector2.zero;
        {
            if (Mathf.Abs(leftJoy.x) > deadzoneController || Mathf.Abs(leftJoy.y) > deadzoneController)
            {
                floatManipulation = playerRigid.velocity.magnitude; 
                playerRigid.velocity = Vector2.Lerp(playerRigid.velocity,leftJoy,0.35f).normalized * floatManipulation;
            }
        }
        if (!isAiming)
        {
            
            if (Mathf.Abs(leftJoy.x) > deadzoneController || Mathf.Abs(leftJoy.y) > deadzoneController)
            {
                animatorPlayer.SetBool(animatorID[8],true); 
                playerRigid.velocity = leftJoy * playerSpeed;
            }
            else
            {
                animatorPlayer.SetBool(animatorID[8],false);
            }
        }
        else
        {
            animatorPlayer.SetBool(animatorID[8], false);
            if (charge < timeMaxCharge)
            {
                charge += Time.deltaTime;
            }
        }
        
        if (dash)
        {
            if (dashGoingFor > dashDuration)
            {
                playerRigid.velocity = Vector2.zero;
                dash = false;
                if (dashSpellActive)
                {
                    DashSpell();
                }
            }
            else
            {
                dashGoingFor += Time.fixedDeltaTime;
                playerRigid.velocity = new Vector2(leftJoy.x, leftJoy.y) * dashSpeed;
            }
        }
    }

    private void AnimatorManagement()
    {
        if (isAiming)
        {
            animatorPlayer.SetFloat(animatorID[10],GetComponentInChildren<CursorBehaviour>().AimDirection().Item2.x);//Horizontal
            animatorPlayer.SetFloat(animatorID[11],GetComponentInChildren<CursorBehaviour>().AimDirection().Item2.y);//Vertical
        }
        
        if (Mathf.Abs(leftJoy.x) < deadzoneController && leftJoy.y > deadzoneController)
        {// Going Up
            animatorPlayer.SetTrigger(animatorID[0]);
        }
        else if (Mathf.Abs(leftJoy.x) < deadzoneController && leftJoy.y < -deadzoneController)
        {//Going Down
            animatorPlayer.SetTrigger(animatorID[1]);
        }
        else if (leftJoy.x > deadzoneController && leftJoy.y < 0.01f)
        {//GoingRightFront
            animatorPlayer.SetTrigger(animatorID[2]);
        }
        else if (leftJoy.x > deadzoneController && leftJoy.y > 0.01f) 
        {//GoingRightBack
            animatorPlayer.SetTrigger(animatorID[3]); 
        }
        else if (leftJoy.x < -deadzoneController && leftJoy.y < 0.01f)
        {//GoingLeftFront
            animatorPlayer.SetTrigger(animatorID[4]);
        }
        else if (leftJoy.x < -deadzoneController && leftJoy.y > 0.01f) 
        {//GoingLeftBack
            animatorPlayer.SetTrigger(animatorID[5]);
        }
    }

    private void Shoot(float charge, Vector2 projDirection)
    {
        float multiplicatorShoot = 1;
        
        if (charge < timeMaxCharge/3)
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("lightArrow", transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(muzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            

        }
        else if (charge < timeMaxCharge/1.5)
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("mediumArrow", transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(bigMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            
        }
        else if (charge >= timeMaxCharge - perfectShootValue && charge <= timeMaxCharge)
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("perfectTimingAmmo",
                transform.GetChild(0).position - new Vector3(-projDirection.x, -projDirection.y, 0).normalized,
                transform.GetChild(0).rotation);
            Instantiate(giantMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            Instantiate(cylindre, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            multiplicatorShoot = 1.5f;
            MMVibrationManager.Haptic(_hapticTypesForPerfectShoot, false, true, this);
        }
        else
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("heavyArrow", transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(giantMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            Instantiate(cylindre, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
        }
        
        spawnedProj.GetComponent<BulletPoolBehaviour>().force = projDirection.normalized;
        spawnedProj.GetComponent<BulletPoolBehaviour>().waitForDestruction = charge * 0.25f;
        spawnedProj.GetComponent<BulletPoolBehaviour>().damage =(int) (baseDamage + Mathf.RoundToInt(charge * 20) * multiplicatorShoot);
    }

    public void TakeDamage(int damageNumber)
    {
        currentHealth -= damageNumber;
        healthBar.value = (float)currentHealth / maxHealth;
        
        DamageFeedbacks.PlayFeedbacks();
        CameraShake.instance.StartShake(0.2f, 0.15f, 10f);
        
        if (currentHealth < 1)
        {
            animatorPlayer.SetTrigger(animatorID[7]);
        }
    }

    public void DashSpell()
    {
        dashSpellactivation -= 1;
        dashNodeList.Add(Instantiate(dashNode, transform.position, quaternion.identity));
        if (dashSpellactivation < 1)
        {
            dashSpellActive = false;
            for (int i = 0; i < dashNodeList.Count-1; i++)
            {
                GameObject laser = Instantiate(spellDashLaser, dashNodeList[i].transform.position, quaternion.identity,dashNodeList[i].transform);
                for (int j = 0; j < laser.transform.childCount - 1; j++)
                {
                    laser.transform.GetChild(j).GetComponent<LineRenderer>().SetPosition(1, dashNodeList[i+1].transform.position - laser.transform.position);
                }
                Vector3 perpendicular = Vector2.Perpendicular(dashNodeList[i + 1].transform.position - laser.transform.position).normalized; 
                laser.GetComponentInChildren<PolygonCollider2D>().SetPath(0,new List<Vector2>
                {
                    perpendicular*0.3f,
                    -perpendicular*0.3f,
                    dashNodeList[i + 1].transform.position - laser.transform.position - perpendicular*0.3f,
                    dashNodeList[i + 1].transform.position - laser.transform.position + perpendicular*0.3f
                });
                Debug.Log(dashNodeList[i].transform.position + perpendicular.normalized);
                Debug.Log(dashNodeList[i + 1].transform.position - perpendicular.normalized);

                Destroy(dashNodeList[i], 3);
            }
            Destroy(dashNodeList[dashNodeList.Count-1], 3);
        }
    }
}
