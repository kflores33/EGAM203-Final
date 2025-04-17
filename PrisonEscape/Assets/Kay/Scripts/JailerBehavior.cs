using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class JailerBehavior : MonoBehaviour
{
    PlayerState _playerState; // reference to the player state script
    Coroutine _switchStateCoroutine;

    public enum JailerState
    {
        Away,
        Alert
    }
    public JailerState CurrentState;

    public float DefaultTimeToWait = 5f;
    float _timeRemaining;

    private void Start()
    {
        _playerState = FindAnyObjectByType<PlayerState>(); // find the player state script in the scene
        CurrentState = JailerState.Away; // set initial state
    }

    IEnumerator CountdownToSwitchState(float timeToWait)
    {
        for (_timeRemaining = timeToWait; _timeRemaining > 0; _timeRemaining -= Time.deltaTime)
        {
            yield return null;
        }

        if (CurrentState == JailerState.Away)
        {
            CurrentState = JailerState.Alert;
            Debug.Log("Jailer is now Alert");
        }
        else
        {
            CurrentState = JailerState.Away;
            Debug.Log("Jailer is now Away");
        }

        _timeRemaining = 0f;
        _switchStateCoroutine = null;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case JailerState.Away:
                if (_switchStateCoroutine == null)
                {
                    float randomizedTime = Random.Range(5f, 20f); // randomize 10-20s only for Away state
                    _switchStateCoroutine = StartCoroutine(CountdownToSwitchState(randomizedTime));
                }
                break;
            case JailerState.Alert:
                if (_switchStateCoroutine == null)
                {
                    _switchStateCoroutine = StartCoroutine(CountdownToSwitchState(DefaultTimeToWait));
                }
                UpdateAlert();
                break;
        }
    }

    private void UpdateAlert()
    {
        if (_playerState.GetCurrentState() == PlayerState.PlayerStates.Misbehaving)
        {
            Debug.Log("Jailer caught the player misbehaving!");
            // end game
        }
    }

    public void DistractJailerFor(float timeToWait)
    {
        if (CurrentState == JailerState.Alert)
        {
            Debug.Log("Jailer is already Alert, no need to reset wait time.");
            return;
        }

        float remainingTime = _timeRemaining;

        if (_switchStateCoroutine != null)
        {
            StopCoroutine(_switchStateCoroutine);
            _switchStateCoroutine = null;
        }

        _switchStateCoroutine = StartCoroutine(CountdownToSwitchState(timeToWait + remainingTime));
    }
}
