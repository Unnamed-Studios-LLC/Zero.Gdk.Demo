using System.Threading.Tasks;
using UnityEngine;

public class MenuBob : MonoBehaviour
{
    public float ShakeTimeScale = 1;
    public float ShakeSizeScale = 1;
    private Vector2 Offsets;

    private RectTransform RectTransform => (RectTransform)transform;

    private void Awake()
    {
        Offsets = RectTransform.anchorMin * -1 * new Vector2(Screen.width, Screen.height);
    }

    private void LateUpdate()
    {
        var offset = Animations.Shake((Time.time * ShakeTimeScale) % 1) * ShakeSizeScale * Offsets;
        var rectTransform = RectTransform;
        rectTransform.offsetMin = offset;
        rectTransform.offsetMax = offset;
    }
}
