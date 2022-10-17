using System;
using UnityEngine;
using Zero.Demo.Model.Data;

public class Aim : MonoBehaviour
{
    public World World;
    public DemoGameService GameService;
    public Transform Indicator;
    public SpriteRenderer IndicatorSpriteRenderer;
    public SpriteRenderer IndicatorSpriteRendererFill;

    private float _cooldown;

    private AimInputData _aimInputData;

    public float MaxRange { get; set; } = 20;
    public float FireRate { get; set; } = 1;
    public float FireRateSeconds => 1f / FireRate;
    public float CannonSpread { get; set; }
    public bool Aiming { get; set; }
    public Vector2 Coordinates { get; set; }
    public Vector2 ClippedCoordinates => GetClippedCoordinates();

    public void StartCooldown()
    {
        _cooldown = FireRateSeconds;
    }

    private void FixedUpdate()
    {
        UpdateAim();
    }

    private Vector2 GetClippedCoordinates()
    {
        if (World == null ||
            World.Controlled == null)
        {
            return Coordinates;
        }

        var controlled = (Vector2)World.Controlled.position;
        var vector = Coordinates - controlled;
        if (vector.sqrMagnitude <= MaxRange * MaxRange)
        {
            return Coordinates;
        }

        return controlled + vector.normalized * MaxRange;
    }

    private void LateUpdate()
    {
        _cooldown -= Time.deltaTime;
        if (Indicator == null)
        {
            return;
        }

        //Indicator.gameObject.SetActive(Aiming);
        Indicator.transform.position = ClippedCoordinates;
        IndicatorSpriteRenderer.size = new Vector2(CannonSpread, CannonSpread);
        IndicatorSpriteRendererFill.transform.localPosition = new Vector3(0, CannonSpread * -0.5f, 0);
        IndicatorSpriteRendererFill.transform.localScale = new Vector3(CannonSpread * 8, CannonSpread * 8 * Mathf.Clamp01((FireRateSeconds - _cooldown) / FireRateSeconds));
    }

    private void UpdateAim()
    {
        var coordinates = Coordinates.ToZero();
        if (_aimInputData.Aiming == Aiming &&
            _aimInputData.Coordinates == coordinates)
        {
            return;
        }

        _aimInputData = new AimInputData(coordinates, Aiming);
        GameService.Push(ref _aimInputData);
    }
}