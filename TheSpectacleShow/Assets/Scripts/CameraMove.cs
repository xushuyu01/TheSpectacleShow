using UnityEngine;
using UnityEngine.XR;

public class CameraMove : MonoBehaviour
{
    [Header("移动与旋转参数")]
    public float moveSpeed = 2f;
    public float mouseSensitivity = 2f;
    public float vrTurnSpeed = 60f; // VR 手柄旋转速度（度/秒）

    [Header("相机引用")]
    public Transform cameraTransform; // VR时为头显相机，PC时为主摄像机

    private bool isVR;
    private float rotationX;
    private float rotationY;

    void Start()
    {
        // 检查当前是否启用了VR设备
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

        // 用相机方向决定移动方向（保持水平）
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
        // 针对通用OpenXR右手手柄的旋转输入（X轴）
        float turnInput = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickHorizontal");
        if (Mathf.Abs(turnInput) > 0.1f)
            transform.Rotate(Vector3.up, turnInput * vrTurnSpeed * Time.deltaTime);
    }
}
