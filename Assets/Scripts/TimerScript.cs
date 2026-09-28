using System.Collections;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI pauseTimerText;
    [SerializeField] private TextMeshProUGUI studyTmerText;
    
    [SerializeField] private float initialTimer = 0;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private GameObject timerButtons;
    [SerializeField] private float pauseTimer = 0f;
    [SerializeField] private GameObject pomodorosHandler;
    [SerializeField] private float transitionDelay = 1.5f;

    private float startInitTimer = 0f;
    private float startPauseTimer = 0f;

    private bool runningTimer = false;
    private bool state = true;
    private bool isTransitioning = false;
    private static bool isPomoFinished = false;

    private GameObject popup;
    [SerializeField] private SessionHandler _sessionHandler;
    void Start()
    {
        startInitTimer = initialTimer;
        startPauseTimer = pauseTimer;
        popup = GameObject.Find(Constants.POPUP);
    }

    void Update()
    {
        if (popup != null && popup.gameObject.activeSelf)
        {
            int minutes = 0;
            int seconds = 0;
            // Setting initial timer
            minutes = Mathf.FloorToInt(initialTimer / 60);
            seconds = Mathf.FloorToInt(initialTimer % 60);
            studyTmerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            // Setting pause timer
            minutes = Mathf.FloorToInt(pauseTimer / 60);
            seconds = Mathf.FloorToInt(pauseTimer % 60);
            pauseTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            return;
        }
        
        if (!isTransitioning && !popup.gameObject.activeSelf)
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
        arrows[0].gameObject.SetActive(false);
        arrows[1].gameObject.SetActive(false);
        pomodorosHandler.SetActive(false);
    }

    public void PauseTimer()
    {
        runningTimer = false;
        arrows[0].gameObject.SetActive(true);
        arrows[1].gameObject.SetActive(true);
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
        arrows[0].gameObject.SetActive(true);
        arrows[1].gameObject.SetActive(true);
    }

    public void ChooseTimer()
    {
        if (runningTimer)
            return;

        arrows[0].gameObject.SetActive(!arrows[0].gameObject.activeSelf);
        arrows[1].gameObject.SetActive(!arrows[1].gameObject.activeSelf);
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
    
    public void DecreseStudyTime(float time)
    {
        initialTimer -= time;
        if (initialTimer < 0) initialTimer = 0;
    }

    public void IncreseStudyTimer(float time)
    {
        initialTimer += time;
    }
    public void DecresePauseTime(float time)
    {
        pauseTimer -= time;
        if (pauseTimer < 0) pauseTimer = 0;
    }

    public void IncresePauseTimer(float time)
    {
        pauseTimer += time;
    }

    public static bool IsPomoFinished() { return isPomoFinished; }
    public static void SetIsPomoFinished(bool finished) { isPomoFinished = finished; }
    public float GetCurrentTime() { return initialTimer; }
    public void SetCurrentTime(float time)
    {
        initialTimer = time;
    }

    public void Apply()
    {
        _sessionHandler.ActivateHandlingButtons();
        startInitTimer = initialTimer;
        startPauseTimer = pauseTimer;
        popup.SetActive(false);
    }

    public void Cancel()
    {
        popup.SetActive(false);
        _sessionHandler.QuitPopup();
    }
}