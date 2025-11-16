using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    public Transform player;       // Your ship (drag the player object here)
    public RectTransform mapRect;  // RawImage RectTransform (drag the RawImage here)
    public Vector2 mapSize = new Vector2(200, 200); // Size of the mini-map in UI units
    public Vector2 worldSize = new Vector2(1000, 1000); // Size of world covered by mini-map

    void Update()
    {
        // Get the player's position
        Vector3 playerPos = player.position;

        // Normalize the player's position to fit the mini-map
float x = (playerPos.x / worldSize.x) * mapSize.x;
float y = (playerPos.z / worldSize.y) * mapSize.y;
transform.localPosition = new Vector3(x, y, 0f);
    }
}
