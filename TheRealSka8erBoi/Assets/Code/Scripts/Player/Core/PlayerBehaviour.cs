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
    [SerializeField] Vector2 leftJoy;
    [SerializeField] int playerSpeed;
    [SerializeField] private float deadzoneController = 0.3f;
    public Vector2 latestDirection;

    //Declaration Dash
    public float dashCd;
    public float dashSpeed;
    public float dashGoingFor;
    public float dashDuration;
    private bool dash = true;
    
    //Declaration Animation
    private Animator animatorPlayer;
    private int playerDirec = 0; 
    
    //Declaration Shoot
    [SerializeField] bool isAiming;
    [SerializeField] private float charge;
    [SerializeField] private float resetShoot;
    [SerializeField] private float timeMaxCharge = 0.9f;
    private GameObject spawnedProj;

    //Declaration VFXShoot
    public GameObject cylindre;
    public GameObject bigMuzzle;
    public GameObject muzzle;
    
    //Declaration UI
    public int maxHealth;
    public int currentHealth;
    public Slider healthBar;
    
    

    private void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerRender = GetComponent<SpriteRenderer>();
        animatorPlayer = GetComponent<Animator>();
        dashGoingFor = dashDuration;
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
            charge = 0;
        }
        if (Input.GetButtonUp("BowShot"))
        {
            
            Shoot(charge, GetComponentInChildren<CursorBehaviour>().AimDirection().Item2);
            transform.GetChild(0).gameObject.SetActive(false);
        }

        dashCd -= Time.deltaTime;
        if (Input.GetAxisRaw("Dash") > 0)
        {
            if (dashCd <= 0)
            {
                dashCd = 2;
                dashGoingFor = 0;
                dash = true;
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
         TakeDamage(5);
        }
    }

    private void Ongoing()
    {
        playerRigid.velocity = Vector2.zero;
        if (!isAiming)
        {
            
            animatorPlayer.SetBool("IsAiming",false);
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
            animatorPlayer.SetBool("IsAiming",true);
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
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else
            {
                dashGoingFor += Time.fixedDeltaTime;
                playerRigid.velocity = new Vector2(leftJoy.x,leftJoy.y) * dashSpeed;
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
        isAiming = false;
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
            Instantiate(bigMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            Instantiate(cylindre, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
        }
        
        spawnedProj.GetComponent<BulletPoolBehaviour>().force = projDirection.normalized;
        spawnedProj.GetComponent<BulletPoolBehaviour>().waitForDestruction = charge * 0.2f;
        spawnedProj.GetComponent<BulletPoolBehaviour>().damage = 7 + Mathf.RoundToInt(charge * 20);
        //Invoke(("ResetShoot"), resetShoot);
    }

    public void TakeDamage(int damageNumber)
    {
        currentHealth -= damageNumber;
        healthBar.value = (float)currentHealth / maxHealth;
    }
}
