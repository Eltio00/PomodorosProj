using System.Collections;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float initialTimer = 0;
    [SerializeField] private GameObject arrows;
    [SerializeField] private GameObject timerButtons;
    [SerializeField] private float pauseTimer = 0f;
    [SerializeField] private GameObject pomodorosHandler;
    [SerializeField] private float transitionDelay = 1.5f;

    //private int pomodors = 0;

    private float startInitTimer = 0f;
    private float startPauseTimer = 0f;

    private bool runningTimer = false;
    private bool state = true;
    private bool isTransitioning = false;
    private static bool isPomoFinished = false;

    void Start()
    {
        startInitTimer = initialTimer;
        startPauseTimer = pauseTimer;
    }

    void Update()
    {
        if (!isTransitioning)
        {
            float currentTimer = state ? initialTimer : pauseTimer;
            currentTimer = ExpireTimer(currentTimer);

            if (state)
                initialTimer = currentTimer;
            else
                pauseTimer = currentTimer;

            int minutes = Mathf.FloorToInt(currentTimer / 60);
            int seconds = Mathf.FloorToInt(currentTimer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            float currentTimer = state ? startPauseTimer : startInitTimer;

            int minutes = Mathf.FloorToInt(currentTimer / 60);
            int seconds = Mathf.FloorToInt(currentTimer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        }
    }

    private float ExpireTimer(float time)
    {
        if (runningTimer)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                if (time < 0) time = 0;
            }
            else
            {
                StartCoroutine(TransitionToNextState());
            }
        }
        return time;
    }

    private IEnumerator TransitionToNextState()
    {
        isTransitioning = true;
        yield return new WaitForSeconds(transitionDelay);

        if (state)
        {
            pauseTimer = startPauseTimer;
            state = false;
        }
        else
        {
            initialTimer = startInitTimer;
            state = true;
            isPomoFinished = true;

            if (SessionHandler.GetPomodorosCounter() <= 1)
                runningTimer = false;
        }

        isTransitioning = false;
    }

    public void StratTimer()
    {
        runningTimer = true;
        arrows.SetActive(false);
        pomodorosHandler.SetActive(false);
    }

    public void PauseTimer()
    {
        runningTimer = false;
        arrows.SetActive(true);
        pomodorosHandler.SetActive(true);
    }

    public void StopTimer()
    {
        initialTimer = startInitTimer;
        pauseTimer = startPauseTimer;
        runningTimer = false;
        state = true;
        isTransitioning = false;
        StopAllCoroutines();
        arrows.SetActive(true);
    }

    public void ChooseTimer()
    {
        if (!runningTimer)
            arrows.SetActive(!arrows.activeSelf);
    }

    public void DecreseTime(float time)
    {
        initialTimer -= time;
        if (initialTimer < 0) initialTimer = 0;
    }

    public void IncreseTimer(float time)
    {
        initialTimer += time;
    }

    public static bool IsPomoFinished() { return isPomoFinished; }
    public static void SetIsPomoFinished(bool finished) { isPomoFinished = finished; }
    public float GetCurrentTime() { return initialTimer; }
    public void SetCurrentTime(float time)
    {
        initialTimer = time;
    }
}