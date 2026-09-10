using UnityEngine;

public class BaloonSceneManager : MonoBehaviour
{
    
    private Material material;
    
    public float rotSpeed = 1f;

    void Awake()
    {
        material = this.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;
        material.SetFloat("_X", 1f);
        material.SetFloat("_Y", 1f);
        material.SetFloat("_Z", 1f);
    }
    void Update()
    {
        this.transform.GetChild(0).GetChild(1).Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        this.transform.GetChild(0).GetChild(2).Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
    }
}
