using UnityEngine;
using UnityEngine.UI;

public class K_GuardAnim : MonoBehaviour
{
    public JailerBehavior jailer;
    public PlayerState playerState; // reference to the player state script

    private JailerBehavior.JailerState _lastState;
    private Coroutine transitionRoutine;

    public Animator animator;

    void Start()
    {
        if (jailer == null)
        {
            jailer = FindAnyObjectByType<JailerBehavior>();
        }

        if (playerState == null)
        {
            playerState = FindAnyObjectByType<PlayerState>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        _lastState = jailer.CurrentState;
    }

    void Update()
    {
        if (jailer == null) return;

        if(jailer.CurrentState == JailerBehavior.JailerState.Alert)
        {
            if(playerState.CurrentState == PlayerState.PlayerStates.Misbehaving)
            {
                animator.SetTrigger("Caught");
            }
        }
    }

    float knownLength = 6f;

    public void Do_SlowedAnim()
    {
        animator.speed = knownLength / jailer._footstepDuration;
    }

    public void ANIM_OnComplete()
    {
        animator.speed = 1;
    }
}
