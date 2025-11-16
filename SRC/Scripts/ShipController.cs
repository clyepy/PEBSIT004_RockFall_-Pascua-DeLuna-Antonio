using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 60f;
    [SerializeField] private float tiltAmount = 10f;
    [SerializeField] private float smoothRotation = 5f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;

    private float yaw;   // Turning left/right
    private float pitch; // Tilting up/down
    private float roll;  // Visual tilt

    void Update()
    {
        if (InputManager.instance == null || InputManager.instance.steering == null)
            return;

        Vector2 input = InputManager.instance.steering.GetInputDirection();

        // Smooth rotation changes
        yaw += input.x * turnSpeed * Time.deltaTime;   // Horizontal turn
        pitch -= input.y * turnSpeed * 0.5f * Time.deltaTime; // Pitch (less sensitive)
        pitch = Mathf.Clamp(pitch, -45f, 45f);         // Optional: limit pitch

        // Tilt visually when turning
        roll = Mathf.Lerp(roll, -input.x * tiltAmount, Time.deltaTime * smoothRotation);

        // Build target rotation
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotation);

        // Always move forward smoothly
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (showDebugLogs)
            Debug.Log($"[ShipController] Input: {input}, Pitch: {pitch}, Yaw: {yaw}, Roll: {roll}");
    }
}
