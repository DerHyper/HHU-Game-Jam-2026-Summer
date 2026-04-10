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

    public event Action<string> TimeChanged;


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
        if (!GlobalTimer.dayTimer.IsRunning)
        {
            return;
        }

        TimeSpan remainingTime = dayDuration - GlobalTimer.dayTimer.Elapsed;
        TimeChanged?.Invoke(remainingTime.ToString(@"mm\:ss"));

        if (GlobalTimer.dayTimer.Elapsed >= dayDuration)
        {
            EndDay();
        }
    }

    private void EndDay()
    {
        GlobalTimer.dayTimer.Stop();
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
        GlobalTimer.dayTimer.Restart();
    }
}

public static class GlobalTimer
{
    public static readonly Stopwatch dayTimer = new();
}