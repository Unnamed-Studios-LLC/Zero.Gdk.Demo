using UnityEngine;
using Zero.Demo.Model;
using Zero.Demo.Model.Data;

public class Ship : Component,
    IDataHandler<ShipData>,
    IDataHandler<ShootData>,
    IDataHandler<HitData>,
    IDataHandler<EventData>,
    IDataHandler<IconData>,
    IDataHandler<FlagData>
{
    public SpriteRenderer SpriteRenderer;
    public Transform CannonParent;
    public AudioSource AudioSource;
    public TrailRenderer[] Wakes;
    public SpriteRenderer Icon;
    public SpriteRenderer Flag;
    public SpriteRenderer FlagColor;
    public SpriteRenderer Shadow;
    private int _cannonIndex = 0;
    private bool _wasEnabled = false;

    public ShipDefinition Definition { get; private set; }
    public ShipType Type { get; private set; }

    public void Process(ref EventData data)
    {
        switch (data.EventType)
        {
            case Zero.Demo.Model.EventType.Death:
                var death = ComponentLibrary.Get<DeathEffect>();
                death.transform.position = transform.position;
                death.Play();

                if (SfxLibrary.DeathSfx.Length != 0)
                {
                    World.AudioSource.PlayOneShot(SfxLibrary.DeathSfx.Random(), World.GetVolumeFromPlayer(transform.position) * 0.6f);
                }
                break;
            case Zero.Demo.Model.EventType.Upgrade:
                var upgrade = ComponentLibrary.Get<UpgradeEffect>();
                upgrade.transform.position = transform.position;
                upgrade.Play();

                if (SfxLibrary.UpgradeSfx.Length != 0)
                {
                    AudioSource.PlayOneShot(SfxLibrary.UpgradeSfx.Random(), World.GetVolumeFromPlayer(transform.position));
                }
                break;
        }
    }

    public void Process(ref FlagData data)
    {
        Flag.sprite = CharacterLibrary.GetFlag(data.Flag);
        FlagColor.color = CharacterLibrary.GetFlagColor(data.Color);
    }

    public void Process(ref IconData data)
    {
        var active = IconLibrary.TryGet(data.IconType, out var sprite);
        Icon.gameObject.SetActive(active);
        if (active)
        {
            Icon.sprite = sprite;
        }
    }

    public void Process(ref ShipData data)
    {
        Type = data.ShipType;
        Definition = ShipLibrary.Get(Type);
        SpriteRenderer.sprite = Definition.Sprite;
        FlagColor.transform.localPosition = new Vector3(Definition.FlagCoordinates.x / 8f, Definition.FlagCoordinates.y / 8f, -0.1f);
        Shadow.size = Definition.ShadowSize;

        for (int i = 0; i < Wakes.Length; i++)
        {
            var wake = Wakes[i];
            var active = i < Definition.Wakes.Length;
            wake.gameObject.SetActive(active);

            if (active)
            {
                var position = Definition.Wakes[i];
                wake.transform.localPosition = new Vector3(position, 0, 0);
            }
        }
    }

    public void Process(ref ShootData data)
    {
        var cannonPosition = Definition.GetCannon(_cannonIndex++);

        var effect = GetCannonEffect();
        effect.transform.localPosition = cannonPosition;
        effect.Play();

        var cannon = GetCannon();
        var origin = transform.position + new Vector3(cannonPosition.x, 0, cannonPosition.y);
        cannon.Setup(origin, data.Target.ToUnity(), data.Duration, World);
    }

    public void Process(ref HitData data)
    {
        if (Definition.HitSfx.Length != 0)
        {
            AudioSource.PlayOneShot(Definition.HitSfx.Random(), World.GetVolumeFromPlayer(transform.position));
        }
    }

    private Cannon GetCannon()
    {
        var cannon = ComponentLibrary.Get<Cannon>();
        cannon.transform.localScale = Vector3.one;
        return cannon;
    }

    private CannonEffect GetCannonEffect()
    {
        var cannonEffect = ComponentLibrary.Get<CannonEffect>();
        cannonEffect.transform.SetParent(CannonParent);
        cannonEffect.transform.localEulerAngles = Vector3.zero;
        cannonEffect.transform.localScale = Vector3.one;
        return cannonEffect;
    }

    private void OnEnable()
    {
        _wasEnabled = true;
    }

    private void LateUpdate()
    {
        if (_wasEnabled)
        {
            _wasEnabled = false;
            for (int i = 0; i < Wakes.Length; i++)
            {
                var wake = Wakes[i];
                wake.Clear();
            }
        }
    }
}
