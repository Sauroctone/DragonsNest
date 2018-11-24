using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerVisualizer : MonoBehaviour
{
    public Renderer rend;
    
    public void ChangeBanner(Material _newBanner)
    {
        rend.material = _newBanner;
    }
}
