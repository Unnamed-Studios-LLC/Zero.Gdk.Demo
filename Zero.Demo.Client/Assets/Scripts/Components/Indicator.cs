using UnityEngine;

public class Indicator : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Color EnemyColor = Color.white;
    public Color AllyColor = Color.white;

    private float _endTime;

    public void Setup(Rect rect, float duration, bool enemy)
    {
        SpriteRenderer.size = rect.size;
        SpriteRenderer.color = enemy ? EnemyColor : AllyColor;
        transform.position = rect.center;
        _endTime = Time.time + duration;
    }

    private void LateUpdate()
    {
        if (Time.time > _endTime)
        {
            ComponentLibrary.Return(this);
            return;
        }
    }
}
