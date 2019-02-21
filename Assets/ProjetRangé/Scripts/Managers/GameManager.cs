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
	public LeaderBoard lb;

    [Header ("SetUp Instance")]
    public GameObject playerControllerPrefab;
    public GameObject playerControllerInstance;
    // public 

    [Header("Spawn")]
    public GameObject waveCanvas;
    
    [Header("Timer")]
    public TimeManager timeMan;
    [Header("Parameters")]
    public ParameterManager paraMan;

    public GameObject[] tutorials;
    int currentTutorial= -1;
    internal bool gotFirstBabyDragon;
    internal bool selfDestructed;

    private void Awake()
    {
      CreateInstances();
      scoreManager = UI.GetComponentInChildren<ScoreManager>();
      lb.LoadLeaderBoard();

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        InitManagers();
        timeMan.LaunchTimer();
    }

    private void Update()
    {
        if (currentTutorial == -1)
            NextTutorial();
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

    public void NextTutorial()
    {
        currentTutorial++;

        if (currentTutorial > 0)
            tutorials[currentTutorial -1].SetActive(false);

        if (currentTutorial == tutorials.Length)
        {
            spawnMan.BeginWave();
            //CALL TRUE EGG
        }
        else
        {
            tutorials[currentTutorial].SetActive(true);
            StartCoroutine(ITutorial());
        }
    }

    IEnumerator ITutorial()
    {
        switch (currentTutorial)
        {
            case 0:
                yield return new WaitForSeconds(5f);
                NextTutorial();
                break;
            case 1:
                while (Input.GetAxis(player.inputSprint) < .1f && Input.GetAxis(player.inputSprintAlt) < .1f)
                {
                    yield return null;
                }
                NextTutorial();
                break;
            case 2:
                while (Input.GetAxis(player.inputSlowDown) < .1f && Input.GetAxis(player.inputSlowDownAlt) < .1f)
                {
                    yield return null;
                }
                NextTutorial();
                break;
            case 3:
                while (!Input.GetButton(player.inputShoot))
                {
                    yield return null;
                }
                NextTutorial();
                //CALL TUTORIAL EGG
                break;
            case 4:
                while (!gotFirstBabyDragon)
                {
                    yield return null;
                }
                NextTutorial();
                break;
            case 5:
                while (!Input.GetButtonDown(player.inputInteract))
                {
                    yield return null;
                }
                NextTutorial();
                break;
            case 6:
                while (!selfDestructed)
                {
                    yield return null;
                }
                NextTutorial();
                break;
        }
    }
}

      CreateInstances();
      paraMan = ParameterManager.Instance;
      scoreManager = UI.GetComponentInChildren<ScoreManager>();
      lb.LoadLeaderBoard();