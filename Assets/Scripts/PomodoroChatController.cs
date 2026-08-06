using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class PomodoroChatController : MonoBehaviour
{
    [SerializeField] private PoikiManager poikiManager;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text answareText;
    [SerializeField] private Button sendButton;

    private string currentTeme = "Try Teme"; // in pratica lo imposti in base al tema scelto dall'utente
    private static List<char> currentText = new List<char>();


    void Update()
    {
        if (poikiManager.IsReplyDone && 
            currentText != null && 
            currentText.Count > 0)
        {
            answareText.text += currentText[0];
            currentText.RemoveAt(0);
        }
    }

    public void OnClickSend()
    {
        if (!poikiManager.IsModelReady) return;

        string question = inputField.text;
        if (string.IsNullOrWhiteSpace(question)) return;


        answareText.text = "";
        poikiManager.AskTheModel(question, currentTeme);
        inputField.text = "";
    }

    public static void SetCurrentText(List<char> text) { currentText = text; }

    
}
