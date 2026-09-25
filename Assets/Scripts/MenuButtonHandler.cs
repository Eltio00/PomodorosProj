using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] private SavingManager savingManager;
    [SerializeField] private PoikiManager poikiManager;
    [SerializeField] private TimerScript pomodoroTimer;
    [SerializeField] private SessionHandler sessionHandler;
    [SerializeField] private ToDosScript toDosHandler;

    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject menuPanel;

    [Header("Todo Handling")]
    [SerializeField] private GameObject toDoContainer;
    [SerializeField] private Sprite toDosSpriteCrouch;
    [SerializeField] private Sprite toDosSprite;
    [SerializeField] private GameObject toDosButtons;

    private GameObject dropDownOpen;
    private GameObject dropDownClose;
    private GameObject toDosPanel;
    
    void Start()
    {
        dropDownOpen = GameObject.Find(Constants.DROPDOWN_BTN_OPEN);
        dropDownClose = GameObject.Find(Constants.DROPDOWN_BTN_CLOSE);
        toDosPanel = GameObject.Find(Constants.TODO_SCROLL);
        Debug.Log($"Open: {dropDownOpen.name}; Close: {dropDownClose.name}; Panel: {toDosPanel.name}; Buttons: {toDosButtons.name}");
    }

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
    public void OpenToDos()
    {
        RectTransform rect = toDoContainer.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200f);
        //Image img = toDoContainer.GetComponent<Image>();
        //img.sprite = toDosSpriteCrouch;
        dropDownOpen.SetActive(false);
        dropDownClose.SetActive(true);
        toDosPanel.SetActive(true);
        toDosButtons.SetActive(true);
    }
    public void CloseToDos()
    {
        RectTransform rect = toDoContainer.GetComponent<RectTransform>();
        //Image img = toDoContainer.GetComponent<Image>();
        //img.sprite = toDosSprite;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 780f);
        dropDownOpen.SetActive(true);
        dropDownClose.SetActive(false);
        toDosPanel.SetActive(false);
        toDosButtons.SetActive(false);
    }
}