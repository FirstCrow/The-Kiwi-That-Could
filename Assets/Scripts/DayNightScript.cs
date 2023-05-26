using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightScript : MonoBehaviour
{
    private float TimeElapsed;      // 1 hour is 1 minute in real life
    public float TimeTarget;        // How long you want the day night cycle to be
    public float dayStartTime;      // Hour that the day starts on
    public bool TimerOn = false;
    private bool isPm = false;

    public TextMeshProUGUI TimerTxt;

    void Start()
    {
        TimerOn = true;
        TimeElapsed = 0;
    }

    void Update()
    {
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

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);

        minutes += dayStartTime;

        if (minutes > 12)
            isPm = true;

        if(!isPm)
            TimerTxt.text = string.Format("{0} AM", minutes);
        else
            TimerTxt.text = string.Format("{0} PM", minutes - 12);
    }

}