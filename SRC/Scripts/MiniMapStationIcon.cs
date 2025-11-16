using UnityEngine;

public class MiniMapStationIcon : MonoBehaviour
{
    public Transform objectTransform; // The space station or enemy
    public RectTransform mapRect;     // The mini-map RawImage
    public Vector2 mapSize = new Vector2(200, 200);
    public Vector2 worldSize = new Vector2(1000, 1000);

    void Update()
    {
        Vector3 objectPos = objectTransform.position;
float x = (objectPos.x / worldSize.x) * mapSize.x;
float y = (objectPos.z / worldSize.y) * mapSize.y;
transform.localPosition = new Vector3(x, y, 0f);
    }
}
