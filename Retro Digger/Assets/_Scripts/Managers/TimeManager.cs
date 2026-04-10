using UnityEngine;
using System.Diagnostics;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public int dayLimit = 5;
    public int DayLengthInSeconds = 60;
    public TimeSpan dayDuration; // Duration of a day in seconds
    public int CurrentDay = 1;
    private Stopwatch dayTimer = new Stopwatch();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        dayDuration = TimeSpan.FromSeconds(DayLengthInSeconds);
    }

    public void Update()
    {
        if (!dayTimer.IsRunning)
        {
            return;
        }

        TimeSpan remainingTime = dayDuration - dayTimer.Elapsed;
        UiManager.Instance.SetTimerText(remainingTime.ToString(@"mm\:ss"));

        if (dayTimer.Elapsed >= dayDuration)
        {
            EndDay();
        }
    }

    private void EndDay()
    {
        dayTimer.Stop();
        CurrentDay++;
        if (CurrentDay > dayLimit)
        {
            GameManager.Instance.GoToGameFinished();
        }
        else
        {
            GameManager.Instance.GoToShop();
        }
    }

    public void StartDay()
    {
        dayTimer.Restart();
    }
}
