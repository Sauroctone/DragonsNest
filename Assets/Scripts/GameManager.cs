﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public PlayerController player;
    public VignetteManager vignetteMan;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            vignetteMan.ChangeVignette(vignetteMan.hurtVignette);

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            vignetteMan.ChangeVignette(vignetteMan.basicVignette);
        }
    }
}
