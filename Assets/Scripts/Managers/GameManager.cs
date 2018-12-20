using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public CameraBehaviour camBehaviour;
    public Camera mainCamera;
    [Header("Player")]
    public PlayerController player;
    public Rigidbody playerRb;
    [Header("Managers")]
    public VignetteManager vignetteMan;
    public SpawnManager spawnMan;
    public BabyDragonManager babyDragonMan;
    public VibrationManager vibrationMan;
    [Header("Environment")]
    public Terrain terrain;
    public TerrainData terrainData;
    
    [Header("Score")]  
    public GameObject UI;
    public GameObject scoreCanevas;
    public ScoreManager scoreManager;

    [Header ("SetUp Instance")]
    public GameObject playerControllerPrefab;
    public GameObject playerControllerInstance;
    // public 

    private void Awake()
    {
      //  CreateInstances();
      scoreManager = UI.GetComponentInChildren<ScoreManager>();
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

    public void CreateInstances()
    {
        // playerControllerInstance = Instantiate(playerControllerPrefab, _transform.position, _transform.rotation);
        playerControllerInstance = Instantiate(playerControllerPrefab,new Vector3(0,4.86f,0), Quaternion.identity);
        player = playerControllerInstance.GetComponent<PlayerController>();
        babyDragonMan = player.babyDragonMan;
        mainCamera = Instantiate(mainCamera.gameObject).GetComponent<Camera>();
        Instantiate(UI);
        scoreManager = UI.GetComponentInChildren<ScoreManager>();
    }   
}
