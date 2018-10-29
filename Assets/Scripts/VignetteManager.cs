using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class VignetteManager : MonoBehaviour {

    VignettePreset currentPreset;

    public VignettePreset CurrentPreset
    {
        get
        {
            return currentPreset;
        }
    }

    float timeBeforeSmoothnessDecay;
    bool smoothnessIsDecaying;
    [Header("Presets")]
    public VignettePreset basicVignette;
    public VignettePreset hurtVignette;
    [Header("References")]
    public PostProcessingProfile profile;
    VignetteModel.Settings settings;
    Coroutine changePresetCor;
    Coroutine smoothnessDecayCor;

    private void Start()
    {
        currentPreset = basicVignette;

        //Set the "original value" variables - there should be a better way to maintain this
        hurtVignette.originalSmoothness = basicVignette.smoothness;
    }

    public void ChangeVignette(VignettePreset _preset)
    {
        if (smoothnessDecayCor != null)
            StopCoroutine(smoothnessDecayCor);

        if (changePresetCor != null)
            StopCoroutine(changePresetCor);
        changePresetCor = StartCoroutine(ILerpValues(_preset));
        currentPreset = _preset;
    }

    public void IncrementSmoothness(VignettePreset _preset, float _increment)
    {
        if (!_preset.incrementSmoothness)
        {
            Debug.LogWarning("[" + _preset.name + "]'s smoothness cannot be incremented. Try flagging the appropriate boolean as 'true' in the VignettePreset asset.");
            return;
        }

        timeBeforeSmoothnessDecay = 0;

        if (_preset.smoothness + _increment > _preset.maxSmoothness)
            return;

        if (_preset.smoothness == _preset.originalSmoothness || smoothnessIsDecaying) 
        {
            if (smoothnessIsDecaying)
            {
                smoothnessIsDecaying = false;
                _preset.smoothness = _preset.originalSmoothness;
            }
            if (smoothnessDecayCor != null)
                StopCoroutine(smoothnessDecayCor);
            smoothnessDecayCor = StartCoroutine(IDecaySmoothness(_preset));
        }
        _preset.smoothness += _increment;
        _preset.smoothness = Mathf.Clamp(_preset.smoothness, 0f, _preset.maxSmoothness);
    }

    IEnumerator IDecaySmoothness(VignettePreset _preset)
    { 
        while(timeBeforeSmoothnessDecay < _preset.timeBeforeSmoothnessDecay)
        {
            timeBeforeSmoothnessDecay += Time.deltaTime;
            yield return null;
        }
        smoothnessIsDecaying = true;
        while (_preset.smoothness > _preset.originalSmoothness)
        {
            _preset.smoothness -= _preset.smoothnessDecayPerSecond * Time.deltaTime;
            yield return null;
        }
        _preset.smoothness = _preset.originalSmoothness;
        smoothnessIsDecaying = false;
    }

    IEnumerator ILerpValues(VignettePreset _preset)
    {
        if (_preset.incrementSmoothness)
            _preset.smoothness = _preset.originalSmoothness;

        settings = profile.vignette.settings;
        float time = 0;
        while (time < _preset.transitionTime)
        {
            time += Time.deltaTime;
            settings.color = Color.Lerp(settings.color, _preset.color, time/_preset.transitionTime);
            settings.intensity = Mathf.Lerp(settings.intensity, _preset.intensity, time / _preset.transitionTime);
            settings.smoothness = Mathf.Lerp(settings.smoothness, _preset.smoothness, time / _preset.transitionTime);
            settings.roundness = Mathf.Lerp(settings.roundness, _preset.roundness, time / _preset.transitionTime);
            profile.vignette.settings = settings;
            yield return null;
        }
    }
}