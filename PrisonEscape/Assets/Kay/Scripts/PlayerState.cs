using UnityEngine;

/// <summary>
/// This script checks both players' states and determines if they are behaving or misbehaving.
/// So, there should only be one instance of this script in the scene.
/// </summary>

public class PlayerState : MonoBehaviour
{
    JailerBehavior _jailerBehavior; // reference to the jailer behavior script
    public enum PlayerStates
    {
        Behaving,
        Misbehaving
    }
    public PlayerStates CurrentState;

    private void Start()
    {
        _jailerBehavior = FindAnyObjectByType<JailerBehavior>(); // find the jailer behavior script in the scene
        CurrentState = PlayerStates.Behaving; // set initial state
    }

    private void Update()
    {
        // for the sake of testing---subject to change
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (CurrentState == PlayerStates.Behaving)
            {
                ChangeState(PlayerStates.Misbehaving);
            }
            else if (CurrentState == PlayerStates.Misbehaving)
            {
                ChangeState(PlayerStates.Behaving);
            }
        }

        if (Input.GetKeyDown(KeyCode.D)) // Distract jailer
        {
            _jailerBehavior.DistractJailerFor(5f); // distract the jailer for 5 seconds
        }
    }

    #region psuedo code
    /* in update function, check if both players' feet are within the bounds of the box
     * if all feet are within bounds, set state to behaving, else set state to misbehaving
     * 
     * also be sure to check if the player is touching things within the jail cell---if they are, that also counts as misbehavior
     */
    #endregion

    private void ChangeState(PlayerStates newState)
    {
        CurrentState = newState;
        Debug.Log("Player state changed to: " + CurrentState);
    }

    public PlayerStates GetCurrentState() // for other scripts to access without modifying
    {
        return CurrentState;
    }
}
