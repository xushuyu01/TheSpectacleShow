using UnityEngine;

public class EnvGen : MonoBehaviour
{
    [Header("Box Prefabs")]
    public GameObject boxPrefab1;
    public GameObject boxPrefab2;

    [Header("Circle Settings")]
    public int numberOfObjectsPerLayer = 20;  // 每层圆环上的数量
    public float radius = 5f;                 // 圆半径
    public int numberOfLayers = 3;            // 圆柱层数
    public float layerHeight = 2f;            // 每层高度间隔

    void Start()
    {
        GenerateCylinderArray();
    }

    void GenerateCylinderArray()
    {
        for (int layer = 0; layer < numberOfLayers; layer++)
        {
            float y = layer * layerHeight; // 每层高度

            for (int i = 0; i < numberOfObjectsPerLayer; i++)
            {
                // 随机选择一个 Box
                GameObject prefab = Random.value > 0.5f ? boxPrefab1 : boxPrefab2;

                // 计算圆环角度
                float angle = i * Mathf.PI * 2f / numberOfObjectsPerLayer;

                // 计算位置
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                Vector3 pos = new Vector3(x, y, z) + transform.position;

                // 面向圆柱中心轴（只考虑XZ平面）
                Vector3 directionToAxis = new Vector3(transform.position.x - pos.x, 0, transform.position.z - pos.z);
                Quaternion rotation = Quaternion.LookRotation(directionToAxis);

                rotation *= Quaternion.Euler(0, 90f, 0);

                // 实例化
                Instantiate(prefab, pos, rotation, transform);
            }
        }
    }
}
