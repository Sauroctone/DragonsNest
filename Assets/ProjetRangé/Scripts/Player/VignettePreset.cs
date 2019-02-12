using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/VignettePresets/VignettePreset.asset", menuName = "Vignette Preset")]
public class VignettePreset : ScriptableObject {

    [Header("Transition")]
    public float transitionTime;

    [Header("Settings")]
    public Color color = Color.black;
    [Range(0f, 1f)]
    public float intensity;
    [Range(0f, 1f)]
    public float smoothness;
    [Range(0f, 1f)]
    public float roundness;

    [Header("Increment Smoothness - make HideAttribute")]
    public bool incrementSmoothness;
    [Range(0f, 1f)]
    public float maxSmoothness;
    [HideInInspector]
    public float originalSmoothness;
    public float timeBeforeSmoothnessDecay;
    public float smoothnessDecayPerSecond;
}
