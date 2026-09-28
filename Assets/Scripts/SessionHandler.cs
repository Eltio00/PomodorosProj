using UnityEngine;
using UnityEngine.UI;
public class SessionHandler : MonoBehaviour
{
    [SerializeField] private GameObject pomodorosContainer;
    [SerializeField] private GameObject pomodorosHandler;
    [SerializeField] private Sprite pomodoroSprite;
    
    private static int pomodorosCounter = 0;
    
    [SerializeField] private GameObject[] timerBtns;
    [SerializeField] private GameObject newSessionBtn;
    [SerializeField] private GameObject handlingPopUp;
    
    
    private void Start()
    {
        AddPomodoros();
        handlingPopUp.gameObject.SetActive(false);   
    }
    void Update()
    {
        if (TimerScript.IsPomoFinished())
        {
            RemovePomodoros();
            TimerScript.SetIsPomoFinished(false);
        }
    }

    public void NewSession()
    {
        newSessionBtn.SetActive(false);
        handlingPopUp.gameObject.SetActive(true);
    }

    public void AddPomodoros()
    {
        if (pomodorosCounter < 5)
        {
            ++pomodorosCounter;
            GameObject newImage = new GameObject($"PomodoroImage_{pomodorosCounter}", typeof(Image));
            newImage.transform.SetParent(pomodorosContainer.transform, false);

            Image img = newImage.GetComponent<Image>();
            img.sprite = pomodoroSprite;

            RectTransform rt = newImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(60, 60);
        }
    }

    public void RemovePomodoros()
    {
        if (pomodorosCounter > 1)
        {
            Destroy(pomodorosContainer.transform.GetChild(pomodorosCounter-1).gameObject);
            --pomodorosCounter;
        }

    }

    public static int GetPomodorosCounter() { return pomodorosCounter; }
    public void Quit()
    {
        Application.Quit();
    }
    public void SetPomodorosCounter(int count)
    {
        pomodorosCounter = count;
    }
    public void ActivateHandlingButtons()
    {
        foreach (GameObject btn in timerBtns)  
            btn.SetActive(true);
    }
    public void QuitPopup(){ newSessionBtn.SetActive(true);}
}
