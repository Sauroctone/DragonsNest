using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class VibrationManager : MonoBehaviour {

    Coroutine vibrationCor;

	public void StartVibrating(int _playerIndex, float _leftMotor, float _rightMotor)
    {
        if (vibrationCor != null)
            StopCoroutine(vibrationCor);

        GamePad.SetVibration((PlayerIndex)_playerIndex, _leftMotor, _rightMotor);
    }

    public void StopVibrating(int _playerIndex)
    {
        if (vibrationCor != null)
            StopCoroutine(vibrationCor);

        GamePad.SetVibration((PlayerIndex)_playerIndex, 0f, 0f);
    }

    public void VibrateFor(float _timeToVibrate, int _playerIndex, float _leftMotor, float _rightMotor)
    {
        if (vibrationCor != null)
            StopCoroutine(vibrationCor);

        vibrationCor = StartCoroutine(IVibrateFor(_timeToVibrate, (PlayerIndex)_playerIndex, _leftMotor, _rightMotor));
    }

    public void VibrateFor(float _timeToVibrate, int _playerIndex, AnimationCurve _leftMotorCurve, AnimationCurve _rightMotorCurve, int _loopCount = 1)
    {
        if (vibrationCor != null)
            StopCoroutine(vibrationCor);

        vibrationCor = StartCoroutine(IVibrateFor(_timeToVibrate, (PlayerIndex)_playerIndex, _leftMotorCurve, _rightMotorCurve, _loopCount));
    }

    IEnumerator IVibrateFor(float _timeToVibrate, PlayerIndex _playerIndex, float _leftMotor, float _rightMotor)
    {
        float time = 0;
        while (time < _timeToVibrate)
        {
            time += Time.deltaTime;
            GamePad.SetVibration(_playerIndex, _leftMotor, _rightMotor);
            yield return null;
        }
        GamePad.SetVibration(_playerIndex, 0f, 0f);
    }

    IEnumerator IVibrateFor(float _timeToVibrate, PlayerIndex _playerIndex, AnimationCurve _leftMotorCurve, AnimationCurve _rightMotorCurve, int _loopCount = 1)
    {
        float time = 0;
        float loopTime = 0;
        while (time < _timeToVibrate)
        {
            time += Time.deltaTime;
            loopTime += Time.deltaTime;
            if (loopTime > _timeToVibrate / _loopCount)
                loopTime = 0;

            GamePad.SetVibration(_playerIndex, _leftMotorCurve.Evaluate(loopTime / (_timeToVibrate / _loopCount)), _rightMotorCurve.Evaluate(loopTime /(_timeToVibrate / _loopCount)));

            yield return null;
        }
        GamePad.SetVibration(_playerIndex, 0f, 0f);
    }
}
