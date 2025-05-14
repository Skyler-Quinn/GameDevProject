using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private Transform playerCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;

        // Reset vertical velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative to stick to ground
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Movement direction relative to camera
        Vector3 right = playerCamera.right;
        Vector3 forward = Vector3.Scale(playerCamera.forward, new Vector3(1, 0, 1)).normalized;
        moveDirection = (right * h + forward * v).normalized;

        Vector3 move = moveDirection * moveSpeed;

        // Apply gravity
        velocity.y += gravity * gravityMultiplier * Time.deltaTime;

        // Combine movement and gravity
        controller.Move((move + velocity) * Time.deltaTime);
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        enabled = isEnabled;
    }
}
