using UnityEngine;

public class EnvGen : MonoBehaviour
{
    [Header("Box Prefabs")]
    public GameObject boxPrefab1;
    public GameObject boxPrefab2;

    [Header("Circle Settings")]
    public int numberOfObjectsPerLayer = 20;  // ÿ��Բ���ϵ�����
    public float radius = 5f;                 // Բ�뾶
    public int numberOfLayers = 3;            // Բ������
    public float layerHeight = 2f;            // ÿ��߶ȼ��

    void Start()
    {
        GenerateCylinderArray();
    }

    void GenerateCylinderArray()
    {
        for (int layer = 0; layer < numberOfLayers; layer++)
        {
            float y = layer * layerHeight; // ÿ��߶�

            for (int i = 0; i < numberOfObjectsPerLayer; i++)
            {
                // ���ѡ��һ�� Box
                GameObject prefab = Random.value > 0.5f ? boxPrefab1 : boxPrefab2;

                // ����Բ���Ƕ�
                float angle = i * Mathf.PI * 2f / numberOfObjectsPerLayer;

                // ����λ��
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                Vector3 pos = new Vector3(x, y, z) + transform.position;

                // ����Բ�������ᣨֻ����XZƽ�棩
                Vector3 directionToAxis = new Vector3(transform.position.x - pos.x, 0, transform.position.z - pos.z);
                Quaternion rotation = Quaternion.LookRotation(directionToAxis);

                rotation *= Quaternion.Euler(0, 90f, 0);

                // ʵ����
                Instantiate(prefab, pos, rotation, transform);
            }
        }
    }
}
