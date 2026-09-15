using UnityEngine;

public class AndromedaKatosSceneManager : MonoBehaviour
{
    private Material material;
    public GameObject bubble;
    public GameObject bubbleInside;

    void Awake()
    {
        StartMaterialMovement(0);
        StartMaterialMovement(3);
        StartMaterialMovement(4);
        StartMaterialMovement(6);
    }

    private Vector3 bubbleStartPos;
    private Vector3 bubbleInsideStartPos;
    public float amplitude = 0.3f;
    public float speed = 2f;

    void Start()
    {
        bubbleStartPos = bubble.transform.position;
        bubbleInsideStartPos = bubbleInside.transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;

        bubble.transform.position = bubbleStartPos + new Vector3(0f, offset, 0f);
        bubbleInside.transform.position = bubbleInsideStartPos + new Vector3(0f, offset, 0f);
    }
    private void StartMaterialMovement(int i)
    {
        Debug.Log(this.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().material);
        material = this.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().material;
        material.SetFloat("_X", 0f);
        material.SetFloat("_Y", 0f);
        material.SetFloat("_Z", 1f);
    }
}
