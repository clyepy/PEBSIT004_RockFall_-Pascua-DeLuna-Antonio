using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform target;

    [Header("Camera Offset")]
    public float distance = 60f;   // how far behind the ship
    public float height = 3f;      // how high above the ship
    public float followSmoothness = 5f; // how smoothly the camera follows
    public float lookSmoothness = 5f;   // how smoothly the camera rotates

    private Vector3 _currentVelocity;

    void LateUpdate()
    {
        if (!target) return;

        // Calculate desired position behind and above the ship
        Vector3 desiredPosition = target.position 
                                - target.forward * distance 
                                + Vector3.up * height;

        // Smoothly move the camera to that position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, 1f / followSmoothness);

        // Smoothly rotate the camera to look at the ship
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * lookSmoothness);
    }
}
