using System;
using UnityEngine;

public class RockLibrary : MonoBehaviour
{
    private static Sprite[] s_rocks = Array.Empty<Sprite>();

    public Sprite[] Rocks;

    public static Sprite GetRock(uint index)
    {
        return s_rocks[Math.Min(index, s_rocks.Length)];
    }

    private void Awake()
    {
        s_rocks = Rocks;
    }
}
