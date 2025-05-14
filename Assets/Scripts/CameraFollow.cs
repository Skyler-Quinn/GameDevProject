using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target; // The player transform
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -5f);
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Look Settings")]
    [SerializeField] private bool lookAtTarget = true;
    [SerializeField] private float rotationSmoothing = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
        transform.position = smoothedPosition;

        // Handle rotation
        if (lookAtTarget)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSmoothing * Time.deltaTime
            );
        }
    }

    // Call this to change the follow target
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Call this to change the offset (useful for cutscenes)
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}