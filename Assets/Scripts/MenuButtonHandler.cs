using System.Collections.Generic;
using UnityEngine;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] private SavingManager savingManager;
    [SerializeField] private PoikiManager poikiManager;
    [SerializeField] private TimerScript pomodoroTimer;
    [SerializeField] private SessionHandler sessionHandler;
    [SerializeField] private ToDosScript toDosHandler;
    [SerializeField] private SceneHandler sceneHandler;
    
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject menuPanel;

    public void SaveSession()
    {
        string currentScene = sceneHandler.GetCurrentScene().name;
        List<ToDos> currentToDos = toDosHandler.GetCurrentToDos();
        float currentTime = pomodoroTimer.GetCurrentTime();
        float currentSeconds = pomodoroTimer.GetPauseTimer();
        int currentPomodoros = SessionHandler.GetPomodorosCounter();
        List<ChatMessageData> currentConversation = poikiManager.GetConversationHistory();

        savingManager.SaveCurrentState(currentScene, currentToDos, currentTime, currentSeconds, currentPomodoros, currentConversation);

        Debug.Log("Game saved successfully.");
    }
    
    public void LoadSession()
    {
        string savedScene = savingManager.GetSceneName();

        if (string.IsNullOrEmpty(savedScene) || savedScene != Constants.BALOON_SCENE)
        {
            sceneHandler.SetScene(savedScene);
            return;
        }
        sessionHandler.OpenFromLoad();
        RestoreSessionData();
    }

    private void RestoreSessionData()
    {
        List<ToDos> savedToDos = savingManager.GetToDos();
        float savedTime = savingManager.GetTime();
        float savedPause = savingManager.GetPause();
        int savedPomodoros = savingManager.GetPomodoros();
        List<ChatMessageData> savedConversation = savingManager.GetPoikiConversation();

        if (savedToDos != null)
            toDosHandler.LoadToDos(savedToDos);

        if (savedTime >= 0)
            pomodoroTimer.SetCurrentTime(savedTime);
        else
            pomodoroTimer.SetCurrentTime(5f);

        if (savedPause >= 0)
            pomodoroTimer.SetPauseTime(savedPause);
        else
            pomodoroTimer.SetPauseTime(5f);
        
        
        if (savedPomodoros >= 0)
            sessionHandler.SetPomodorosCounter(savedPomodoros);

        poikiManager.RestoreConversationHistory(savedConversation);

        Debug.Log("Session restored successfully.");
    }

    public void OpenMenu()
    {
        menuButton.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        menuButton.SetActive(true);
    }
}