using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerBillboard : MonoBehaviour {

    Transform cam;

    private void Start()
    {
        cam = GameManager.Instance.mainCamera.transform;
    }

    private void LateUpdate()
    {
        Vector3 projectedPos = Vector3.ProjectOnPlane(cam.position, Vector3.up);
        transform.LookAt(projectedPos);
    }
}
