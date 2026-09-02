using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] private SavingManager savingManager;
    [SerializeField] private PoikiManager poikiManager;
    [SerializeField] private TimerScript pomodoroTimer;
    [SerializeField] private SessionHandler sessionHandler;
    [SerializeField] private ToDosScript toDosHandler;

    public void SaveSession()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        List<ToDos> currentToDos = toDosHandler.GetCurrentToDos();
        float currentTime = pomodoroTimer.GetCurrentTime();
        int currentPomodoros = SessionHandler.GetPomodorosCounter();
        List<ChatMessageData> currentConversation = poikiManager.GetConversationHistory();

        savingManager.SaveCurrentState(currentScene, currentToDos, currentTime, currentPomodoros, currentConversation);

        Debug.Log("Game saved successfully.");
    }

    public void LoadSession()
    {
        string savedScene = savingManager.GetSceneName();

        if (string.IsNullOrEmpty(savedScene) || savedScene != SceneManager.GetActiveScene().name)
        {
            // Different (or no) scene saved: load it first.
            // Restoring the rest of the state needs to happen AFTER that
            // scene's objects exist, so it can't happen right here.
            SceneManager.LoadScene(savedScene);
            return;
        }

        RestoreSessionData();
    }

    private void RestoreSessionData()
    {
        List<ToDos> savedToDos = savingManager.GetToDos();
        float savedTime = savingManager.GetTime();
        int savedPomodoros = savingManager.GetPomodoros();
        List<ChatMessageData> savedConversation = savingManager.GetPoikiConversation();

        if (savedToDos != null)
            toDosHandler.LoadToDos(savedToDos);

        if (savedTime >= 0)
            pomodoroTimer.SetCurrentTime(savedTime);

        if (savedPomodoros >= 0)
            sessionHandler.SetPomodorosCounter(savedPomodoros);

        poikiManager.RestoreConversationHistory(savedConversation);

        Debug.Log("Session restored successfully.");
    }
}