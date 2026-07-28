using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SessionHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject arrows;
    [SerializeField] private GameObject timerButtons;

    // Add it on another script
    [SerializeField] private GameObject toDos;
    [SerializeField] private GameObject pomodorosContainer;
    [SerializeField] private GameObject pomodorosHandler;
    [SerializeField] private GameObject session;

    //[SerializeField] private GameObject pomodoroImagePrefab;

    [SerializeField] private Sprite pomodoroSprite;
    private static int pomodorosCounter = 0;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewSession()
    {
        toDos.SetActive(true);
        pomodorosContainer.SetActive(true);
        pomodorosHandler.SetActive(true);
        timerButtons.SetActive(true);
        arrows.SetActive(true);
        timerText.gameObject.SetActive(true);
        session.SetActive(false);
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
        if (pomodorosCounter > 0)
        {
            Destroy(pomodorosContainer.transform.GetChild(pomodorosCounter-1).gameObject);
            --pomodorosCounter;
        }

    }

    public static int GetPomodorosCounter() { return pomodorosCounter; }
}
