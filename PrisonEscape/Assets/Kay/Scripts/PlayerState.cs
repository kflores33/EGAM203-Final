using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    public GameObject filterUI;      // reference to UI overlay shown when caught
    public GameObject gameOverPanel; // reference to UI game over panel

    private int timesCaught = 0;
    private bool wasMisbehavingLastFrame = false;

    private void Start()
    {
        _jailerBehavior = FindAnyObjectByType<JailerBehavior>(); // find the jailer behavior script in the scene
        CurrentState = PlayerStates.Behaving; // set initial state

        if (filterUI != null) filterUI.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        // for the sake of testing---subject to change
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (CurrentState == PlayerStates.Behaving)
                ChangeState(PlayerStates.Misbehaving);
            else
                ChangeState(PlayerStates.Behaving);
        }

        if (Input.GetKeyDown(KeyCode.D)) // Distract jailer
        {
            _jailerBehavior.DistractJailerFor(5f); // distract the jailer for 5 seconds
        }

        // Catching logic
        if (_jailerBehavior.CurrentState == JailerBehavior.JailerState.Alert &&
            CurrentState == PlayerStates.Misbehaving)
        {
            if (!wasMisbehavingLastFrame) // only trigger once per catch
            {
                timesCaught++;
                Debug.Log("Player caught! Times caught: " + timesCaught);

                if (filterUI != null)
                    filterUI.SetActive(true); // show red filter

                if (timesCaught >= 2 && gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true); // show game over
                }

                StartCoroutine(HideFilterAfterDelay(1.5f)); // hide red filter after short delay
            }

            wasMisbehavingLastFrame = true;
        }
        else
        {
            wasMisbehavingLastFrame = false;
        }
    }

    #region psuedo code
    /* in update function, check if both players' feet are within the bounds of the box
     * if all feet are within bounds, set state to behaving, else set state to misbehaving
     * 
     * also be sure to check if the player is touching things within the jail cell---if they are, that also counts as misbehavior
     */
    #endregion

    private IEnumerator HideFilterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (filterUI != null)
            filterUI.SetActive(false);
    }

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
