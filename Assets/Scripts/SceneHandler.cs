using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] scenes;
    private int currentSceneIndex = 0;
    [SerializeField] private Light sceneLight;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ForwardScene()
    {
        ++currentSceneIndex;
        ChangeScene();
    }
    public void BackwardScene()
    {
        --currentSceneIndex;
        ChangeScene();
    }

    private int ChangeScene()
    {
        if (currentSceneIndex > 3)
            currentSceneIndex = 0;
        if (currentSceneIndex < 0)
            currentSceneIndex = 3;
        switch (currentSceneIndex)
        {
            case 0:
                OpenBaloonScene();
                return 0;
            
            case 1:
                OpenSpaceScene();
                return 1;
            
            case 2:
                OpenMachineHouseScene();
                return 2;
            
            case 3:
                OpenAndromedaKatosScene();
                return 3;
            default: 
                return 0;
        }
    }

    private void OpenBaloonScene()
    {
        scenes[0].SetActive(true);
        scenes[1].SetActive(false);
        scenes[2].SetActive(false);
        scenes[3].SetActive(false);
        SetLight(new Vector3(0f, 0f, 0f), 10032f, 3.5f, new Color(1f, 0.808f, 0.620f));
    }
    private void OpenSpaceScene()
    {
        scenes[0].SetActive(false);
        scenes[1].SetActive(true);
        scenes[2].SetActive(false);
        scenes[3].SetActive(false);
        //SetLight(new Vector3(-10f, 260f, 0f), 1500f, 2f, new Color(1f, 0.816f, 0.816f));
        SetLight(new Vector3(0f, 0f, 0f), 10032f, 2.2f, new Color(1f, 0.808f, 0.620f));
    }
    private void OpenMachineHouseScene()
    {
        scenes[0].SetActive(false);
        scenes[1].SetActive(false);
        scenes[2].SetActive(true);
        scenes[3].SetActive(false);
        //SetLight(new Vector3(445f, -187f, 0f), 4000f, 5f, new Color(0.306f, 0.914f, 0.443f));
        SetLight(new Vector3(0f, 0f, 0f), 10032f, 2f, new Color(1f, 0.808f, 0.620f));
    }
    private void OpenAndromedaKatosScene()
    {
        scenes[0].SetActive(false);
        scenes[1].SetActive(false);
        scenes[2].SetActive(false);
        scenes[3].SetActive(true);
        SetLight(new Vector3(-63f, -18.3f, 0f), 8574f, 3f, new Color(0.706f, 0.918f, 0.894f));
    }

    private void SetLight(Vector3 eulerRotation, float temperature, float intensity, Color color)
    {
        sceneLight.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(eulerRotation));
        sceneLight.colorTemperature = temperature;
        sceneLight.intensity = intensity;
        sceneLight.color = color;
    }
}
