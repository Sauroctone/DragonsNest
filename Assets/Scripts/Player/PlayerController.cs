using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public enum PlayerStates { FLYING, DODGING, LAYING_EGG, SELF_DESTROYING, TURNING_AROUND };

public class PlayerController : LivingBeing {

    private float lifesShader;
    private float lifeFeedBackAmount;
    public float vignetteFactor;

    [HideInInspector]
    public int dragonPopup;

    [Space]
    [Space]
    public int level;
    public PlayerStates playerState;

    [Header("Inputs")]
    public string inputShoot;
    public string inputSprint;
    public string inputSprintAlt;
    public string inputSlowDown;
    public string inputSlowDownAlt;
    public string inputInteract;
    public string inputPlaceAncient;

    [Header("Flying")]
    public float flySpeed;
    public float shootSpeed;
    float speed;
    internal Vector3 desiredDir = Vector3.forward;
    float hinput;
    float vinput;
    float rotationAngle;
    public float flyingRotationLerp;
    public float shootingRotationLerp;
    float rotationLerp;
    Quaternion targetRot;
    Quaternion rollRot;
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
    float flapTime;
    float timeToFlap;
    public float minTimeToFlap;
    public float maxTimeToFlap;

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

    [Header("Looking")]
    public float lookRange;
    float rhinput;
    float rvinput;
    float lastRHInput;
    float lastRVInput;

    [Header("Shooting")]
    public float timeBetweenCols;
    internal bool isShooting;
    float fireTime;
    float prevSprintAxis;
    float prevSlowDownAxis;
    public float fireColSpeed;
    public Transform shootTarget;
    public float scrShakeTimer;
    public float scrShakeAmount;

    //[Header("Dodging")]
    //public float dodgeSpeed;
    //public float megaDodgeSpeed;
    //public float dodgeTime;
    //Coroutine dodgeCor;
    //bool canDodge = true;
    //public float dodgeCooldown;
    
    [Header("Landing")]
    public  Vector3 nestPosition;

    [Header("Self destruct")]
    public float timeToSelfDestruct;
    float selfDestructTime;
    public AnimationCurve selfDestructLeftVibCurve;
    public AnimationCurve selfDestructRightVibCurve;
    public float selfDestructLaunchVibTime;
    public float selfDestructLaunchVibInt;
    public float selfDestructFreeze;
    public float timeToPlunge;

    [System.NonSerialized]
    public bool canLand;

    [Header("Turn Around")]
    public float turnAroundLockTimeFactor;
    Coroutine turnAroundCor;

    [Header("Vibration")]
    public int playerIndex = 0;
    public float fireBurstVibrateTime;
    public float leftFireBurst;
    public float rightFireBurst;

    [Header("References")]
    public Transform visuals;
    public Transform aimCursor;
    public ParticleSystem firePartSys;
    public Transform fireOrigin;
    public Rigidbody rb;
    public GameObject fireCollider;
    public Animator anim;
    // public Slider staminaBar;
    public BabyDragonManager babyDragonMan;
    public EggManager eggMan;
    // public GameObject egg;
    // public Transform toDropegg;
    GameManager gameMan;
    // public GameObject placeholderFeedback;
    public ParticleSystem smokeScreen;
    public Nest nestScript;
    public GameObject ancientPrefab;
    public GameObject aimProjector;
    public MeshRenderer LifeQuad;
    public MeshRenderer StamiQuad;
    public Image selfDestructInputSlider;
    public GameObject selfDestructFeedback;

    [Header("SFXPlayer")]
    AudioSource[] AudioSources;
    AudioSource WindflowSoundSource;
    AudioSource AttackSoundSource;
    AudioSource SFXSource;
    public AudioClip DodgeClip;
    public AudioClip SlowdownClip;
    public AudioClip WindflowClip;
    public AudioClip DragonHitClip;
    public AudioClip DragonDeathClip;
    public AudioClip EggDropping;

    public void Awake()
    {
        InstantiateRefs();
    }

    public override void Start()
    {
        base.Start();
        AudioSources = GetComponents<AudioSource>();
        WindflowSoundSource = AudioSources[0];
        AttackSoundSource = AudioSources[1];
        SFXSource = AudioSources[2];

        //LifeQuad.material.shader = Shader.Find ("LifeEnergyShader");
        lifeFeedBackAmount = lifesShader = 1;
        stamina = maxStamina;
        sprintTime = sprintCooldown;
        slowTime = slowCooldown;
        gameMan = GameManager.Instance;

        timeToFlap = Random.Range(minTimeToFlap, maxTimeToFlap);
    }

    private void FixedUpdate()
    {
        //Storing the joystick inputs
        hinput = Input.GetAxis("Horizontal");
        vinput = Input.GetAxis("Vertical");
        WindflowSoundSource.volume = speed/sprintSpeed;
        switch (playerState)
        {
            case PlayerStates.FLYING:
                GetDirectionAndSpeed();
                Move();
                break;
            
            case PlayerStates.LAYING_EGG:
                rb.velocity = new Vector3(0f, 0f, 0f);
                if (nestPosition != null)
                    transform.position = Vector3.Lerp(transform.position, nestPosition, Time.deltaTime * landSpeed);

                break;

            case PlayerStates.TURNING_AROUND:
                Move();
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameMan.vibrationMan.StopVibrating(playerIndex);
            Application.Quit();
        }

        RegenStamina();

        switch (playerState)
        {
            case PlayerStates.FLYING:
                Shoot();
                Sprint();
                SlowDown();
                LayEgg();
                ChargeSelfDestruct();
                //Dodge();
                break;

            case PlayerStates.TURNING_AROUND:
                Shoot();
                LayEgg();
                break;
        }

        timeOutOfSlow -= Time.deltaTime;

        UpdateStaminaUI();
    }

    //Actions

    private void InstantiateRefs()
    {
       babyDragonMan.target = this.transform;
       babyDragonMan = Instantiate(babyDragonMan.gameObject, Vector3.zero, Quaternion.identity).GetComponent<BabyDragonManager>();
    }

    void GetDirectionAndSpeed()
    {
        if (timeOutOfSlow > 0 || isSprinting && !isSlowing)
            speed = sprintSpeed;
        else if (isSlowing && !isSprinting)
            speed = slowSpeed;
        else if (isShooting)
            speed = shootSpeed;
        else
            speed = flySpeed;

        rotationLerp = isShooting ? shootingRotationLerp : flyingRotationLerp;

        //Don't change the desired direction if there is no input
        if (hinput != 0 || vinput != 0)
        {
            //Direction based on input
            desiredDir = new Vector3(hinput, 0f, vinput);
        }
    }

    void Move()
    {
        //Going forward at all times
        rb.velocity = transform.forward * speed;

        //If the player's forward isn't aligned with the desired direction
        if (transform.forward != desiredDir)
        {
            //Lerp between the current rotation and the desired direction's look rotation
            targetRot = Quaternion.LookRotation(desiredDir);
            rotationAngle = Vector3.SignedAngle(transform.forward, desiredDir, transform.up);
            //targetRot = Quaternion.Euler(targetRot.eulerAngles.x, targetRot.eulerAngles.y, -rotationAngle * maxSteerRot / 180);
            rollRot = Quaternion.Euler(0, 0, -rotationAngle * maxSteerRot / 180);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerp);
            visuals.localRotation = Quaternion.Slerp(visuals.localRotation, rollRot, rotationLerp);
        }

        flapTime += Time.deltaTime;
        if (flapTime >= timeToFlap)
        {
            flapTime = 0;
            if(!isSprinting && !isSlowing)
                anim.SetTrigger("flaps");
            timeToFlap = Random.Range(minTimeToFlap, maxTimeToFlap);
        }
    }

    void Shoot()
    {
        //Begin shooting
        if (Input.GetButtonDown(inputShoot))
        {
            AttackSoundSource.Play();
            firePartSys.Play();
            isShooting = true;
            fireTime = 0;

            foreach(BabyDragonBehaviour babyDragon in babyDragonMan.babyDragons)
            {
                babyDragon.Shoot(shootTarget, rb);
            }

            gameMan.vibrationMan.VibrateFor(fireBurstVibrateTime, playerIndex, leftFireBurst, rightFireBurst);
            anim.SetBool("isShooting", true);

        }

        //End shooting
        if (Input.GetButtonUp(inputShoot))
        {
            AttackSoundSource.Stop();
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

    //void Dodge()
    //{
    //    if ((Input.GetButtonDown(inputDodge) || Input.GetAxis(inputDodgeAlt) > .1f) && canDodge) { 
    //    SFXSource.PlayOneShot(DodgeClip, 0.5f);
    //    dodgeCor = StartCoroutine(IDodge());
    //    }
    //}

    void SlowDown()
    {
        if (Input.GetButtonDown(inputSlowDown) || (prevSlowDownAxis < .1f && Input.GetAxis(inputSlowDownAlt) >= .1f))
        {
            timeOutOfSlow = 0;
            anim.SetBool("isSprinting", false);
        }

        if ((Input.GetButton(inputSlowDown) || Input.GetAxis(inputSlowDownAlt) >= .1f) && stamina > 0 && slowTime >= slowCooldown)
        {
            isSlowing = true;

            if (!isSprinting)
                UseStamina(slowCostPerSecond);
        }

        if (isSlowing && (Input.GetButtonUp(inputSlowDown) || Input.GetAxis(inputSlowDownAlt) < .1f || stamina == 0))
        {
            isSlowing = false;
            //SFXSource.PlayOneShot(DodgeClip);
            if (timeOutOfSlow < -minSlowTime)
                timeOutOfSlow = boostTimeOutOfSlow;
            if (stamina == 0)
                slowTime = 0;

            if (isSprinting)
                anim.SetBool("isSprinting", true);
        }

        prevSlowDownAxis = Input.GetAxis(inputSlowDownAlt);
    }

    void Sprint()
    {
        if ((Input.GetButton(inputSprint) || Input.GetAxis(inputSprintAlt) >= .1f) && stamina > 0 && sprintTime >= sprintCooldown)
        {
            if (!isSprinting)
                anim.SetBool("isSprinting", true);
            isSprinting = true;
            if (!isSlowing)
                UseStamina(sprintCostPerSecond);
        }
        else if (isSprinting)
        {
            isSprinting = false;
            if (stamina == 0)
                sprintTime = 0;
            anim.SetBool("isSprinting", false);
        }
    }

    void StopShooting()
    {
        AttackSoundSource.Stop();
        firePartSys.Stop();
        isShooting = false;
        foreach (BabyDragonBehaviour babyDragon in babyDragonMan.babyDragons)
        {
            babyDragon.StopShooting();
        }
        anim.SetBool("isShooting", false);
    }

    void LayEgg()
    {
        if (Input.GetButtonDown(inputInteract) && canLand)
        {
            SFXSource.PlayOneShot(EggDropping);
            eggMan.eggSlider.fillAmount = 0;
            eggMan.eggSlider.color = eggMan.startEggColor;

            if (playerState == PlayerStates.TURNING_AROUND)
                StopTurningAround();

            StartCoroutine(ILayEgg());
        }
    }
    
    void ChargeSelfDestruct()
    {
        if (gameMan.babyDragonMan.babyDragons.Count > 0)
        {
            if (Input.GetButtonDown(inputPlaceAncient))
            {
                StopShooting();

                selfDestructTime = 0;
                selfDestructFeedback.SetActive(true);
                selfDestructInputSlider.fillAmount = 0;
                gameMan.vibrationMan.VibrateFor(timeToSelfDestruct, 0, selfDestructLeftVibCurve, selfDestructRightVibCurve);

                isSlowing = true;
                anim.SetBool("isSprinting", false);
            }

            if (selfDestructFeedback.activeSelf)
            {
                if (Input.GetButton(inputPlaceAncient))
                {
                    selfDestructTime += Time.deltaTime;
                    selfDestructInputSlider.fillAmount = selfDestructTime / timeToSelfDestruct;
                    isSlowing = true;
                }

                if (selfDestructTime >= timeToSelfDestruct || Input.GetButtonUp(inputPlaceAncient))
                {
                    isSlowing = false;
                    gameMan.vibrationMan.StopVibrating(0);
                    selfDestructFeedback.SetActive(false);

                    if (selfDestructTime >= timeToSelfDestruct)
                    {
                        Debug.Log("BOOM");
                        StartCoroutine(ISelfDestruct());
                    }
                }
            }

        }
    }

    public void TurnAround(Vector3 _newDir)
    {
        playerState = PlayerStates.TURNING_AROUND;

        //Set correct speed
        isSlowing = false;
        isSprinting = false;
        speed = flySpeed;
        rotationLerp = flyingRotationLerp;

        //If is self destructing
        gameMan.vibrationMan.StopVibrating(0);
        selfDestructFeedback.SetActive(false);

        //Set direction
        desiredDir = _newDir;

        //Launch timer
        if (turnAroundCor != null)
            StopCoroutine(turnAroundCor);
        turnAroundCor = StartCoroutine(ITurnAround());
    }

    void StopTurningAround()
    {
        if (turnAroundCor != null)
            StopCoroutine(turnAroundCor);

        if (playerState == PlayerStates.TURNING_AROUND)
            playerState = PlayerStates.FLYING;
    }

    //Stamina

    public void UpdateStaminaUI()
    {
        StamiQuad.material.SetFloat ("_Stamina", stamina / maxStamina);
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

        SFXSource.PlayOneShot(DragonDeathClip, 1);
        if (babyDragonMan.babyDragons.Count > 0)
        {
            //Instantiate(placeholderFeedback, babyDragonMan.babyDragons[0].transform.position, Quaternion.identity);
            //Instantiate(placeholderFeedback, transform.position, Quaternion.identity);
            smokeScreen.Play();
            StartCoroutine(IPlaceholderNewMother());

            babyDragonMan.RemoveBabyDragon();

            ResetLife(2f);
            MakeInvincible(2f);

            StopShooting();
            aimProjector.SetActive(true);

            playerState = PlayerStates.FLYING;
        }
        else
            SceneManager.LoadScene(0);
    }

    public override void UpdateHealthUI(int _damage)
    {
        LifeQuad.material.SetFloat ("_Life", lifesShader);
        SFXSource.PlayOneShot(DragonHitClip, 0.2f);
        //base.UpdateHealthUI(_damage);
        LifeQuad.material.SetFloat ("_Life", life / maxLife);

        if (regenCor != null)
            StopCoroutine(regenCor);

        timeSinceLastDamage = 0;
        
        lostLifeBeforeDecay += _damage;
        LifeQuad.material.SetFloat ("_LifeBeforeDecay", lostLifeBeforeDecay);

        if (lifesShader == lifeFeedBackAmount || feedbackIsDecaying)
        {
            if (feedbackIsDecaying)
            {
                lifeFeedBackAmount = lifesShader;
                LifeQuad.material.SetFloat ("_LifeFeedbackAmount", lifeFeedBackAmount);
                feedbackIsDecaying = false;
            }
            if (feedbackCor != null)
                StopCoroutine(feedbackCor);
            feedbackCor = StartCoroutine(IHealthBarFeedback());
        }
        lifesShader = life/maxLife;

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
   
    internal override IEnumerator IHealthBarFeedback()
    {
        LifeQuad.material.SetFloat ("_DisplayLife", 1);        
        while (timeSinceLastDamage < timeToUpdateFeedbackBar)
        {
            timeSinceLastDamage += Time.deltaTime;
            yield return null;
        }
        lostLifeBeforeDecay = 0;
        float decayingTime = 0;
        feedbackIsDecaying = true;
        while (decayingTime < feedbackDecayTime)
        {
            decayingTime += Time.deltaTime;
            lifeFeedBackAmount = Mathf.Lerp(lifeFeedBackAmount, life / maxLife, decayingTime / feedbackDecayTime); 
            // Debug.Log(lifeFeedBackAmount);
            LifeQuad.material.SetFloat ("_LifeFeedbackAmount", lifeFeedBackAmount);

            yield return null;
        }
        LifeQuad.material.SetFloat ("_DisplayLife", 0);        
        feedbackIsDecaying = false;
    }

    internal override IEnumerator IHealthBarRegen(float _timeToRegen)
    {
        yield return new WaitForSeconds(1f);

        lifeFeedBackAmount = 1;
        LifeQuad.material.SetFloat ("_LifeFeedbackAmount", lifeFeedBackAmount);
        
        float time = 0;
        LifeQuad.material.SetFloat ("_DisplayLife", 1);        
        while (time < _timeToRegen)
        {
            time += Time.deltaTime;
            lifesShader = Mathf.Lerp(0, 1, time / _timeToRegen);
            LifeQuad.material.SetFloat ("_Life", lifesShader);
            yield return null;
        }
        LifeQuad.material.SetFloat ("_DisplayLife", 0);        
        lifesShader = lifeFeedBackAmount = 1;
        LifeQuad.material.SetFloat ("_LifeFeedbackAmount", lifeFeedBackAmount);
        LifeQuad.material.SetFloat ("_Life", lifesShader);

    }

    //Coroutines

    IEnumerator ILayEgg()
    {
        StopShooting();
        playerState = PlayerStates.LAYING_EGG;

        yield return new WaitForSeconds(1.0f);
        var instanceEgg = nestScript.Action();
        gameMan.spawnMan.eggs.Add(instanceEgg);

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

    IEnumerator ISelfDestruct()
    {
        StopShooting();
        MakeInvincible(4f);
        aimProjector.SetActive(false);

        rb.velocity = Vector3.zero;
        playerState = PlayerStates.SELF_DESTROYING;

        yield return new WaitForSeconds(selfDestructFreeze);

        gameMan.vibrationMan.VibrateFor(selfDestructLaunchVibTime, 0, .2f, 1f);

        Vector3 originPos = transform.position;
        Vector3 targetPos = shootTarget.position;
        float time = 0f;
        while (time < timeToPlunge)
        {
            time += Time.fixedDeltaTime;
            rb.MovePosition(Vector3.Lerp(originPos, targetPos, time / timeToPlunge));
            yield return new WaitForFixedUpdate();
        }

        Die();
    }

    IEnumerator ITurnAround()
    {
        yield return new WaitForSeconds(turnAroundLockTimeFactor / speed);
        turnAroundCor = null;
        StopTurningAround();
    }

    //IEnumerator IDodge()
    //{
    //    canDodge = false;
    //    StopShooting();
    //    playerState = PlayerStates.DODGING;

    //    desiredDir = new Vector3(hinput, 0f, vinput);
    //    if (desiredDir == Vector3.zero)
    //        desiredDir = transform.right;

    //    anim.SetTrigger("dodges");

    //    float time = 0;
    //    while (time < dodgeTime)
    //    {
    //        time += Time.deltaTime;
    //        rb.velocity = desiredDir * (isSprinting ? megaDodgeSpeed : dodgeSpeed);

    //        targetRot = Quaternion.LookRotation(desiredDir) * Quaternion.Euler(0f, 0f, 90f);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerp);
    //        yield return null;
    //    }

    //    playerState = PlayerStates.FLYING;

    //    yield return new WaitForSeconds(dodgeCooldown);
    //    canDodge = true;
    //}
}