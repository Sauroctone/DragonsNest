using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public EggManager eggMan;
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

    [Header("Spawn")]
    public GameObject waveCanvas;

    private void Awake()
    {
      CreateInstances();
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
        playerRb = player.rb;
        babyDragonMan = player.babyDragonMan;
        camBehaviour = Instantiate(camBehaviour.gameObject).GetComponent<CameraBehaviour>();
        mainCamera = camBehaviour.gameObject.GetComponentInChildren<Camera>();
        UI = Instantiate(UI);
        spawnMan.waveTimerText = UI.transform.GetChild(2).GetComponentInChildren<Text>();
    }   
}
