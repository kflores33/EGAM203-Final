using UnityEngine;
using System.Collections;

public class JailerBehavior : MonoBehaviour
{
    PlayerState _playerState;
    Coroutine _switchStateCoroutine;

    public enum JailerState
    {
        Away,
        Alert
    }
    public JailerState CurrentState;

    public float DefaultTimeToWait = 5f;
    private float _totalTimeRemaining;     // 统一计时变量
    private float _footstepDuration;       // 警告阶段长度（仅用于音效淡入）
    private bool _footstepStarted = false;

    [Header("Footstep Settings")]
    public AudioSource footstepAudio;
    public Vector2 footstepDurationRange = new Vector2(3f, 5f);

    private void Start()
    {
        _playerState = FindAnyObjectByType<PlayerState>();
        CurrentState = JailerState.Away;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log($"[Debug] H pressed. TotalTimeRemaining: {_totalTimeRemaining:F2}");
            ReduceTimeRemaining(10f);
        }

        switch (CurrentState)
        {
            case JailerState.Away:
                if (_switchStateCoroutine == null)
                {
                    float randomizedTime = Random.Range(10f, 20f);
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
        }
    }

    IEnumerator CountdownToSwitchState(float timeToWait)
    {
        // 初始化总时间（主倒计时 + footstep 阶段）
        _footstepDuration = Random.Range(footstepDurationRange.x, footstepDurationRange.y);
        _totalTimeRemaining = timeToWait + _footstepDuration;
        _footstepStarted = false;

        if (footstepAudio != null)
        {
            footstepAudio.volume = 0f;
        }

        while (_totalTimeRemaining > 0f)
        {
            _totalTimeRemaining -= Time.deltaTime;

            // 到达 footstep 阶段开始播放音效
            if (!_footstepStarted && _totalTimeRemaining <= _footstepDuration)
            {
                _footstepStarted = true;
                if (footstepAudio != null)
                {
                    footstepAudio.Play();
                }
            }

            // 音量淡入处理
            if (_footstepStarted && footstepAudio != null)
            {
                float fadeProgress = 1f - (_totalTimeRemaining / _footstepDuration);
                footstepAudio.volume = Mathf.Clamp01(fadeProgress);
            }

            yield return null;
        }

        // 停止音效
        if (footstepAudio != null)
        {
            footstepAudio.Stop();
            footstepAudio.volume = 1f;
        }

        // 切换状态
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

        _switchStateCoroutine = null;
    }

    public void DistractJailerFor(float timeToWait)
    {
        if (CurrentState == JailerState.Alert)
        {
            Debug.Log("Jailer is already Alert, no need to reset wait time.");
            return;
        }

        if (_switchStateCoroutine != null)
        {
            StopCoroutine(_switchStateCoroutine);
            _switchStateCoroutine = null;
        }

        _switchStateCoroutine = StartCoroutine(CountdownToSwitchState(_totalTimeRemaining + timeToWait));
    }

    public float GetTimeRemaining()
    {
        return Mathf.Max(0f, _totalTimeRemaining);
    }

    public void ReduceTimeRemaining(float amount)
    {
        if (CurrentState != JailerState.Away) return;

        float minSafeTime = 3f;

        if (_totalTimeRemaining <= minSafeTime)
        {
            Debug.Log($"[ReduceTime] Already near alert phase. _totalTimeRemaining: {_totalTimeRemaining:F2}, can't reduce.");
            return;
        }

        float targetTime = _totalTimeRemaining - amount;

        if (targetTime < minSafeTime)
        {
            Debug.Log($"[ReduceTime] Requested -{amount:F2} would go too low. Reducing to safe minimum {minSafeTime:F2}s.");
            _totalTimeRemaining = minSafeTime;
        }
        else
        {
            float before = _totalTimeRemaining;
            _totalTimeRemaining = targetTime;
            Debug.Log($"[ReduceTime] Reduced jailer timer from {before:F2} → {_totalTimeRemaining:F2}.");
        }
    }
}
