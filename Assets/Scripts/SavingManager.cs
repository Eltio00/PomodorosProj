using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ChatMessageData
{
    public string role;
    public string content;
}

[System.Serializable]
public class ToDos {
    public string toDosText;
    public bool isCompleted;
}

[System.Serializable]
public class Data
{
    public string sceneName;
    public List<ToDos> toDos;
    public float time;
    public int pomodoros;
    public List<ChatMessageData> poikiConversation = new List<ChatMessageData>();
}
public class SavingManager : MonoBehaviour
{
    public void SaveCurrentState(string currentSceneName, List<ToDos> currentToDos, float currentTime, int currentPomodoros, List<ChatMessageData> currentSessionMessages)
    {
        Data savingData = new Data();
        savingData.sceneName = currentSceneName;
        savingData.toDos = currentToDos;
        savingData.time = currentTime;
        savingData.pomodoros = currentPomodoros;
        string json = JsonUtility.ToJson(savingData);

        string filePath = Path.Combine(Application.persistentDataPath, "pomodoro_save.json");
        try
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            File.WriteAllText(filePath, json);
            SavePoikiConversation(currentSessionMessages);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Errore salvataggio: {ex}");
        }

    }
    public string GetSceneName() {
        try
        {
            return JsonUtility.FromJson<Data>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "pomodoro_save.json"))).sceneName;
        }
        catch(System.Exception ex)
        {
            return ex.ToString();
        }
    }
    public List<ToDos> GetToDos() {
        try
        {
            return JsonUtility.FromJson<Data>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "pomodoro_save.json"))).toDos;
        }
        catch (System.Exception ex)
        {

            return null;
        }
    }
    public float GetTime() {
        try
        {
            return JsonUtility.FromJson<Data>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "pomodoro_save.json"))).time;
        }
        catch (System.Exception ex)
        {

            return -1;
        }
    }
    public int GetPomodoros() {
        try
        {
            return JsonUtility.FromJson<Data>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "pomodoro_save.json"))).pomodoros;
        }
        catch (System.Exception ex)
        {

            return -1;
        }
    }
    public void SavePoikiConversation(List<ChatMessageData> conversation)
    {
        Data savingData = LoadFullData();
        savingData.poikiConversation = conversation;

        string json = JsonUtility.ToJson(savingData);
        string filePath = Path.Combine(Application.persistentDataPath, "pomodoro_save.json");

        try
        {
            File.WriteAllText(filePath, json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Conversation saving error: {ex}");
        }
    }

    public List<ChatMessageData> GetPoikiConversation()
    {
        try
        {
            Data data = JsonUtility.FromJson<Data>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "pomodoro_save.json")));
            return data.poikiConversation ?? new List<ChatMessageData>();
        }
        catch (System.Exception ex)
        {
            return new List<ChatMessageData>();
        }
    }

    private Data LoadFullData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "pomodoro_save.json");
        if (!File.Exists(filePath))
            return new Data();

        try
        {
            return JsonUtility.FromJson<Data>(File.ReadAllText(filePath));
        }
        catch
        {
            return new Data();
        }
    }
}
