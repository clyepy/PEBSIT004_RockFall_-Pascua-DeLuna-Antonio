using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform _thumb;
    private Vector2 _inputDirection = Vector2.zero;
    private float _radius;

    void Start()
    {
        // Calculate joystick movement radius
        _radius = GetComponent<RectTransform>().sizeDelta.x * 0.5f;

        // Ensure thumb starts in the center
        _thumb.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convert drag position into local joystick coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 pos
        );

        // Normalize direction
        pos = Vector2.ClampMagnitude(pos, _radius);
        _thumb.anchoredPosition = pos;
        _inputDirection = pos / _radius;

        Debug.Log($"[VirtualJoystick] Direction: {_inputDirection}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset thumb to center
        _inputDirection = Vector2.zero;
        _thumb.anchoredPosition = Vector2.zero;
        Debug.Log("[VirtualJoystick] Released - Centered");
    }

    // ✅ This is what ShipMovement and ShipSteering use
    public Vector2 GetInputDirection()
    {
        return _inputDirection;
    }
}
