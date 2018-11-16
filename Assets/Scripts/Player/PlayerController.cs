using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStates { FLYING, DODGING, LAYING_EGG };

public class PlayerController : LivingBeing {

    public float vignetteFactor;

    [HideInInspector]
    public int dragonPopup;

    [Space]
    [Space]
    public int level;
    public PlayerStates playerState;

    [Header("Inputs")]
    public string inputShoot;
    public string inputShootAlt;
    public string inputDodge;
    public string inputDodgeAlt;
    public string inputSprint;
    public string inputSlowDown;
    public string inputLayEgg;
    public string inputPlaceAncient;

    [Header("Flying")]
    public float flySpeed;
    public float shootSpeed;
    float speed;
    internal Vector3 desiredDir = Vector3.forward;
    Vector3 lastDesiredDir = Vector3.forward;
    float hinput;
    float vinput;
    float rotationAngle;
    public float flyingRotationLerp;
    public float shootingRotationLerp;
    float rotationLerp;
    Quaternion targetRot;
    public float maxSteerRot;
    Vector3 dirDiff;
    internal bool isSprinting;
    public float sprintSpeed;
    float sprintTime;
    internal bool isSlowing; 
    public float slowSpeed;
    float slowTime;
    float timeOutOfSlow;
    public float minSlowTime;
    public float boostTimeOutOfSlow;
    public float landSpeed;

    [Header("Stamina")]
    public float maxStamina;
    float stamina;
    public float sprintCostPerSecond;
    public float sprintCooldown;
    public float slowCostPerSecond;
    public float slowCooldown;
    public float regenPerSecond;
    public float regenCooldown;
    float regenTime;

    [Header("Aiming")]
    public float cursorSpeed;
    float rhinput;
    float rvinput;
    public Vector3 originCursorPos;

    [Header("Shooting")]
    public float timeBetweenCols;
    internal bool isShooting;
    float fireTime;
    public float fireColSpeed;
    public Transform shootTarget;
    public float scrShakeTimer;
    public float scrShakeAmount;

    [Header("Dodging")]
    public float dodgeSpeed;
    public float megaDodgeSpeed;
    public float dodgeTime;
    Coroutine dodgeCor;
    bool canDodge = true;
    public float dodgeCooldown;
    
    [Header("Landing")]
    private Vector3 nestPosition;

    [System.NonSerialized]
    public bool canLand;

    [Header("Vibration")]
    public int playerIndex = 0;
    public enum FireVibrationDebug { Continous, Burst};
    public FireVibrationDebug fireVibrationDebug;
    public float leftFireContinuous;
    public float rightFireContinuous;
    public float fireBurstVibrateTime;
    public float leftFireBurst;
    public float rightFireBurst;

    [Header("References")]
    public Transform aimCursor;
    public ParticleSystem firePartSys;
    public Transform fireOrigin;
    public Rigidbody rb;
    public GameObject fireCollider;
    public Animator anim;
    public Slider staminaBar;
    public BabyDragonManager babyDragonMan;
    public EggManager eggMan;
    public GameObject egg;
    public Transform toDropegg;
    GameManager gameMan;
    public GameObject placeholderFeedback;
    Nest nestScript;

    public override void Start()
    {
        base.Start();

        stamina = maxStamina;
        sprintTime = sprintCooldown;
        slowTime = slowCooldown;
        gameMan = GameManager.Instance;
    }

    private void FixedUpdate()
    {
        //Storing the joystick inputs
        hinput = Input.GetAxis("Horizontal");
        vinput = Input.GetAxis("Vertical");

        switch (playerState)
        {
            case PlayerStates.FLYING:
                //Going forward at all times

                if (timeOutOfSlow > 0 || isSprinting && !isSlowing)
                    speed = sprintSpeed;
                else if (isSlowing && !isSprinting)
                    speed = slowSpeed;
                else if (isShooting)
                    speed = shootSpeed;
                else
                    speed = flySpeed;

                rb.velocity = transform.forward * speed;

                //Don't change the desired direction if there is no input
                if (hinput != 0 || vinput != 0)
                {
                    //Direction based on input
                    desiredDir = new Vector3(hinput, 0f, vinput);
                    //Store the desired direction for next frame
                    lastDesiredDir = desiredDir;
                }

                //If the player's forward isn't aligned with the desired direction
                if (transform.forward != desiredDir)
                {
                    //Lerp between the current rotation and the desired direction's look rotation
                    targetRot = Quaternion.LookRotation(desiredDir);
                    rotationAngle = Vector3.SignedAngle(transform.forward, desiredDir, transform.up);
                    targetRot = Quaternion.Euler(targetRot.eulerAngles.x, targetRot.eulerAngles.y, -rotationAngle * maxSteerRot / 180);
                    rotationLerp = isShooting ? shootingRotationLerp : flyingRotationLerp;
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerp);
                }
                break;

            case PlayerStates.DODGING:
                break;
            
            case PlayerStates.LAYING_EGG:
                // Decrease speed and stop 
                /*  if(rb.velocity != new Vector3 (0,0,0))
                  {
                      rb.velocity = Vector3.Lerp(rb.velocity,new Vector3(0,0,0),Time.deltaTime*landSpeed);
                  }*/
                rb.velocity = new Vector3(0f, 0f, 0f);
                if (nestPosition != null)
                {
                    transform.position = Vector3.Lerp(transform.position, nestPosition, Time.deltaTime * landSpeed);
                }

                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        RegenStamina();

        switch (playerState)
        {
            case PlayerStates.FLYING:
                //Aim();
                Shoot();
                LayEgg();
                Sprint();
                SlowDown();
                Dodge();
                break;

            case PlayerStates.DODGING:
                //Aim();
                break;
            
            case PlayerStates.LAYING_EGG:
                break;
        }

        timeOutOfSlow -= Time.deltaTime;

        UpdateStaminaUI();
    }

    private void OnTriggerEnter(Collider colNest)
    {
        if (colNest.gameObject.tag == "Nest")
        {
            nestScript = colNest.gameObject.GetComponent<Nest>();
            if (nestScript.content == null)
            {
                nestScript.active = true;
                nestPosition = colNest.transform.position;
                canLand = true;
            }
        }
    }

    private void OnTriggerExit(Collider colNest)
    {
        if (colNest.gameObject.tag == "Nest")
        {
            nestScript = colNest.gameObject.GetComponent<Nest>();
            nestScript.active = false;
            canLand = false;
        }
    }

    //Actions

    void Aim()
    {
        rhinput = Input.GetAxis("Horizontal_R");
        rvinput = Input.GetAxis("Vertical_R");

        if (rhinput != 0 || rvinput != 0)
        {
            //Direction based on input
            //aimCursor.Translate(new Vector3(hinput, 0f, vinput) * cursorSpeed);
            aimCursor.localPosition = originCursorPos + new Vector3(rhinput, aimCursor.localPosition.y, rvinput) * cursorSpeed; //range
        }
    }

    void Shoot()
    {
        //Begin shooting
        if (Input.GetButtonDown(inputShoot))
        {
            firePartSys.Play();
            isShooting = true;
            fireTime = 0;

            foreach(BabyDragonBehaviour babyDragon in babyDragonMan.babyDragons)
            {
                babyDragon.Shoot(shootTarget, rb);
            }

            switch (fireVibrationDebug)
            {
                case FireVibrationDebug.Continous:
                    gameMan.vibrationMan.StartVibrating(playerIndex, leftFireContinuous, rightFireContinuous);
                    break;
                case FireVibrationDebug.Burst:
                    gameMan.vibrationMan.VibrateFor(fireBurstVibrateTime, playerIndex, leftFireBurst, rightFireBurst);
                    break;
            }

        }

        //End shooting
        if (Input.GetButtonUp(inputShoot))
        {
            StopShooting();
        }

        //While the player is shooting
        if (isShooting)
        {
            if (fireTime >= timeBetweenCols)
            {
                GameObject fireCol = Instantiate(fireCollider, fireOrigin.position, Quaternion.identity);
                fireCol.GetComponent<Rigidbody>().velocity = rb.velocity + (shootTarget.position - fireOrigin.position).normalized * fireColSpeed;
                fireTime = 0;
            }
            else
                fireTime += Time.deltaTime;
        }

    }

    void Dodge()
    {
        if ((Input.GetButtonDown(inputDodge) || Input.GetAxis(inputDodgeAlt) > .1f) && canDodge)
            dodgeCor = StartCoroutine(IDodge());
    }

    void SlowDown()
    {
        if (Input.GetButtonDown(inputSlowDown))
            timeOutOfSlow = 0;

        if (Input.GetButton(inputSlowDown) && stamina > 0 && slowTime >= slowCooldown)
        {
            isSlowing = true;

            if (!isSprinting)
                UseStamina(slowCostPerSecond);
        }

        if (isSlowing && (Input.GetButtonUp(inputSlowDown) || stamina == 0))
        {
            isSlowing = false;
            if (timeOutOfSlow < -minSlowTime)
                timeOutOfSlow = boostTimeOutOfSlow;
            if (stamina == 0)
                slowTime = 0;
        }
    }

    void Sprint()
    {
        if (Input.GetButton(inputSprint) && stamina > 0 && sprintTime >= sprintCooldown)
        {
            isSprinting = true;
            if (!isSlowing)
                UseStamina(sprintCostPerSecond);
        }
        else if (isSprinting)
        {
            isSprinting = false;
            if (stamina == 0)
                sprintTime = 0;
        }
    }

    void StopShooting()
    {
        firePartSys.Stop();
        isShooting = false;
        foreach (BabyDragonBehaviour babyDragon in babyDragonMan.babyDragons)
        {
            babyDragon.StopShooting();
        }

        if (fireVibrationDebug == FireVibrationDebug.Continous)
            gameMan.vibrationMan.StopVibrating(playerIndex);
    }

    private void LayEgg()
    {
        if (Input.GetButtonDown(inputLayEgg) && canLand && eggMan.eggSlider.value >= 1)
        {
            eggMan.eggSlider.value = 0;
            StartCoroutine(ILayEgg());
        }
    }

    //Stamina

    public void UpdateStaminaUI()
    {
        staminaBar.value = stamina / maxStamina;
    }

    void UseStamina(float _cost)
    {
        stamina -= _cost * Time.deltaTime;
        if (stamina < 0)
        {
            stamina = 0;
        }
        regenTime = 0;
    }

    void RegenStamina()
    {
        if (stamina < maxStamina && regenTime >= regenCooldown)
        {
            stamina += regenPerSecond * Time.deltaTime;
            if (stamina > maxStamina)
                stamina = maxStamina;
        }

        if (regenTime < regenCooldown)
            regenTime += Time.deltaTime;

        if (sprintTime < sprintCooldown)
            sprintTime += Time.deltaTime;

        if (slowTime < slowCooldown)
            slowTime += Time.deltaTime;
    }

    //Overrides

    public override void Die()
    {
        base.Die();

        if (babyDragonMan.babyDragons.Count > 0)
        {
            Instantiate(placeholderFeedback, babyDragonMan.babyDragons[0].transform.position, Quaternion.identity);
            Instantiate(placeholderFeedback, transform.position, Quaternion.identity);
            StartCoroutine(IPlaceholderNewMother());

            babyDragonMan.RemoveBabyDragon();

            ResetLife(2f);
            MakeInvincible(2f);
        }
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public override void UpdateHealthUI(int _damage)
    {
        base.UpdateHealthUI(_damage);

        if (gameMan.vignetteMan.CurrentPreset != gameMan.vignetteMan.hurtVignette)
            gameMan.vignetteMan.ChangeVignette(gameMan.vignetteMan.hurtVignette);
        else
        {
            VignettePreset toSwitchAfterDecay = life / maxLife < .25 ? gameMan.vignetteMan.hurtVignette : gameMan.vignetteMan.basicVignette;
            //print(toSwitchAfterDecay.name);
            gameMan.vignetteMan.IncrementSmoothness(gameMan.vignetteMan.hurtVignette, _damage * vignetteFactor, toSwitchAfterDecay);
        }
    }

    public override void ResetHealthUI(float _timeToRegen)
    {
        base.ResetHealthUI(_timeToRegen);

        gameMan.vignetteMan.ChangeVignette(gameMan.vignetteMan.basicVignette);
    }
   
    //Coroutines

    IEnumerator IDodge()
    {
        canDodge = false;
        StopShooting();
        playerState = PlayerStates.DODGING;

        desiredDir = new Vector3(hinput, 0f, vinput);
        if (desiredDir == Vector3.zero)
            desiredDir = transform.right;

        anim.SetTrigger("dodges");

        float time = 0;
        while (time < dodgeTime)
        {
            time += Time.deltaTime;
            rb.velocity = desiredDir * (isSprinting ? megaDodgeSpeed : dodgeSpeed);

            targetRot = Quaternion.LookRotation(desiredDir) * Quaternion.Euler(0f, 0f, 90f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerp);
            yield return null;
        }

        playerState = PlayerStates.FLYING;

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }
    
    IEnumerator ILayEgg()
    {
        StopShooting();
        //anim.SetTrigger("land");

        yield return new WaitForSeconds(0.5f);
        playerState = PlayerStates.LAYING_EGG;

        yield return new WaitForSeconds(1.0f);
        GameObject instanceEgg;
        instanceEgg = Instantiate(egg, new Vector3(nestPosition.x, nestPosition.y + 0.5f, nestPosition.z), Quaternion.identity) as GameObject;
        nestScript.content = instanceEgg;
        gameMan.spawnMan.eggs.Add(instanceEgg.transform);

        yield return new WaitForSeconds(0.5f);
        playerState = PlayerStates.FLYING;

        yield break;
    }

    IEnumerator IPlaceholderNewMother()
    {
        float time = 0f;
        float growTime = 2f;
        while (time < growTime)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(new Vector3(.3f, .3f, .3f), Vector3.one, time / growTime);
            yield return null;
        }
    }
}