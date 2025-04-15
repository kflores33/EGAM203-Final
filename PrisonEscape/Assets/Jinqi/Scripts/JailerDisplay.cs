using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JailerDisplay : MonoBehaviour
{
    public JailerBehavior jailer;
    private RawImage rawImage;

    private JailerBehavior.JailerState _lastState;
    private Coroutine transitionRoutine;

    void Start()
    {
        if (jailer == null)
        {
            jailer = FindAnyObjectByType<JailerBehavior>();
        }

        rawImage = GetComponent<RawImage>();

        if (rawImage == null)
        {
            Debug.LogError("No RawImage component found on " + gameObject.name);
        }

        _lastState = jailer.CurrentState;
        SetColorInstant(_lastState);
    }

    void Update()
    {
        if (jailer == null || rawImage == null) return;

        if (jailer.CurrentState != _lastState)
        {
            if (transitionRoutine != null) StopCoroutine(transitionRoutine);
            transitionRoutine = StartCoroutine(BlinkAndTransition(jailer.CurrentState));
            _lastState = jailer.CurrentState;
        }
    }

    void SetColorInstant(JailerBehavior.JailerState state)
    {
        rawImage.color = (state == JailerBehavior.JailerState.Alert) ? Color.white : Color.black;
    }

    IEnumerator BlinkAndTransition(JailerBehavior.JailerState targetState)
    {
        Color finalColor = (targetState == JailerBehavior.JailerState.Alert) ? Color.white : Color.black;
        Color blinkColor = Color.gray;

        // Blink 3 times
        for (int i = 0; i < 3; i++)
        {
            rawImage.color = blinkColor;
            yield return new WaitForSeconds(0.1f);
            rawImage.color = finalColor;
            yield return new WaitForSeconds(0.1f);
        }

        // Smooth transition to final color over 0.7 seconds
        float duration = 0.7f;
        float elapsed = 0f;
        Color startColor = rawImage.color;

        while (elapsed < duration)
        {
            rawImage.color = Color.Lerp(startColor, finalColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rawImage.color = finalColor;
        transitionRoutine = null;
    }
}
