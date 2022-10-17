using System;
using UnityEngine;

public class WoodLibrary : MonoBehaviour
{
    private static Sprite[] s_sprites = Array.Empty<Sprite>();

    public Sprite[] Wood;

    public static Sprite GetWood(uint index)
    {
        return s_sprites[Math.Min(index, s_sprites.Length)];
    }

    private void Awake()
    {
        s_sprites = Wood;
    }
}
