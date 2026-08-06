using UnityEngine;
using LLMUnity;

public class ModelSelector : MonoBehaviour
{
    public LLM llm;

    void Awake()
    {
        llm.model = ScegliModelloPerPiattaforma();
    }

    string ScegliModelloPerPiattaforma()
    {
    #if UNITY_ANDROID || UNITY_IOS
        if (SystemInfo.systemMemorySize >= 8000)
            return "Qwen3.5-2B-Q4_K_M.gguf";
        else
            return "Qwen3.5-0.8B-Q4_K_M.gguf";
    #else
            return "Qwen3.5-9B-Q4_K_M.gguf";
    #endif
    }
}