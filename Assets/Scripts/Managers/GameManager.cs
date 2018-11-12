using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public PlayerController player;
    public Rigidbody playerRb;
    public VignetteManager vignetteMan;
    public SpawnManager spawnMan;
    public BabyDragonManager babyDragonMan;

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
