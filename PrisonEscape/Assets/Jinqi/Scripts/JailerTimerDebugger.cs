using UnityEngine;

/// <summary>
/// Accurately debugs the time left until the jailer becomes alert using real time from JailerBehavior.
/// </summary>
public class JailerTimerDebugger : MonoBehaviour
{
    public JailerBehavior jailer;

    private bool isCounting = false;
    private JailerBehavior.JailerState lastState;

    private void Start()
    {
        if (jailer == null)
        {
            jailer = FindAnyObjectByType<JailerBehavior>();
        }

        lastState = jailer.CurrentState;

        if (jailer.CurrentState == JailerBehavior.JailerState.Away)
        {
            isCounting = true;
        }
    }

    private void Update()
    {
        if (jailer == null) return;

        // Detect state changes
        if (jailer.CurrentState != lastState)
        {
            if (jailer.CurrentState == JailerBehavior.JailerState.Away)
            {
                isCounting = true;
                Debug.Log("[JailerTimerDebugger] Timer started. Jailer is away.");
            }
            else if (jailer.CurrentState == JailerBehavior.JailerState.Alert)
            {
                isCounting = false;
                Debug.Log("[JailerTimerDebugger] Timer stopped. Jailer is alert.");
            }

            lastState = jailer.CurrentState;
        }

        if (isCounting)
        {
            float remaining = jailer.GetTimeRemaining();
            Debug.Log($"[JailerTimerDebugger] Time until alert: {remaining:F2} seconds");
        }
    }
}
