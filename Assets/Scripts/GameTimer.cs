using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        UpdateDisplay();
    }

    public void StartTimer() => isRunning = true;
    public void PauseTimer() => isRunning = false;
    public void ResumeTimer() => isRunning = true;

    public void StopTimer()
    {
        isRunning = false;
        UpdateDisplay();
        Debug.Log("Tempo final: " + GetFormattedTime());
    }

    void UpdateDisplay()
    {
        if (timerText != null)
            timerText.text = GetFormattedTime();
    }

    public string GetFormattedTime()
    {
        int hours = (int)(elapsedTime / 3600);
        int minutes = (int)(elapsedTime % 3600 / 60);
        int seconds = (int)(elapsedTime % 60);
        int millis = (int)((elapsedTime % 1) * 100);

        if (hours > 0)
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        else
            return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, millis);
    }

    public float GetElapsedTime() => elapsedTime;
}