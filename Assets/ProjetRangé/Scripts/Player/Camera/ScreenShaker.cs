using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    public bool canShake = true; //on or off
    public float trauma;
    float traumaMult; //the power of the shake
    float traumaMag; //the range of movment
    float traumaRotMag; //the rotational power
    float traumaDepthMag; //the depth multiplier
    float traumaDecay; //how quickly the shake falls off

    float timeCounter = 0; //counter stored for smooth transition
    public float Trauma //accessor is used to keep trauma within 0 to 1 range
    {
        get
        {
            return trauma;
        }
        set
        {
            trauma = Mathf.Clamp01(value);
        }
    }

    //Get a perlin float between -1 & 1, based off the time counter.
    float GetFloat(float seed)
    {
        return (Mathf.PerlinNoise(seed, timeCounter) - 0.5f) * 2f;
    }

    //use the above function to generate a Vector3, different seeds are used to ensure different numbers
    Vector3 GetVector3()
    {
        return new Vector3(
            GetFloat(1),
            GetFloat(10),
            //depth modifier applied here
            GetFloat(100) * traumaDepthMag
            );
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightControl))
        //{
        //    SetTrauma(1f, .5f, 10f, 2f);
        //}

        if (canShake && Trauma > 0)
        {
            //increase the time counter (how fast the position changes) based off the traumaMult and some root of the Trauma
            timeCounter += Time.deltaTime * Mathf.Pow(Trauma, 0.3f) * traumaMult;
            //Bind the movement to the desired range
            Vector3 newPos = GetVector3() * traumaMag * Trauma; ;
            transform.localPosition = newPos;
            //rotation modifier applied here
            transform.localRotation = Quaternion.Euler(newPos * traumaRotMag);
            //decay faster at higher values
            Trauma -= Time.deltaTime * traumaDecay * (Trauma + 0.3f);
        }
        else
        {
            //lerp back towards default position and rotation once shake is done
            Vector3 newPos = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime);
            transform.localPosition = newPos;
            transform.localRotation = Quaternion.Euler(newPos * traumaRotMag);
        }
    }

    public void SetTrauma(float _traumaDecay, float _trauma, float _traumaMult, float _traumaMagnitude, float _traumaRotMagnitude = 0.0f, float _traumaDepthMagnitude = 0.0f)
    {
        Trauma = _trauma;
        traumaDecay = _traumaDecay;
        traumaMult = _traumaMult;
        traumaMag = _traumaMagnitude;
        traumaRotMag = _traumaRotMagnitude;
        traumaDepthMag = _traumaDepthMagnitude;
    }
}