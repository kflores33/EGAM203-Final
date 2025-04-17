using UnityEngine;

/// <summary>
/// Reduces the jailer's remaining away time by 5 seconds when the player presses "H".
/// Ensures at least 3 seconds remain for audio/alert reaction.
/// </summary>
public class ReduceJailerAwayTime : MonoBehaviour
{
    public JailerBehavior jailer;

    private void Start()
    {
        if (jailer == null)
        {
            jailer = FindAnyObjectByType<JailerBehavior>();
        }

        if (jailer == null)
        {
            Debug.LogError("JailerBehavior not found in scene.");
        }
    }

    private void Update()
    {
        if (jailer == null) return;

        if (Input.GetKeyDown(KeyCode.H) && jailer.CurrentState == JailerBehavior.JailerState.Away)
        {
            jailer.ReduceTimeRemaining(10f);
        }
    }
}
