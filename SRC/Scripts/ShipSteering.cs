using UnityEngine;

public class ShipSteering : MonoBehaviour
{
    [SerializeField] private float _turnRate = 80f;
    [SerializeField] private float _levelDamping = 2.0f;

    void Update()
    {
        if (InputManager.instance == null || InputManager.instance.steering == null)
            return;

        // Get joystick input
        Vector2 steeringInput = InputManager.instance.steering.GetInputDirection();

        if (steeringInput.sqrMagnitude < 0.001f)
        {
            Debug.Log("[ShipSteering] Joystick Centered (no movement)");
            return;
        }

        Debug.Log($"[ShipSteering] Steering Input: {steeringInput}");

        // Rotate based on input
        float yaw = steeringInput.x * _turnRate * Time.deltaTime;
        float pitch = -steeringInput.y * _turnRate * Time.deltaTime;

        transform.Rotate(pitch, yaw, 0f, Space.Self);

        // Smoothly level out Z axis (roll)
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.z = Mathf.LerpAngle(currentEuler.z, 0f, _levelDamping * Time.deltaTime);
        transform.eulerAngles = currentEuler;
    }
}
