using System;
using System.Collections;
using MoreMountains.NiceVibrations;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;

public class PlayerBehaviour : MonoBehaviour
{
    private GameObject chargeProjectile;
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
    public bool over9000Power;
    public bool canTakeDamage;
    [Space]
    
    //Declaration Dash
    [Header("Dash Tweaking")]
    [SerializeField] private float _dashCd;

    public float dashCd => _dashCd - (BonusManager.instance.greenStat * GreenStatModifier);
    public float dashSpeed;
    public float dashDuration;
    public Image dashUI;
    private float dashOngoingCd;
    private float dashGoingFor;
    public bool dash = true;
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
    public float shootingCooldown;
    [SerializeField] private float _shootingCd;
    public float shootingCd => _shootingCd - (BonusManager.instance.greenStat / 7);
    [SerializeField] private MMFeedbacks shootingCDFB;
    public float baseDamage => _baseDamage + (BonusManager.instance.blueStat * BlueStatModifier);
    public bool shurikenActive;
    [Space]
    private bool isAiming;

    private bool isDead;
    private float charge;

    private GameObject spawnedProj;
    private GameObject spawnedShuriken;

    public bool perfectTiming=false;
    //Declaration VFXShoot
    [Header("FX Declaration")]
    public GameObject cylindre;
    public GameObject bigMuzzle;
    public GameObject muzzle;
    public GameObject giantMuzzle;
    public GameObject chargeProjo;
    

    [Space] [Header("Feedback Declaration")]
    public MMFeedbacks DamageFeedbacks;

    public GameObject feedbackKunai;
    private GameObject currentFBShuriken;

    [Space]

    //Declaration UI
    //public TextMeshProUGUI lifeText;
    [SerializeField] private int _maxHealth = 5;
    public PauseMenu pause;
    

    public int maxHealth => (int)(_maxHealth + BonusManager.instance.redStat * RedStatModifier);
    [HideInInspector] public int currentHealth;
    public Image healthBar;
    
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

    private bool canActivateStepSound;

    public MMFeedbacks perfection;
    public GameObject DeathCanvasGroup;
    public GameObject WinningCanvas;
    public GameObject ChienCanvas;
    public bool isDogActive;
    public Animator refusMortAnimator;
    

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
        //lifeText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        canActivateStepSound = true;
        over9000Power = false;
        canTakeDamage = true;
        shurikenActive = false;
        isDogActive = false;
        
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
        if (!isDead)
        {
            MyInput();
        }
        
        switch (isDogActive)
        {
            case true:
                ChienCanvas.SetActive(true);
                break;
            case false:
                ChienCanvas.SetActive(false);
                break;
        }

        if (shurikenActive) feedbackKunai.GetComponent<ParticleSystem>().Play();
        else feedbackKunai.GetComponent<ParticleSystem>().Stop();
        
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
            if (shootingCooldown - 0.05f >= 0 ) return;
            
            transform.GetChild(0).gameObject.SetActive(true);
            isAiming = true;
            animatorPlayer.SetBool(animatorID[9], isAiming);
            charge = 0;
            
            chargeProjectile = Instantiate(chargeProjo, transform.position, Quaternion.identity);
        }
        if (Input.GetButtonUp("BowShot") && isAiming)
        {
            Shoot(charge, GetComponentInChildren<CursorBehaviour>().AimDirection().Item2);
            transform.GetChild(0).gameObject.SetActive(false);
            isAiming = false;
            SoundCaller.instance.PLayerTirSound();
            animatorPlayer.SetTrigger(animatorID[6]);//Release
            animatorPlayer.SetBool(animatorID[9], isAiming);
       
            Destroy(chargeProjectile);
            
        }

        
        dashOngoingCd -= Time.deltaTime;
        shootingCooldown -= Time.deltaTime;
        dashUI.fillAmount = 1 - shootingCooldown / shootingCd;

        if (Math.Abs(dashUI.fillAmount - 0.98f) < 0.01f) shootingCDFB.PlayFeedbacks();

        if (Input.GetAxisRaw("Dash") > 0 && !isAiming)
        {
            if (dashOngoingCd <= 0)
            {
                dashGoingFor = 0;
                dash = true;
                gameObject.tag = "PlayerDashing";
                Physics2D.IgnoreLayerCollision(6, 10, true);
                Physics2D.IgnoreLayerCollision(6, 11, true);
                if (dashSpellActive) dashOngoingCd = dashCd / 2 + dashDuration;
                else dashOngoingCd = dashCd + dashDuration ;
                if (dashSpellActive && dashSpellactivation == 3)
                {
                    dashNodeList.Add(Instantiate(dashNode, transform.position, quaternion.identity));
                }
                SoundCaller.instance.DashSound();
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
            if (charge < timeMaxCharge + perfectShootValue)
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
                gameObject.tag = "Player";
                Physics2D.IgnoreLayerCollision(6, 10, false);
                Physics2D.IgnoreLayerCollision(6, 11, false);
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

        if (playerRigid.velocity != Vector2.zero && canActivateStepSound)
        {
            canActivateStepSound = false;
            StartCoroutine(CallStepSound()); 
        }
    }

    IEnumerator CallStepSound()
    {
        SoundCaller.instance.StepSound();
        yield return new WaitForSeconds(SoundCaller.instance.timeBetweenSoundsStep);
        canActivateStepSound = true;
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
            shootingCooldown = shootingCd * 0.4f;
            multiplicatorShoot = 0.5f;
            if (shurikenActive)
            {
                spawnedShuriken =
                    PoolObjectManager.Instance.GetBullet("shurikenFaucheuse", transform.position, Quaternion.Euler(Mathf.Atan2(GetComponent<PlayerBehaviour>().latestDirection.y, -GetComponent<PlayerBehaviour>().latestDirection.x) * Mathf.Rad2Deg,90,0));
            }
        }
        else if (charge < timeMaxCharge/1.5)
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("mediumArrow", transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(bigMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            shootingCooldown = shootingCd * 0.7f;
            multiplicatorShoot = 0.8f;
            if (shurikenActive)
            {
                spawnedShuriken =
                    PoolObjectManager.Instance.GetBullet("shurikenFaucheuse", transform.position, Quaternion.Euler(Mathf.Atan2(GetComponent<PlayerBehaviour>().latestDirection.y, -GetComponent<PlayerBehaviour>().latestDirection.x) * Mathf.Rad2Deg,90,0));
            }
        }
        else if (charge >= timeMaxCharge - perfectShootValue && charge < timeMaxCharge + perfectShootValue)
        {
            perfectTiming = true;
            spawnedProj = PoolObjectManager.Instance.GetBullet("perfectTimingAmmo",
                transform.GetChild(0).position - new Vector3(-projDirection.x, -projDirection.y, 0).normalized,
                transform.GetChild(0).rotation);
            Instantiate(giantMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            Instantiate(cylindre, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            multiplicatorShoot = 1.25f;
            shootingCooldown = shootingCd * 0.7f;
            MMVibrationManager.Haptic(_hapticTypesForPerfectShoot, false, true, this);
            if (shurikenActive)
            {
                spawnedShuriken =
                    PoolObjectManager.Instance.GetBullet("shurikenFaucheuse", transform.position, Quaternion.Euler(Mathf.Atan2(GetComponent<PlayerBehaviour>().latestDirection.y, -GetComponent<PlayerBehaviour>().latestDirection.x) * Mathf.Rad2Deg,90,0));
            }
        }
        else if (charge >= timeMaxCharge + perfectShootValue)
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("heavyArrow", transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(giantMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            Instantiate(cylindre, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            shootingCooldown = shootingCd;
            multiplicatorShoot = 1;
            if (shurikenActive)
            {
                spawnedShuriken =
                    PoolObjectManager.Instance.GetBullet("shurikenFaucheuse", transform.position, Quaternion.Euler(Mathf.Atan2(GetComponent<PlayerBehaviour>().latestDirection.y, -GetComponent<PlayerBehaviour>().latestDirection.x) * Mathf.Rad2Deg,90,0));
            }
        }
        else
        {
            spawnedProj = PoolObjectManager.Instance.GetBullet("heavyArrow", transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized,transform.GetChild(0).rotation);
            Instantiate(giantMuzzle, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            Instantiate(cylindre, transform.GetChild(0).position - new Vector3(-projDirection.x,-projDirection.y,0).normalized, transform.GetChild(0).rotation);
            shootingCooldown = shootingCd;
            multiplicatorShoot = 1;
            if (shurikenActive)
            {
                spawnedShuriken =
                    PoolObjectManager.Instance.GetBullet("shurikenFaucheuse", transform.position, Quaternion.Euler(Mathf.Atan2(GetComponent<PlayerBehaviour>().latestDirection.y, -GetComponent<PlayerBehaviour>().latestDirection.x) * Mathf.Rad2Deg,90,0));
            }
        }
        
        spawnedProj.GetComponent<BulletPoolBehaviour>().force = projDirection.normalized;
        spawnedProj.GetComponent<BulletPoolBehaviour>().waitForDestruction = charge * 0.4f;
        if(over9000Power) spawnedProj.GetComponent<BulletPoolBehaviour>().damage =(int) ((baseDamage + baseDamage * charge * 1000) * multiplicatorShoot);
        else spawnedProj.GetComponent<BulletPoolBehaviour>().damage =(int) ((baseDamage + baseDamage * charge * 1.15f) * multiplicatorShoot);
        

        if (shurikenActive)
        {
            spawnedShuriken.GetComponent<BulletPoolBehaviour>().force = projDirection.normalized;
            spawnedShuriken.GetComponent<BulletPoolBehaviour>().waitForDestruction = charge * 0.4f;
            shurikenActive = false;
        }
    }

    public void TakeDamage(int damageNumber)
    {
        if(!canTakeDamage) return;
        if(isDead) return;
        currentHealth -= damageNumber;
        float division = (float)currentHealth / maxHealth;
        float value = Mathf.Clamp(1 - division, 0, 1);
        healthBar.fillAmount = value;
        //lifeText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        StartCoroutine(Invincibility());
        DamageFeedbacks.PlayFeedbacks();
        CameraShake.instance.StartShake(0.2f, 0.15f, 10f);

        if (currentHealth < 1)
        {
            if (gameObject.GetComponent<Inventory>().deathDefiance1)
            {
                RestoreLife(1);
                refusMortAnimator.SetTrigger("Niv1Down");
                return;
            }
            else
            {
                if (gameObject.GetComponent<Inventory>().deathDefiance2)
                {
                    RestoreLife(2);
                    refusMortAnimator.SetTrigger("Niv2Down");
                    return;
                }
                else
                {
                    StartCoroutine(DyingCharacter());
                }
            }

            isDead = true;
            animatorPlayer.SetBool(animatorID[7], true);
            StartCoroutine(DyingCharacter());
        }
    }

    public void GetHealth(int healthNumber)
    {
        currentHealth += healthNumber;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float division = (float)currentHealth / maxHealth;
        float value = Mathf.Clamp(1 - division, 0, 1);
        healthBar.fillAmount = value;
        //lifeText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
       
    }

    IEnumerator Invincibility()
    {
        if (!canTakeDamage) yield break;

        canTakeDamage = false;
        yield return new WaitForSeconds(0.8f);
        canTakeDamage = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            perfection.PlayFeedbacks();
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

    void RestoreLife(int number)
    {
        if (number == 1) gameObject.GetComponent<Inventory>().deathDefiance1 = false;
        if (number == 2) gameObject.GetComponent<Inventory>().deathDefiance2 = false;
        
        GetHealth(maxHealth / 4);
    }

    IEnumerator DyingCharacter()
    {
        yield return new WaitForSeconds(3);
        SoundCaller.instance.PlayerDeath();
        DeathCanvasGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Score : " + BonusManager.instance.finalScore;
        DeathCanvasGroup.gameObject.SetActive(true);
        pause.GameOverPause();
    }

    public void WinningCharacter()
    {
        WinningCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Score : " + BonusManager.instance.finalScore;
        WinningCanvas.gameObject.SetActive(true);
        pause.GameOverPause();
    }
}
