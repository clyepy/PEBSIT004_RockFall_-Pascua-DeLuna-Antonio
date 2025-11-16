using UnityEngine;

public class Shot : MonoBehaviour
{
    public float speed = 30f;      // Bullet speed
    public float lifetime = 2f;    // Destroy after 2 seconds

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move bullet upwards relative to its rotation
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
