using System;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    void Start()
    {
        Debug.Log($"[Debug Text]: {messageText.name}");
    }

    public void Setup(string text, bool isUser)
    {

        Debug.Log($"[Text from {isUser}]: {text}");
        messageText.text = text;
        messageText.alignment = isUser ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft;
    }

    public void AppendText(string chunk)
    {
        messageText.text += chunk;
    }
}