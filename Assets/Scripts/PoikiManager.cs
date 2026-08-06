using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using TMPro;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class PoikiManager : MonoBehaviour
{
    [SerializeField] public RAG rag;
    [SerializeField] public LLMAgent llmAgent;

    [SerializeField] private string text;

    public bool IsModelReady = false;
    public bool IsReplyDone = true;

    private const string ChatHistoryFile = "poiki_chat_history.json";
    private const string RagDataFile = "poiki_notes_rag.zip";
    private const string NotesIndexFile = "poiki_notes_index.json";

    private Dictionary<string, HashSet<string>> notesIndex = new Dictionary<string, HashSet<string>>();

    async void Start()
    {
        // set the save path BEFORE calling SaveHistory/LoadHistory
        llmAgent.save = ChatHistoryFile;

        await LLM.WaitUntilModelSetup();

        await LoadPersistedData();
        LoadNotesIndex();

        Debug.Log("Poiki is ready: chat + embedding.");
        IsModelReady = true;
    }

    async System.Threading.Tasks.Task LoadPersistedData()
    {
        try
        {
            await llmAgent.LoadHistory();
            Debug.Log("Chat history loaded.");
        }
        catch (System.Exception e)
        {
            Debug.Log("No previous chat history found: " + e.Message);
        }

        try
        {
            await rag.Load(RagDataFile);
            Debug.Log("RAG notes loaded.");
        }
        catch (System.Exception e)
        {
            Debug.Log("No previous RAG notes found: " + e.Message);
        }
    }

    public void SavePersistedData()
    {
        _ = llmAgent.SaveHistory();
        rag.Save(RagDataFile);
        SaveNotesIndex();
        Debug.Log("Chat history, RAG notes and index saved.");
    }

    // --- Notes index management (to avoid duplicates) ---

    string ComputeNoteHash(string noteText)
    {
        using (var md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(noteText.Trim()));
            return System.Convert.ToBase64String(hashBytes);
        }
    }

    bool NoteAlreadyExists(string noteText, string temaId)
    {
        string hash = ComputeNoteHash(noteText);
        return notesIndex.TryGetValue(temaId, out HashSet<string> hashesForTema)
               && hashesForTema.Contains(hash);
    }

    void RegisterNote(string noteText, string temaId)
    {
        string hash = ComputeNoteHash(noteText);
        if (!notesIndex.ContainsKey(temaId))
            notesIndex[temaId] = new HashSet<string>();
        notesIndex[temaId].Add(hash);
    }

    [System.Serializable]
    private class NotesIndexEntry { public string temaId; public List<string> hashes; }

    [System.Serializable]
    private class NotesIndexWrapper { public List<NotesIndexEntry> entries; }

    void SaveNotesIndex()
    {
        var wrapper = new NotesIndexWrapper { entries = new List<NotesIndexEntry>() };
        foreach (var kvp in notesIndex)
            wrapper.entries.Add(new NotesIndexEntry { temaId = kvp.Key, hashes = kvp.Value.ToList() });

        string json = JsonUtility.ToJson(wrapper);
        string path = Path.Combine(Application.persistentDataPath, NotesIndexFile);
        File.WriteAllText(path, json);
    }

    void LoadNotesIndex()
    {
        string path = Path.Combine(Application.persistentDataPath, NotesIndexFile);
        if (!File.Exists(path))
        {
            Debug.Log("No previous notes index found, starting empty.");
            return;
        }

        string json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<NotesIndexWrapper>(json);
        notesIndex = new Dictionary<string, HashSet<string>>();
        foreach (var entry in wrapper.entries)
            notesIndex[entry.temaId] = new HashSet<string>(entry.hashes);

        Debug.Log("Notes index loaded.");
    }

    // --- Notes and chat ---

    public async void SaveNote(string noteText, string temaId)
    {
        if (NoteAlreadyExists(noteText, temaId))
        {
            Debug.Log($"Note already present in topic '{temaId}', skipping insertion.");
            return;
        }

        await rag.Add(noteText, temaId);
        RegisterNote(noteText, temaId);
        Debug.Log($"Poiki has added the note to the topic '{temaId}'.");

        rag.Save(RagDataFile);
        SaveNotesIndex();
    }

    public async void AskTheModel(string userQuery, string temaId)
    {
        rag.ReturnChunks(true);
        (string[] chunkFound, float[] distances) = await rag.Search(userQuery, 4, temaId);

        string context = "Poiki has found some relevant notes:\n";
        IsReplyDone = false;
        foreach (string chunk in chunkFound)
            context += $"- {chunk}\n";

        string finalPrompt = $"{context}\nUser Query: {userQuery}";

        _ = llmAgent.Chat(finalPrompt, OnStreamingReply, OnReplyDone);
    }

    void OnStreamingReply(string partialText)
    {
        text = partialText;
        Debug.Log("... Poiki is answering: " + partialText);
    }

    void OnReplyDone()
    {
        IsReplyDone = true;
        PomodoroChatController.SetCurrentText(text.ToList());
        Debug.Log("Poiki answered.");

        _ = llmAgent.SaveHistory();
    }

    void OnApplicationQuit()
    {
        SavePersistedData();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SavePersistedData();
    }
}