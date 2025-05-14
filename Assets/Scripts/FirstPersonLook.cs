using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerBody;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 70f;
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private float minVerticalAngle = -80f;
    [SerializeField] private float maxVerticalAngle = 80f;
    [SerializeField] private bool invertY = false;

    private float xRotation = 0f;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaSmoothed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Raw input
        Vector2 targetMouseDelta = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        // Apply sensitivity
        targetMouseDelta *= mouseSensitivity * Time.deltaTime;

        // Optional invert Y
        if (invertY)
            targetMouseDelta.y = -targetMouseDelta.y;

        // Smooth the mouse delta using Lerp
        currentMouseDeltaSmoothed = Vector2.Lerp(
            currentMouseDeltaSmoothed,
            targetMouseDelta,
            1f / smoothing
        );

        // Apply pitch (up/down) to camera
        xRotation -= currentMouseDeltaSmoothed.y;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Apply yaw (left/right) to player body
        playerBody.Rotate(Vector3.up * currentMouseDeltaSmoothed.x);
    }

    public void SetLookEnabled(bool isEnabled)
    {
        enabled = isEnabled;
        Cursor.lockState = isEnabled ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isEnabled;
    }
}
