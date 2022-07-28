using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TimerManager : MonoBehaviour
{
    protected static TimerManager i;
    public event Action<float> PlantsGrowTimeEvent;
    public static TimerManager Instance
    {
        get
        {
            if (i != null)
                return i;

            i = FindObjectOfType<TimerManager>();
            if (i != null)
                return i;

            GameObject tm = new GameObject("TimerManager");
            i = tm.AddComponent<TimerManager>();
            return i;
        }
    }

    public float plantGrowSpeed { get; set; }

    DateTime oldtime;
    TimeSpan passedTime;
    bool gameStarted;
    public UnityEngine.UI.Text Text;
    private void Awake()
    {
        passedTime = CalculatePassedTime();
        Debug.Log(GetPassedTimeInFloat());
    }

    private void OnApplicationQuit()
    {
        //If game started or game has played for couple second
        //Do the following line
        Debug.Log(DateTime.UtcNow.ToString());
        PlayerPrefs.SetString("LastOnline", DateTime.UtcNow.ToString());
        //Otherwise, LastOnline timer will be erased;
    }

    //Calculate Passed Time since last offline
    TimeSpan CalculatePassedTime()
    {
        TimeSpan passedTime;
        if (PlayerPrefs.HasKey("LastOnline"))
        {
            string lastTime_s = PlayerPrefs.GetString("LastOnline");
            Debug.Log(lastTime_s);
            DateTime lastTime_d = Convert.ToDateTime(lastTime_s);
            passedTime = DateTime.UtcNow.Subtract(lastTime_d);
            Debug.Log(passedTime);
            Text.text = "Last Online : " + lastTime_s +
                "\nCurrent Time : " + DateTime.UtcNow.ToLongTimeString() +
                "\nTotal Time : " + String.Format("{0}H:{1}M:{2}S", passedTime.Hours, passedTime.Minutes, passedTime.Seconds);
        }
        return passedTime;
    }

    public float GetPassedTimeInFloat()
    {
        float result = 0;
        result += (float)passedTime.TotalSeconds;
        return result;
    }
    
    public void ChangePlantSpeed(float v)
    {
        PlantsGrowTimeEvent?.Invoke(v);
    }
}
