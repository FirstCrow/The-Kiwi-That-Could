using TMPro;
using UnityEngine;
using UnityEngine.UI;

//This script controlls the timer
public class TimerScript : MonoBehaviour
{
    [Header("Time Varibles (Seconds)")]
    [Tooltip("How long the timer lasts in the resource run")]
    public float TimeLeft;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;

    void Start()
    {
        TimerOn = true;
    }

    void Update()
    {
        // Checks if time is up and stops the timer if it has
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time is UP!");
                TimeLeft = 0;
                TimerOn = false;
            }
        }
    }

    // Updates the timer text
    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}