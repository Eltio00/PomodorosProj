using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using UglyToad.PdfPig;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PomodoroChatController : MonoBehaviour
{
    [SerializeField] private PoikiManager poikiManager;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text answareText;
    [SerializeField] private Button sendButton;

    private string currentTeme = "General Study";
    private static List<char> currentText = new List<char>();

    private string pdfFileType;
    private string txtFileType;
    private string docxFileType;
    private string mdFileType;


    void Start()
    {
        pdfFileType = NativeFilePicker.ConvertExtensionToFileType("pdf");
        txtFileType = NativeFilePicker.ConvertExtensionToFileType("txt");
        docxFileType = NativeFilePicker.ConvertExtensionToFileType("docx");
        mdFileType = NativeFilePicker.ConvertExtensionToFileType("md");
    }


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


    public void PickFile()
    {
        if (NativeFilePicker.IsFilePickerBusy())
            return;

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

        NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                return;

            if (poikiManager == null)
            {
                Debug.LogError("poikiManager not assigned in PomodoroChatController");
                return;
            }

            string fileContent;
            try
            {
                switch (path)
                {
                    case string p when p.EndsWith(".pdf", System.StringComparison.OrdinalIgnoreCase):
                        fileContent = FileConverter.ExtractFromPdf(path);
                        break;
                    case string p when p.EndsWith(".txt", System.StringComparison.OrdinalIgnoreCase):
                        fileContent = File.ReadAllText(path, Encoding.UTF8);
                        break;
                    case string p when p.EndsWith(".md", System.StringComparison.OrdinalIgnoreCase):
                        fileContent = File.ReadAllText(path, Encoding.UTF8);
                        break;
                    case string p when p.EndsWith(".docx", System.StringComparison.OrdinalIgnoreCase):
                        fileContent = FileConverter.ExtractFromDocx(path);
                        break;
                    default:
                        Debug.LogWarning($"Unrecognized file extension for path: {path}");
                        FileLogger.LogUnsupportedFormat(path);
                        ShowUserMessage("Unsupported file format.");
                        return;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to extract text from '{path}': {e.Message}");
                FileLogger.LogError(path, e.Message);
                ShowUserMessage("Could not read this file. It may be corrupted or unsupported.");
                return;
            }

            if (FileConverter.LooksLikeGarbage(fileContent))
            {
                Debug.LogWarning($"Extraction produced unreadable content for '{path}'.");
                FileLogger.LogGarbage(path, fileContent.Length);
                ShowUserMessage("This file couldn't be read properly — it may be a scanned document or use an unsupported font. Try a text-based file instead.");
                return;
            }

            Debug.Log($"File content extracted from {path} ({fileContent.Length} chars).");
            FileLogger.LogSuccess(path, fileContent.Length);
            poikiManager.SaveNote(fileContent, "General Study");

        }, new string[] { pdfFileType, txtFileType, docxFileType, mdFileType });
    }

    // placeholder: hook this to your actual UI feedback system
    void ShowUserMessage(string message)
    {
        Debug.Log($"[User message] {message}");
        // TODO: replace with a toast/popup in the actual UI
    }
}
