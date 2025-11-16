using UnityEngine;

public class TestFollow : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            // Force camera to stick exactly behind the ship
            transform.position = target.position + new Vector3(0, 5, -10);
            transform.LookAt(target);
        }
    }
}
