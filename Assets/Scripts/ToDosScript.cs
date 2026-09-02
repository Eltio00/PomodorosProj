using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToDosScript : MonoBehaviour
{
    private List<GameObject> listItems = new List<GameObject>();
    private GameObject itemToDelete;
    [SerializeField] private GameObject defaultItem;
    [SerializeField] private Transform container;

    private Dictionary<GameObject, float> lastClickTimes = new Dictionary<GameObject, float>();
    private const float doubleClickThreshold = 0.5f;

    private List<ToDos> currentToDos;

    public void CheckItem(TMP_Text text, bool isOn)
    {
        ToDos matchingToDo = currentToDos.Find(t => t.toDosText == text.text);
        if (matchingToDo != null)
            matchingToDo.isCompleted = isOn;

        text.text = isOn ? $"<s>{text.text}</s>" : text.text.Replace("<s>", "").Replace("</s>", "");
    }

    // Registra (o ri-registra) il listener onDeselect per questo item
    private void SetupInputListener(GameObject item)
    {
        TMP_InputField inputField = item.transform.GetChild(2).GetComponent<TMP_InputField>();
        inputField.onDeselect.RemoveAllListeners();
        inputField.onDeselect.AddListener((newText) => OnInputEditFinished(item, newText));
    }

    // Passa alla modalità "modifica": mostra l'input, nascondi il testo, dai focus
    public void ChangeText(GameObject itemToChange)
    {
        Transform buttonText = itemToChange.transform.GetChild(1);
        TMP_InputField inputField = itemToChange.transform.GetChild(2).GetComponent<TMP_InputField>();

        inputField.text = buttonText.GetChild(0).GetComponent<TMP_Text>().text;

        buttonText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);

        inputField.Select();
        inputField.ActivateInputField();
    }

    // Quando l'input perde il focus: applica il testo, torna alla modalità "statica"
    private void OnInputEditFinished(GameObject itemToChange, string newText)
    {
        Transform buttonText = itemToChange.transform.GetChild(1);
        TMP_InputField inputField = itemToChange.transform.GetChild(2).GetComponent<TMP_InputField>();

        buttonText.GetChild(0).GetComponent<TMP_Text>().text = newText;
        buttonText.gameObject.SetActive(true);
        inputField.gameObject.SetActive(false);
    }

    public void AddToDo(string text)
    {
        GameObject newItem = Instantiate(defaultItem, container);

        Button newButton = newItem.transform.GetChild(1).GetComponent<Button>();
        newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        newButton.onClick.AddListener(() => ActionOnClick(newItem));

        Toggle generalToggle = newItem.transform.GetChild(0).GetComponent<Toggle>();
        TMP_Text label = newButton.transform.GetChild(0).GetComponent<TMP_Text>();
        generalToggle.onValueChanged.AddListener((isOn) => CheckItem(label, isOn));

        // Stato iniziale esplicito: input attivo, testo nascosto
        newItem.transform.GetChild(1).gameObject.SetActive(false);
        newItem.transform.GetChild(2).gameObject.SetActive(true);

        // Registra subito il listener e dai focus, così l'utente scrive da subito
        SetupInputListener(newItem);
        TMP_InputField inputField = newItem.transform.GetChild(2).GetComponent<TMP_InputField>();
        inputField.Select();
        inputField.ActivateInputField();

        listItems.Add(newItem);
        currentToDos.Add(new ToDos { toDosText = text, isCompleted = false });
        lastClickTimes[newItem] = -10f;
    }

    private void SetItemToDelete(GameObject item) { itemToDelete = item; }

    public void RemoveToDo()
    {
        if (listItems.Count == 0) return;
        listItems.Remove(itemToDelete);
        lastClickTimes.Remove(itemToDelete);
        Destroy(itemToDelete);
    }

    private void ActionOnClick(GameObject itemToChange)
    {
        SetItemToDelete(itemToChange);

        float lastTime = lastClickTimes.TryGetValue(itemToChange, out float t) ? t : -10f;
        float timeSinceLastClick = Time.time - lastTime;

        if (timeSinceLastClick <= doubleClickThreshold)
        {
            SetupInputListener(itemToChange); // ri-registra, per sicurezza
            ChangeText(itemToChange);
            lastClickTimes[itemToChange] = -10f;
        }
        else
        {
            lastClickTimes[itemToChange] = Time.time;
        }
    }
    public List<ToDos> GetCurrentToDos(){ return currentToDos; }
    public void LoadToDos(List<ToDos> savedToDos)
    {
        // Pulisci gli elementi UI esistenti, se presenti
        foreach (var item in listItems)
            Destroy(item);
        listItems.Clear();
        currentToDos.Clear();

        foreach (var todo in savedToDos)
        {
            AddToDo(todo.toDosText);
            currentToDos[currentToDos.Count - 1].isCompleted = todo.isCompleted;

            if (todo.isCompleted)
            {
                GameObject lastItem = listItems[listItems.Count - 1];
                Toggle toggle = lastItem.transform.GetChild(0).GetComponent<Toggle>();
                toggle.isOn = true;
            }
        }
    }
}