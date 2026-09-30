using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToDosScript : MonoBehaviour
{
    private List<GameObject> listItems = new List<GameObject>();
    [SerializeField] private GameObject defaultItem;
    [SerializeField] private Transform container;

    [Header("Checkmark sprites")]
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Sprite uncheckedSprite;

    [Header("Selection highlight")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = new Color(1f, 0.8f, 0.8f);

    private List<ToDos> currentToDos = new List<ToDos>();
    private GameObject itemToDelete;

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
    
    public void CheckItem(TMP_InputField inputField, bool isOn, ToDos toDo)
    {
        toDo.isCompleted = isOn;

        string plainText = inputField.text.Replace("<s>", "").Replace("</s>", "");
        inputField.text = isOn ? $"<s>{plainText}</s>" : plainText;
    }

    public void AddToDo(string text)
    {
        GameObject newItem = Instantiate(defaultItem, container);

        TMP_InputField todoInputField = newItem.transform.GetChild(0).GetComponent<TMP_InputField>();
        todoInputField.text = text;
        todoInputField.onEndEdit.AddListener((newText) => OnToDoTextEdited(newItem, newText));

        Toggle generalToggle = newItem.transform.GetChild(1).GetComponent<Toggle>();

        Transform checkmarkTransform = generalToggle.transform.Find("Background/checkmark");
        Image checkmarkImage = checkmarkTransform != null ? checkmarkTransform.GetComponent<Image>() : null;

        ToDos newToDo = new ToDos { toDosText = text, isCompleted = false };

        generalToggle.onValueChanged.AddListener((isOn) =>
        {
            CheckItem(todoInputField, isOn, newToDo);

            if (checkmarkImage != null)
                checkmarkImage.sprite = isOn ? checkedSprite : uncheckedSprite;
        });

        EventTrigger trigger = todoInputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((eventData) =>
        {
            PointerEventData pointerData = (PointerEventData)eventData;
            if (pointerData.clickCount >= 2)
                SetItemToDelete(newItem);
        });
        trigger.triggers.Add(entry);

        Image itemBackground = newItem.GetComponent<Image>();
        if (itemBackground != null)
            itemBackground.color = normalColor;
        else
            Debug.LogWarning("ToDoItem prefab has no Image component on its root — selection highlight won't be visible.");

        listItems.Add(newItem);
        currentToDos.Add(newToDo);
    }

    private void OnToDoTextEdited(GameObject item, string newText)
    {
        int index = listItems.IndexOf(item);
        if (index >= 0)
            currentToDos[index].toDosText = newText;
    }

    private void SetItemToDelete(GameObject item)
    {
        if (itemToDelete != null)
        {
            Image prevBg = itemToDelete.GetComponent<Image>();
            if (prevBg != null) prevBg.color = normalColor;
        }

        itemToDelete = item;

        Image bg = item.GetComponent<Image>();
        if (bg != null) bg.color = selectedColor;

        Debug.Log($"Selected for deletion: {item.name}");
    }

    public void RemoveToDo()
    {
        if (itemToDelete == null) return;

        int index = listItems.IndexOf(itemToDelete);
        if (index >= 0)
        {
            currentToDos.RemoveAt(index);
            listItems.RemoveAt(index);
        }

        Destroy(itemToDelete);
        itemToDelete = null;
    }

    public List<ToDos> GetCurrentToDos() { return currentToDos; }

    public void LoadToDos(List<ToDos> savedToDos)
    {
        foreach (var item in listItems)
            Destroy(item);
        listItems.Clear();
        currentToDos.Clear();
        itemToDelete = null;

        foreach (var todo in savedToDos)
        {
            AddToDo(todo.toDosText);
            currentToDos[currentToDos.Count - 1].isCompleted = todo.isCompleted;

            if (todo.isCompleted)
            {
                GameObject lastItem = listItems[listItems.Count - 1];
                Toggle toggle = lastItem.transform.GetChild(1).GetComponent<Toggle>();
                toggle.isOn = true;
            }
        }
    }
    
    public void OpenToDos()
    {
        RectTransform rect = toDoContainer.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 780f);
        Image img = toDoContainer.GetComponent<Image>(); 
        img.sprite = toDosSprite;
        dropDownOpen.SetActive(false);
        dropDownClose.SetActive(true);
        toDosPanel.SetActive(true);
        toDosButtons.SetActive(true);
    }
    public void CloseToDos()
    {
        
        RectTransform rect = toDoContainer.GetComponent<RectTransform>();
        Image img = toDoContainer.GetComponent<Image>();
        img.sprite = toDosSpriteCrouch;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 180f);
        dropDownOpen.SetActive(true);
        dropDownClose.SetActive(false);
        toDosPanel.SetActive(false);
        toDosButtons.SetActive(false);
    }
}