using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] scenes;
    private int currentSceneIndex = 0;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ForwardScene()
    {
        ChangeScene(++currentSceneIndex);
    }
    public void BackwardScene()
    {
        ChangeScene(--currentSceneIndex);
    }

    private int ChangeScene(int i)
    {
        if (currentSceneIndex >= 3)
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
        
    }
    private void OpenSpaceScene()
    {
        scenes[0].SetActive(false);
        scenes[1].SetActive(true);
        scenes[2].SetActive(false);
        scenes[3].SetActive(false);
    }
    private void OpenMachineHouseScene()
    {
        scenes[0].SetActive(false);
        scenes[1].SetActive(false);
        scenes[2].SetActive(true);
        scenes[3].SetActive(false);
    }
    private void OpenAndromedaKatosScene()
    {
        scenes[0].SetActive(false);
        scenes[1].SetActive(false);
        scenes[2].SetActive(false);
        scenes[3].SetActive(true);
    }
}
