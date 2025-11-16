using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float startTime = 15f; // starting time
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool timerIsRunning = false;
    private bool gameEnded = false;

    void Start()
    {
        // Do NOT auto-start the timer here
        timeRemaining = startTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (!timerIsRunning || gameEnded)
            return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            timeRemaining = 0;
            timerIsRunning = false;
            WinGame();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }

    public void StartTimer()
    {
        gameEnded = false;
        timerIsRunning = true;
        timeRemaining = startTime;
        UpdateTimerText();
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }

    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        timerIsRunning = false;

        if (GameManager.instance != null)
            GameManager.instance.GameWin();
    }

    public void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        timerIsRunning = false;

        if (GameManager.instance != null)
            GameManager.instance.GameOver();
    }
}
