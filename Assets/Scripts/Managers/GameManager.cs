﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public CameraBehaviour mainCamera;
    [Header("Player")]
    public PlayerController player;
    public Rigidbody playerRb;
    [Header("Managers")]
    public VignetteManager vignetteMan;
    public SpawnManager spawnMan;
    public BabyDragonManager babyDragonMan;
    public VibrationManager vibrationMan;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        InitManagers();
    }

    void InitManagers()
    {
        spawnMan.Init();
    }
}
