using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    private TMP_Text messageText;

    void Awake()
    {
        messageText = GetComponent<TMP_Text>();
    }

    public void Setup(string text, bool isUser)
    {
        messageText.text = text;
        messageText.alignment = isUser ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft;
    }

    public void AppendText(string chunk)
    {
        messageText.text += chunk;
    }
}