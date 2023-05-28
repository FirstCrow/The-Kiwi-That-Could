using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//This script controlls the Day/Night cycle
public class DayNightScript : MonoBehaviour
{
    [Header("Time Varibles")]
    [Tooltip("How long you want the village loop to last (Hours)")]
    public float TimeTarget;
    [Tooltip("What hour the day starts at")]
    public float dayStartTime;
    public bool TimerOn = false;
    private bool isPm = false;
    private float TimeElapsed;      // 1 hour is 1 minute in real life

    [Header("Links")]
    public TextMeshProUGUI TimerTxt;

    void Start()
    {
        TimerOn = true;
        TimeTarget *= 60;
        TimeElapsed = 0;
    }

    void Update()
    {
        // Checks if the TimeElapsed has passed the TimeTarget and stops the timer if it has
        if (TimerOn)
        {
            if (TimeElapsed < TimeTarget)
            {
                TimeElapsed += Time.deltaTime;
                updateTimer(TimeElapsed);
            }
            else
            {
                Debug.Log("Time is UP!");
                TimeElapsed = 0;
                TimerOn = false;
            }
        }
    }

    // Updates the timer text
    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);

        minutes += dayStartTime;

        if (minutes > 11)
            isPm = true;

        if(!isPm)
            TimerTxt.text = string.Format("{0} AM", minutes);
        else
            TimerTxt.text = string.Format("{0} PM", minutes - 12);
    }

}