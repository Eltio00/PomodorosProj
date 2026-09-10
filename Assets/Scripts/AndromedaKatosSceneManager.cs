using UnityEngine;

public class AndromedaKatosSceneManager : MonoBehaviour
{
    private Material material;

    void Awake()
    {
        StartMaterialMovement(0);
        StartMaterialMovement(3);
        StartMaterialMovement(4);
        StartMaterialMovement(6);
    }
    private void StartMaterialMovement(int i)
    {
        Debug.Log(this.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().material);
        material = this.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().material;
        material.SetFloat("_X", 1f);
        material.SetFloat("_Y", 1f);
        material.SetFloat("_Z", 0f);
    }
}
