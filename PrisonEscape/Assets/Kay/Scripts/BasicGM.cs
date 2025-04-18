using TMPro;
using UnityEngine;

public class BasicGM : MonoBehaviour
{
    public float timeRemaining = 270f;

    public TMP_Text timerText;

    public GameObject endScreenWin;
    public GameObject endScreenLose;

    public BasicGM instance;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            Debug.Log("Game Over");
            // Here you can add logic to end the game or restart it
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log($"[Debug] H pressed. TimeRemaining: {timeRemaining:F2}");
            ReduceTimeRemaining(10f);
        }
    }

    public void ReduceTimeRemaining(float amount)
    {
        timeRemaining -= amount;
        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }
    }

    void UpdateTimerDisplay()
    {
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowEndScreen(bool win)
    {
        if (win)
        {
            endScreenWin.SetActive(true);
        }
        else
        {
            endScreenLose.SetActive(true);
        }
    }
}
