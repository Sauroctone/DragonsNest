using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AncientFeedback : MonoBehaviour {

	public Material availableMat;
    public Material notAvailableMat;
    public Renderer[] rends;
    public GameObject cone;
    public bool isAvailable = true;

    public void ChangeMat(Material _mat, bool _available)
    {
        foreach (Renderer rend in rends)
            rend.material = _mat;

        cone.SetActive(_available);

        isAvailable = _available;
    }
}
