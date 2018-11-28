using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerVisualizer : MonoBehaviour
{
    public Image bannerFill;
    
    public void UpdateBanner(float _fill)
    {
        bannerFill.fillAmount = _fill;
    }
}
