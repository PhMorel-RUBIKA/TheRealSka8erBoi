using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBehaviour : MonoBehaviour
{
public string bulletLightName;
    
    //Declaration Movement
    private Rigidbody2D playerRigid;
    private SpriteRenderer playerRender;
    private Vector2 leftJoy;
    private float floatManipulation;
    [HideInInspector]public Vector2 latestDirection;
    
    [Header("Player Control")]
    [SerializeField] int playerSpeed;
    [SerializeField] private float deadzoneController = 0.3f;
    [Space]
    
    //Declaration Dash
    [Header("Dash Tweaking")]
    public float dashCd;
    public float dashSpeed;
    public float dashDuration;
    [Space]
    private float dashOngoingCd;
    private float dashGoingFor;
    private bool dash = true;
    
    //Declaration Animation
    private Animator animatorPlayer;
    private int playerDirec = 0; 
    
    //Declaration Shoot
    [Header("Shoot Tweaking")]
    [SerializeField] private float timeMaxCharge = 0.9f;
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

    [Space]

    //Declaration UI
    [HideInInspector] public int maxHealth = 100;
    [HideInInspector] public int currentHealth;
    public Slider healthBar;

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
            animatorPlayer.SetBool("IsAiming",true);
            charge = 0;
        }
        if (Input.GetButtonUp("BowShot") && isAiming)
        {
            Shoot(charge, GetComponentInChildren<CursorBehaviour>().AimDirection().Item2);
            transform.GetChild(0).gameObject.SetActive(false);
            isAiming = false;
            animatorPlayer.SetBool("IsAiming",false);
        }

        dashOngoingCd -= Time.deltaTime;
        if (Input.GetAxisRaw("Dash") > 0 && !isAiming)
        {
            if (dashOngoingCd <= 0)
            {
                dashOngoingCd = dashCd ;
                dashGoingFor = 0;
                dash = true;
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
                animatorPlayer.SetBool("IsRunning",true); 
                playerRigid.velocity = leftJoy * playerSpeed;
            }
            else
            {
                animatorPlayer.SetBool("IsRunning",false);
            }
        }
        else
        {
            animatorPlayer.SetBool("IsRunning", false);
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
        if (leftJoy.x < -deadzoneController)
        {
            playerRender.flipX = true;
        }
        else if (leftJoy.x > deadzoneController)
        {
            playerRender.flipX = false;
        }
        
        if (leftJoy.y < -Mathf.Abs(leftJoy.x))
        {
            if (playerDirec != 0)
            {
                   animatorPlayer.SetTrigger("GoingDown");
                   playerDirec = 0;
            }
        }
        else if (Mathf.Abs(leftJoy.x) > Mathf.Abs(leftJoy.y) && leftJoy.y > 0.3f)
        {
                if (playerDirec != 1)
                {
                    animatorPlayer.SetTrigger("GoingBackSide");
                    playerDirec = 1;
                } 
        }
        else if (Mathf.Abs(leftJoy.x) > Mathf.Abs(leftJoy.y) && leftJoy.y < 0.3f) 
        {
                if (playerDirec != 2)
                {
                    animatorPlayer.SetTrigger("GoingDownSide");
                    playerDirec = 2;
                } 
        }
        else if (leftJoy.y > Mathf.Abs(leftJoy.x)) 
        {
                if (playerDirec != 3)
                {
                    animatorPlayer.SetTrigger("GoingUp");
                    playerDirec = 3;
                } 
        }
    }

    private void Shoot(float charge, Vector2 projDirection)
    {
        
        if (charge < timeMaxCharge/3)
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet(bulletLightName, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(muzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            

        }
        else if (charge < timeMaxCharge/1.5)
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("mediumArrow", transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(bigMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            
        }
        else
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("heavyArrow", transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(giantMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            Instantiate(cylindre, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
        }
        
        spawnedProj.GetComponent<BulletPoolBehaviour>().force = projDirection.normalized;
        spawnedProj.GetComponent<BulletPoolBehaviour>().waitForDestruction = charge * 0.25f;
        spawnedProj.GetComponent<BulletPoolBehaviour>().damage = 7 + Mathf.RoundToInt(charge * 20);
    }

    public void TakeDamage(int damageNumber)
    {
        currentHealth -= damageNumber;
        healthBar.value = (float)currentHealth / maxHealth;
    }
}
