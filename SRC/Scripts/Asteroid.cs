using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Give the asteroid a random direction in 3D space
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-0.2f, 0.2f), // small Y variation so it stays near plane
                Random.Range(-1f, 1f)
            ).normalized;

            rb.linearVelocity = randomDirection * _speed;
        }
        else
        {
            Debug.LogWarning("No Rigidbody found on Asteroid prefab!");
        }

        // Optional: add indicator if system exists
        if (IndicatorManager.instance != null &&
            GameManager.instance != null &&
            GameManager.instance.currentSpaceStation != null)
        {
            var indicator = IndicatorManager.instance.AddIndicator(gameObject, Color.red);
            indicator.showDistanceTo = GameManager.instance.currentSpaceStation.transform;
        }
    }
}
