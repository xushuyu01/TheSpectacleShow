using UnityEngine;
using UnityEngine.XR;

public class CameraMove : MonoBehaviour
{
    [Header("�ƶ�����ת����")]
    public float moveSpeed = 2f;
    public float mouseSensitivity = 2f;
    public float vrTurnSpeed = 60f; // VR �ֱ���ת�ٶȣ���/�룩

    [Header("�������")]
    public Transform cameraTransform; // VRʱΪͷ�������PCʱΪ�������

    private bool isVR;
    private float rotationX;
    private float rotationY;

    void Start()
    {
        // ��鵱ǰ�Ƿ�������VR�豸
        isVR = XRSettings.isDeviceActive;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (!isVR)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        Move();

        if (isVR)
            RotateByVRController();
        else
            RotateByMouse();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �������������ƶ����򣨱���ˮƽ��
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;

        Vector3 move = (forward.normalized * v + right.normalized * h) * moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void RotateByMouse()
    {
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, -80f, 80f);

        transform.rotation = Quaternion.Euler(0, rotationX, 0);
        if (cameraTransform)
            cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);
    }

    void RotateByVRController()
    {
        // ���ͨ��OpenXR�����ֱ�����ת���루X�ᣩ
        float turnInput = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickHorizontal");
        if (Mathf.Abs(turnInput) > 0.1f)
            transform.Rotate(Vector3.up, turnInput * vrTurnSpeed * Time.deltaTime);
    }
}
