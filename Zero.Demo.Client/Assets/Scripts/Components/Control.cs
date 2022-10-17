using UnityEngine;
using Zero.Demo.Model;
using Zero.Demo.Model.Data;

public class Control : Component,
    IDataHandler<EventData>,
    IDataHandler<ShipData>,
    IDataHandler<ShootData>
{
    public AudioSource AudioSource;

    private bool _controlled;
    private ShipType _shipType;

    private bool _didShoot;
    private float _duration;
    private Vector2 _indicatorMin;
    private Vector2 _indicatorMax;

    public void Process(ref ShipData data)
    {
        _shipType = data.ShipType;
        if (!_controlled)
        {
            return;
        }

        UpdateCannonSpread();
    }

    public void Process(ref ShootData data)
    {
        if (_didShoot)
        {
            _indicatorMin = Vector2.Min(data.Target.ToUnity(), _indicatorMin);
            _indicatorMax = Vector2.Max(data.Target.ToUnity(), _indicatorMax);
            _duration = Mathf.Min(_duration, data.Duration);
        }
        else
        {
            _indicatorMin = data.Target.ToUnity();
            _indicatorMax = data.Target.ToUnity();
            _didShoot = true;
            _duration = data.Duration;
        }
    }

    public void Process(ref EventData data)
    {
        if (!_controlled)
        {
            return;
        }

        switch (data.EventType)
        {
            case Zero.Demo.Model.EventType.Pickup:
                if (SfxLibrary.PickupSfx.Length != 0)
                {
                    AudioSource.PlayOneShot(SfxLibrary.PickupSfx.Random(), 0.8f);
                }
                break;
        }
    }

    public void SetControlled()
    {
        _controlled = true;
        UpdateCannonSpread();
    }

    private Indicator GetIndicator()
    {
        var indicator = ComponentLibrary.Get<Indicator>();
        indicator.transform.localScale = Vector3.one;
        return indicator;
    }

    private Rect GetIndicatorRect()
    {
        const float rectBuffer = 0.2f;
        _indicatorMin -= new Vector2(rectBuffer, rectBuffer);
        _indicatorMax += new Vector2(rectBuffer, rectBuffer);

        return new Rect(_indicatorMin, _indicatorMax - _indicatorMin);
    }

    private void LateUpdate()
    {
        if (!_didShoot)
        {
            return;
        }
        _didShoot = false;

        var indicatorRect = GetIndicatorRect();
        var indicator = GetIndicator();
        indicator.Setup(indicatorRect, _duration, !_controlled);

        var definition = ShipLibrary.Get(_shipType);
        if (definition.ShootSfx.Length != 0)
        {
            AudioSource.PlayOneShot(definition.ShootSfx.Random(), World.GetVolumeFromPlayer(transform.position) * 0.85f);
        }

        if (_controlled)
        {
            World.Aim.StartCooldown();

            if (definition.ReloadSfx.Length != 0)
            {
                AudioSource.PlayOneShot(definition.ReloadSfx.Random());
            }
        }
    }

    private void OnEnable()
    {
        _controlled = false;
    }

    private void UpdateCannonSpread()
    {
        var definition = ShipLibrary.Get(_shipType);
        World.Direction.size = new Vector2(definition.ShadowSize.x * 1.2f, 9f / 8f);
        World.Aim.CannonSpread = definition.CannonSpread;
        World.Aim.FireRate = definition.FireRate;
        World.Aim.MaxRange = definition.MaxRange;
        World.SetMaxRange(definition.MaxRange);
    }
}
