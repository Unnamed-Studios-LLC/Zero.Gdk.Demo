using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform Knob;
    public RectTransform Shaft;
    public RectTransform Region;
    public bool ResetOnRelease;
    public bool VariableMagnitude;

    public bool Active { get; private set; }
    public Vector2 Vector => ((Vector2)Knob.position - RegionCenter) / MaxMagnitude;

    private float MaxMagnitude => Mathf.Min(Region.rect.width, Region.rect.height) / 2f;
    private Vector2 RegionCenter => (Vector2)Region.position + Region.rect.size * (Vector2.one * 0.5f - Region.pivot);

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Active)
        {
            Active = true;
        }
        SetPosition(eventData.position, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetPosition(eventData.position, true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Active && ResetOnRelease)
        {
            Active = false;
            SetPosition(RegionCenter, false);
        }
    }

    private void SetPosition(Vector2 screenPosition, bool dragged)
    {
        var maxMagnitude = MaxMagnitude;
        var vector = screenPosition - RegionCenter;
        var magnitude = vector.magnitude;
        if ((dragged && !VariableMagnitude) ||
            magnitude > maxMagnitude)
        {
            magnitude = maxMagnitude;
            vector = vector.normalized * maxMagnitude;
            screenPosition = RegionCenter + vector;
        }
        Knob.position = screenPosition;
        Shaft.sizeDelta = new Vector2(magnitude, Shaft.sizeDelta.y);
        var rotation = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        Shaft.localEulerAngles = new Vector3(0, 0, rotation);
    }
}
