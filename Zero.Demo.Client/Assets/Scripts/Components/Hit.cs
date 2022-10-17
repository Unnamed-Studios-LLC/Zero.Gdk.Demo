using UnityEngine;
using Zero.Demo.Model.Data;

public class Hit : MonoBehaviour, IDataHandler<HitData>
{
    private float _hitTime = -1;

    public SpriteRenderer SpriteRenderer;
    public Color HitTint = Color.red;
    public float TintSpeed = 1;

    public void Process(ref HitData data)
    {
        _hitTime = Time.time;
    }

    private void LateUpdate()
    {
        SpriteRenderer.color = Color.Lerp(HitTint, Color.white, (Time.time - _hitTime) * TintSpeed);
    }
}
