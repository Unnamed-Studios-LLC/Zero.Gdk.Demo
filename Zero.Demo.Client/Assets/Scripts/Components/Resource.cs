using UnityEngine;
using Zero.Demo.Model;
using Zero.Demo.Model.Data;

public class Resource : MonoBehaviour, IDataHandler<ResourceData>, IDataHandler<ShipData>
{
    public Transform Parent;
    public Transform Fill;
    public SpriteRenderer FillSpriteRenderer;

    private ShipType? _shipType;
    private int _amount;

    public void Process(ref ResourceData data)
    {
        _amount = data.Amount;
    }

    public void Process(ref ShipData data)
    {
        _shipType = data.ShipType;
    }

    private void LateUpdate()
    {
        var active = _shipType != null;
        Parent.gameObject.SetActive(active);
        if (!active)
        {
            return;
        }

        var ship = ShipLibrary.Get(_shipType.Value);
        var max = ship.MaxWood;

        var percent = Mathf.Clamp01(_amount / (float)max);
        Parent.localScale = new Vector3(max / 5f, 1, 1);
        Fill.localScale = new Vector3(percent, 1, 1);
    }

    private void OnEnable()
    {
        _shipType = null;
        _amount = 0;
    }
}
