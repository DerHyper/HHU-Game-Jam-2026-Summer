using UnityEngine;
using System.Diagnostics;
using System;

public class TimeManager : MonoBehaviour
{
    public int DayLengthInSeconds = 60;
    public TimeSpan dayDuration; // Duration of a day in seconds


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
        Instance.ChangeTime(remainingTime.ToString(@"mm\:ss"));

        if (GlobalTimer.dayTimer.Elapsed >= dayDuration)
        {
            EndDay();
        }
    }

    private void EndDay()
    {
        GlobalTimer.dayTimer.Stop();
        Instance.CurrentDay++;
        if (Instance.CurrentDay > Instance.dayLimit)
        {
            GameManager.Instance.GoToGameFinished();
        }
        else
        {
            GameManager.Instance.GoToShop();
        }
    }



    public static class Instance
    {
        public static int dayLimit = 5;
        public static int CurrentDay = 1;

        public static event Action<string> TimeChanged;

        public static void ChangeTime(string newTime)
        {
            TimeChanged?.Invoke(newTime);
        }

        public static void StartDay()
        {
            GlobalTimer.dayTimer.Restart();
        }

        public static bool IsLastDay() => Instance.CurrentDay >= Instance.dayLimit;
    }
}

public static class GlobalTimer
{
    public static readonly Stopwatch dayTimer = new();
}