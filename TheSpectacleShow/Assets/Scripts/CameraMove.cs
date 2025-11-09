using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class CameraMove : MonoBehaviour
{
    [Header("移动与旋转参数")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
    public float vrTurnSpeed = 60f;
    public float groundCheckDistance = 0.3f;

    [Header("相机引用")]
    public Transform cameraTransform;

    private bool isVR;
    private float rotationX;
    private float rotationY;
    private Rigidbody rb;
    [SerializeField]
    private bool isGrounded;

    void Start()
    {
        isVR = XRSettings.isDeviceActive;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 禁止翻滚

        if (!isVR)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        GroundCheck();

        if (isVR)
            RotateByVRController();
        else
            RotateByMouse();

        Move();

        if (Input.GetButtonDown("Jump")/* && isGrounded*/)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;

        Vector3 moveDir = (forward.normalized * v + right.normalized * h).normalized;

        // 保留原本的竖直速度
        Vector3 currentVel = rb.velocity;
        Vector3 targetVel = moveDir * moveSpeed;
        Vector3 newVel = new Vector3(targetVel.x, currentVel.y, targetVel.z);

        rb.velocity = newVel;
    }

    void GroundCheck()
    {
        // 从角色底部检测是否着地
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance + 0.1f);
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
        float turnInput = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickHorizontal");
        if (Mathf.Abs(turnInput) > 0.1f)
            transform.Rotate(Vector3.up, turnInput * vrTurnSpeed * Time.deltaTime);
    }
}
