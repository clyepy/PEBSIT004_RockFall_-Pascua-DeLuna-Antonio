using UnityEngine;

public class Boundary : MonoBehaviour
{
    public float warningRadius = 50f;   // distance where warning shows
    public float destroyRadius = 70f;   // distance where ship is destroyed

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, warningRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }
}
