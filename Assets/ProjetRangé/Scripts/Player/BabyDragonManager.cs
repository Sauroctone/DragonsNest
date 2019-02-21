using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDragonManager : MonoBehaviour {

    public Transform target;
    public float followLerp;
    public float rotLerp;
    public int maxBabyDragonCount;
    public List<BabyDragonBehaviour> babyDragons = new List<BabyDragonBehaviour>();
    public float containerDistance;
    public float angleToDivide;
    public GameObject babyDragon;
    public float shootOffsetRange;
    public MeshRenderer LifeQuad;
    public MeshRenderer StamiQuad;

    private void Start()
    {
        babyDragons.AddRange(GetComponentsInChildren<BabyDragonBehaviour>());
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, followLerp /** Time.deltaTime*/);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotLerp /** Time.deltaTime*/);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnNewBabyDragon();

        }
    }

    public void SpawnNewBabyDragon()
    {
        GameObject baby = Instantiate(babyDragon, transform);
        babyDragons.Add(baby.GetComponentInChildren<BabyDragonBehaviour>());
        GiveNewPositions();

        if (!GameManager.Instance.gotFirstBabyDragon)
            GameManager.Instance.gotFirstBabyDragon = true;
    }

    public void RemoveBabyDragon()
    {
        Destroy(babyDragons[0].gameObject);
        babyDragons.RemoveAt(0);
        GiveNewPositions();
    }

    void GiveNewPositions()
    {
        for (int i = 0; i < babyDragons.Count; i++)
        {
            babyDragons[i].transform.parent.localPosition = new Vector3(containerDistance * Mathf.Cos(-(angleToDivide / babyDragons.Count * i) * Mathf.Deg2Rad), 0f, containerDistance * Mathf.Sin(-(angleToDivide / babyDragons.Count * i) * Mathf.Deg2Rad));
            babyDragons[i].targetOffset = new Vector3(shootOffsetRange * Mathf.Cos(-(angleToDivide / babyDragons.Count * i) * Mathf.Deg2Rad), 0f, shootOffsetRange * Mathf.Sin(-(angleToDivide / babyDragons.Count * i) * Mathf.Deg2Rad));
        }
    }
}