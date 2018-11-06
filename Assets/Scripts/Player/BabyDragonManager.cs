using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDragonManager : MonoBehaviour {

    public Transform target;
    public float followLerp;
    public float rotLerp;
    public List<BabyDragonBehaviour> babyDragons = new List<BabyDragonBehaviour>();
    public float containerDistance;
    public float angleToDivide;
    public GameObject babyDragon;

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
            SpawnNewBabyDragon();
    }

    public void SpawnNewBabyDragon()
    {
        GameObject baby = Instantiate(babyDragon, transform);
        babyDragons.Add(baby.GetComponentInChildren<BabyDragonBehaviour>());
        GiveNewPositions();
    }

    void GiveNewPositions()
    {
        for (int i = 0; i < babyDragons.Count; i++)
        {
            babyDragons[i].transform.parent.localPosition = new Vector3(containerDistance * Mathf.Cos(-(angleToDivide / babyDragons.Count * i) * Mathf.Deg2Rad), 0f, containerDistance * Mathf.Sin(-(angleToDivide / babyDragons.Count * i) * Mathf.Deg2Rad));
        }
    }
}
