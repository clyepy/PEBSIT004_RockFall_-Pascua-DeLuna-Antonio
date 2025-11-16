using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro

public class Indicator : MonoBehaviour
{
    public Transform target;           // Object this indicator is tracking
    public Transform showDistanceTo;   // Object to calculate distance from

    public Color color
    {
        set { GetComponent<Image>().color = value; }
        get { return GetComponent<Image>().color; }
    }

    [SerializeField] private TMP_Text _distanceLabel; // TextMeshPro label
    [SerializeField] private int _margin = 50;        // Distance from screen edge
    [SerializeField] private float _slideDuration = 1.5f; // Time to slide to center

    private RectTransform _rectTransform;
    private Image _image;
    private bool _followTarget = false; // Flag to start following the target after slide

    void Start()
    {
        // Get references
        if (_distanceLabel == null)
            _distanceLabel = GetComponentInChildren<TMP_Text>();

        if (_distanceLabel != null)
            _distanceLabel.enabled = false;

        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        if (_image != null)
            _image.enabled = false;

        // Start sliding to center after 3 seconds
        StartCoroutine(SlideToCenterAfterDelay(53f));
    }

    IEnumerator SlideToCenterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_image != null)
            _image.enabled = true;

        if (_distanceLabel != null)
            _distanceLabel.enabled = true;

        Vector2 startPos = _rectTransform.localPosition;
        Vector2 endPos = Vector2.zero; // Center of parent
        float elapsed = 0f;

        while (elapsed < _slideDuration)
        {
            elapsed += Time.deltaTime;
            _rectTransform.localPosition = Vector2.Lerp(startPos, endPos, elapsed / _slideDuration);
            yield return null;
        }

        _rectTransform.localPosition = endPos; // Ensure exact center
        _followTarget = true; // Now start following the target
    }

    void Update()
    {
        if (!_followTarget) return; // Skip until slide finishes

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Update distance label
        if (_distanceLabel != null && showDistanceTo != null)
        {
            _distanceLabel.enabled = true;
            int distance = (int)Vector3.Magnitude(showDistanceTo.position - target.position);
            _distanceLabel.text = distance.ToString() + "m";
        }
        else if (_distanceLabel != null)
        {
            _distanceLabel.enabled = false;
        }

        // Ensure image is enabled
        if (_image != null)
            _image.enabled = true;

        // Convert target position to viewport
        var viewportPoint = Camera.main.WorldToViewportPoint(target.position);
        if (viewportPoint.z < 0)
        {
            viewportPoint.z = 0;
            viewportPoint = viewportPoint.normalized;
            viewportPoint.x *= -Mathf.Infinity;
        }

        // Convert to screen point and clamp
        var screenPoint = Camera.main.ViewportToScreenPoint(viewportPoint);
        screenPoint.x = Mathf.Clamp(screenPoint.x, _margin, Screen.width - _margin * 2);
        screenPoint.y = Mathf.Clamp(screenPoint.y, _margin, Screen.height - _margin * 2);

        // Convert to local position in parent
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            screenPoint,
            Camera.main,
            out localPosition);

        _rectTransform.localPosition = localPosition;
    }
}
