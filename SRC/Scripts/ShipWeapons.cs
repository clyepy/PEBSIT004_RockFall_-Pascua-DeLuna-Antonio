using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapons : MonoBehaviour
{
    [SerializeField] private GameObject _shotPrefab;
    [SerializeField] private Transform[] _firePoints;

    private int _firePointIndex;

    void Start()
    {
        // Register with InputManager only if it exists
        if (InputManager.instance != null)
        {
            InputManager.instance.SetWeapons(this);
        }
        else
        {
            Debug.LogWarning("No InputManager found in the scene. ShipWeapons will not register.");
        }
    }

    void OnDestroy()
    {
        if (Application.isPlaying && InputManager.instance != null)
        {
            InputManager.instance.RemoveWeapons(this);
        }
    }

    public void Fire()
    {
        if (_firePoints == null || _firePoints.Length == 0 || _shotPrefab == null)
            return;

        var firePointToUse = _firePoints[_firePointIndex];

        // Spawn bullet
        GameObject shot = Instantiate(_shotPrefab, firePointToUse.position, firePointToUse.rotation);

        // If bullet has Rigidbody2D, add velocity
        Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Get ship's velocity
            Rigidbody2D shipRb = GetComponent<Rigidbody2D>();
            Vector2 shipVelocity = shipRb != null ? shipRb.linearVelocity : Vector2.zero;

            // Bullet’s own forward speed
            Vector2 bulletVelocity = firePointToUse.up * 20f; // adjust bullet speed here

            // Add both together so bullet inherits ship speed + its own
            rb.linearVelocity = shipVelocity + bulletVelocity;
        }

        _firePointIndex++;
        if (_firePointIndex >= _firePoints.Length)
            _firePointIndex = 0;
    }
}
