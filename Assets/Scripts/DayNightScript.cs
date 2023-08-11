using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//This script controlls the Day/Night cycle
public class DayNightScript : MonoBehaviour
{
    [Header("Time Varibles")]
    //[Tooltip("How long you want the village loop to last (Hours)")]
    //public float TimeTarget;
    [Tooltip("What hour the day starts at")]
    public float dayStartTime;
    public bool TimerOn = false;
    private bool isPm = false;
    private float TimeElapsed;      // 1 hour is 1 minute in real life
    private float previousHour;
    private static float hour;
    private static float day;

    [Header("Links")]
    public TextMeshProUGUI TimerTxt;
    public static DayNightScript current;

    [Header("DEBUG VARIBLES")]
    public float timeScale;


    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        //TimeTarget *= 60;
        TimerOn = true;
        TimeElapsed = dayStartTime * 60;
        day = 1;
        Time.timeScale = timeScale;
    }

    void Update()
    {
        // Checks if the TimeElapsed has passed the TimeTarget and stops the timer if it has
        if (TimerOn)
        {
            TimeElapsed += Time.deltaTime;
            updateTimer(TimeElapsed);
        }
    }

    // Updates the timer text
    void updateTimer(float currentTime)
    {
        currentTime += 1;

        hour = Mathf.FloorToInt(currentTime / 60);

        if (hour != previousHour)
        {
            previousHour = hour;
            newHour();
        }

        if (hour > 23)
        {
            hour = hour % 24;                                   // Resets clock if it is past midnight and changes day
            TimeElapsed = 0;
            day++;
            isPm = false;                                       // Changes time to AM
        }

        if (hour > 11 && !isPm)                                 // Changes time to PM if needed
            isPm = true;

        if (!isPm)
        {
            if (hour == 0)
                TimerTxt.text = "12 AM";
            else
                TimerTxt.text = string.Format("{0} AM", hour);
        }
        else
        {
            if (hour == 12)
                TimerTxt.text = "12 PM";
            else
                TimerTxt.text = string.Format("{0} PM", hour - 12);
        }   
    }

    // Returns the hour it currently is with 0 being midnight and 12 being noon (Ex: 3pm returns 15)
    public static float getHour()
    {
        return hour;
    }

    // Returns what day the game is on (first day is day 1)
    public static float getDay()
    {
        return day;
    }

    public event Action onNewHour;
    public void newHour()
    {
        if(onNewHour != null)
        {
            onNewHour();
        }
    }
    

}