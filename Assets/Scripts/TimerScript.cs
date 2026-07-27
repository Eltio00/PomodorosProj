using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTimer = 0;
    [SerializeField] private GameObject arrows;
    [SerializeField] private GameObject timerButtons;

    // The pause and the time are expressed in milliseconds
    private int pomodors = 0;

    private bool runningTimer = false;

    private bool showArrows = false;
    void Update()
    {
        if (remainingTimer < 0f)
            remainingTimer = 0f;
        else if (remainingTimer > 5999f)
            remainingTimer = 5999f;

        if (runningTimer)
            if (remainingTimer > 0)
            {
                remainingTimer -= Time.deltaTime;
            }
            else if (remainingTimer < 0)
            {
                remainingTimer = pomodors;
                runningTimer = false;

            }
        int minutes = Mathf.FloorToInt(remainingTimer / 60);
        int seconds = Mathf.FloorToInt(remainingTimer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StratTimer()
    {
        runningTimer = true;
        arrows.SetActive(false);
    }
    public void PauseTimer()
    {
        runningTimer = false;
        arrows.SetActive(true);
    }
    public void StopTimer()
    {
        remainingTimer = 0;
        arrows.SetActive(true);
    }

    public void ChooseTimer()
    {
        if (!runningTimer)
            if (arrows.activeSelf)
               arrows.SetActive(false);
            else
                arrows.SetActive(true);
    }


    public void DecreseTime(float time)
    {
        remainingTimer -= time;
    }
    public void IncreseTimer(float time)
    {
        remainingTimer += time;
    }
}
