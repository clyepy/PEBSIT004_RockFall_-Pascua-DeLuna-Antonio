using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 90f; // Adjusted for smoother control

    void Update()
    {
        // Ensure InputManager and joystick exist
        if (InputManager.instance == null || InputManager.instance.steering == null)
            return;

        // Get joystick input (using GetInputDirection)
        Vector2 input = InputManager.instance.steering.GetInputDirection();

        // If joystick is centered, skip movement
        if (input.sqrMagnitude < 0.01f)
            return;

        // Rotate left/right (x controls rotation)
        transform.Rotate(Vector3.up * input.x * _rotationSpeed * Time.deltaTime);

        // Move forward/backward (y controls forward)
        Vector3 moveDirection = transform.forward * input.y * _moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        Debug.Log($"[ShipMovement] MoveDir: {moveDirection}, Input: {input}");
    }
}
