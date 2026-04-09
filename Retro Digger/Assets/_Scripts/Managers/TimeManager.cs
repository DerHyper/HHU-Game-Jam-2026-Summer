using UnityEngine;
using System.Diagnostics;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public int dayLimit = 5;
    public TimeSpan dayDuration = TimeSpan.FromSeconds(60f); // Duration of a day in seconds
    public int CurrentDay { get; private set; } = 1;
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

    public void Update()
    {
        if (!dayTimer.IsRunning)
        {
            return;
        }

        if (dayTimer.Elapsed >= dayDuration)
        {
            EndDay();
        }
    }

    private void EndDay()
    {
        dayTimer.Stop();
        CurrentDay++;
    }

    public void StartDay()
    {
        dayTimer.Restart();
    }
}
