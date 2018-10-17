﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public enum PlayerStates { FLYING, DODGING };
    public PlayerStates playerState;
    [Header("Flying")]
    public float flySpeed;
    internal Vector3 desiredDir = Vector3.forward;
    Vector3 lastDesiredDir = Vector3.forward;
    float hinput;
    float vinput;
    float rotationAngle;
    public float rotationLerp;
    Quaternion targetRot;
    public float maxSteerRot;
    Vector3 dirDiff;
    public bool isSprinting;
    public float sprintSpeed;
    public float sprintCooldown;
    float sprintTime;

    [Header("Stamina")]
    public float maxStamina;
    float stamina;
    public float sprintCostPerSecond;
    public float regenPerSecond;
    public float regenCooldown;
    float regenTime;

    [Header("Shooting")]
    public float timeBetweenCols;
    internal bool isShooting;
    float fireTime;
    public float fireColSpeed;
    public Transform shootTarget;

    [Header("Dodging")]
    public float dodgeSpeed;
    public float megaDodgeSpeed;
    public float dodgeTime;
    Coroutine dodgeCor;
    bool canDodge = true;
    public float dodgeCooldown;

    [Header("References")]
    public ParticleSystem firePartSys;
    public Transform fireOrigin;
    public Rigidbody rb;
    public GameObject fireCollider;
    public Animator anim;
    public Slider staminaBar;
    public BabyDragonManager babyDragonMan;

    void Start()
    {
        stamina = maxStamina;
        sprintTime = sprintCooldown;
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
                rb.velocity = transform.forward * (isSprinting ? sprintSpeed : flySpeed);

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
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerp);
                }
                break;

            case PlayerStates.DODGING:
                break;
        }
    }

    private void Update()
    {
        RegenStamina();

        switch (playerState)
        {
            case PlayerStates.FLYING:
                Shoot();

                //Sprinting
                if (Input.GetAxis("Sprint") > .1f && stamina > 0 && sprintTime >= sprintCooldown)
                {
                    isSprinting = true;
                    UseStamina(sprintCostPerSecond);
                }
                else if (isSprinting)
                {
                    isSprinting = false;
                    if (stamina == 0)
                        sprintTime = 0;
                }

                //Dodge
                if (Input.GetAxis("Dodge") > .1f && canDodge)
                    Dodge();
                break;

            case PlayerStates.DODGING:
                break;
        }

        UpdateUI();
    }

    void Shoot()
    {
        //Begin shooting
        if (Input.GetButtonDown("Fire1"))
        {
            firePartSys.Play();
            isShooting = true;
            fireTime = 0;

            foreach(BabyDragonBehaviour babyDragon in babyDragonMan.babyDragons)
            {
                babyDragon.Shoot(shootTarget, rb);
            }
        }

        //End shooting
        if (Input.GetButtonUp("Fire1"))
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

    void UpdateUI()
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
    }

    void Dodge()
    {
        dodgeCor = StartCoroutine(IDodge());
    }

    void StopShooting()
    {
        firePartSys.Stop();
        isShooting = false;
        foreach (BabyDragonBehaviour babyDragon in babyDragonMan.babyDragons)
        {
            babyDragon.StopShooting();
        }
    }

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
}