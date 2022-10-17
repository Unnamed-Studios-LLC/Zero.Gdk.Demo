using System;
using UnityEngine;
using Zero.Demo.Model;

[Serializable]
public struct ShipDefinition
{
    public ShipType ShipType;
    public Sprite Sprite;
    public float CannonSpread;
    public float FireRate;
    public float MaxRange;
    public int MaxWood;
    public Vector2 ShadowSize;
    public Vector2 FlagCoordinates;
    public Vector2[] Cannons;
    public Vector2[] CharacterCoordinates;
    public float[] Wakes;
    public AudioClip[] ShootSfx;
    public AudioClip[] ReloadSfx;
    public AudioClip[] HitSfx;

    public Vector2 GetCannon(int index)
    {
        return Cannons[index % Cannons.Length] / 8f;
    }

    public Vector2 GetCoordinates(int index)
    {
        return CharacterCoordinates[index % CharacterCoordinates.Length] / 8f + new Vector2(0, 0.1f);
    }
}
