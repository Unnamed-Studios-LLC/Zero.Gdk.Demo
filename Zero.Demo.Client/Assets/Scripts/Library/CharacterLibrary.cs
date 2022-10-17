using UnityEngine;

public class CharacterLibrary : MonoBehaviour
{
    private static Sprite[] s_hats;
    private static Color[] s_headColors;
    private static Color[] s_legColors;
    private static Sprite[] s_flags;
    private static Color[] s_flagColors;

    public Sprite[] Hats;
    public Color[] HeadColors;
    public Color[] LegColors;
    public Sprite[] Flags;
    public Color[] FlagColors;

    public static int HatCount => s_hats.Length;
    public static int HeadCount => s_headColors.Length;
    public static int LegsCount => s_legColors.Length;
    public static int FlagCount => s_flags.Length;
    public static int FlagColorCount => s_flagColors.Length;

    public static Sprite GetHat(int index)
    {
        return s_hats[Mod(index, s_hats.Length)];
    }

    public static Color GetHeadColor(int index)
    {
        return s_headColors[Mod(index, s_hats.Length)];
    }

    public static Color GetLegsColor(int index)
    {
        return s_legColors[Mod(index, s_legColors.Length)];
    }

    public static Sprite GetFlag(int index)
    {
        return s_flags[Mod(index, s_flags.Length)];
    }

    public static Color GetFlagColor(int index)
    {
        return s_flagColors[Mod(index, s_flagColors.Length)];
    }

    private void Awake()
    {
        s_hats = Hats;
        s_headColors = HeadColors;
        s_legColors = LegColors;
        s_flags = Flags;
        s_flagColors = FlagColors;
    }

    private static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
