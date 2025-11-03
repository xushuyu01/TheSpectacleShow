using UnityEngine;
using System.Collections.Generic;

public class EnvGen : MonoBehaviour
{
    [Header("Box Prefabs")]
    public GameObject boxPrefab1;
    public GameObject boxPrefab2;

    [Header("Circle Settings")]
    public int numberOfObjectsPerLayer = 20;
    public float radius = 5f;
    public int numberOfLayers = 3;
    public float layerHeight = 2f;

    // 用来保存生成的 Box
    private List<List<GameObject>> layers = new List<List<GameObject>>();

    void Start()
    {
        GenerateCylinderArray();

        // 删除特定盒子里面的人
        DeletePersonInBox(2, 4); // 第三层第1个（索引从0开始）
        DeleteEntireBox(0, 0); // 第一层第4个
    }

    void GenerateCylinderArray()
    {
        for (int layer = 0; layer < numberOfLayers; layer++)
        {
            float y = layer * layerHeight;
            List<GameObject> currentLayer = new List<GameObject>();

            for (int i = 0; i < numberOfObjectsPerLayer; i++)
            {
                GameObject prefab = Random.value > 0.5f ? boxPrefab1 : boxPrefab2;

                float angle = i * Mathf.PI * 2f / numberOfObjectsPerLayer;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                Vector3 pos = new Vector3(x, y, z) + transform.position;
                Vector3 directionToAxis = new Vector3(transform.position.x - pos.x, 0, transform.position.z - pos.z);
                Quaternion rotation = Quaternion.LookRotation(directionToAxis) * Quaternion.Euler(0, 90f, 0);

                GameObject box = Instantiate(prefab, pos, rotation, transform);
                currentLayer.Add(box);
            }

            layers.Add(currentLayer);
        }
    }

    // 删除指定层、指定索引盒子里的子物体（比如人）
    void DeletePersonInBox(int layerIndex, int boxIndex)
    {
        if (layerIndex < 0 || layerIndex >= layers.Count) return;
        if (boxIndex < 0 || boxIndex >= layers[layerIndex].Count) return;

        GameObject box = layers[layerIndex][boxIndex];

        // 查找 FemaleBody 和 MaleBody 并删除
        Transform female = box.transform.Find("FemaleBody");
        if (female != null) Destroy(female.gameObject);

        Transform male = box.transform.Find("MaleBody");
        if (male != null) Destroy(male.gameObject);
    }
    // 删除整个盒子（包含里面的子物体）
    void DeleteEntireBox(int layerIndex, int boxIndex)
    {
        if (layerIndex < 0 || layerIndex >= layers.Count) return;
        if (boxIndex < 0 || boxIndex >= layers[layerIndex].Count) return;

        GameObject box = layers[layerIndex][boxIndex];

        // 删除后从列表里移除引用，防止后续操作报错
        layers[layerIndex].RemoveAt(boxIndex);

        Destroy(box);
    }
}
