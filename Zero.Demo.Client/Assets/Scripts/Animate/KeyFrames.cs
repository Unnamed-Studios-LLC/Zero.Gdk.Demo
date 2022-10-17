using UnityEngine;

public static class KeyFrames
{
    private static readonly (float, Vector2)[][] s_shakeKeyFrames = new (float, Vector2)[][]
    {
        new (float, Vector2)[]
        {
            (0.125f, new Vector2(-1, -1)),
            (0.25f, new Vector2(1.5f, 0)),
            (0.375f, new Vector2(2, -2)),
            (0.5f, new Vector2(-2, 2)),
            (0.625f, new Vector2(-2, -1.4f)),
            (0.75f, new Vector2(1.2f, 0.8f)),
            (0.875f, new Vector2(0.8f, -1f)),
            (1.0f, new Vector2(0, 0))
        },
        new (float, Vector2)[]
        {
            (0.125f, new Vector2(1, 1)),
            (0.25f, new Vector2(1.5f, -1)),
            (0.375f, new Vector2(-2, 2)),
            (0.5f, new Vector2(-2, -2)),
            (0.625f, new Vector2(2, 1.4f)),
            (0.75f, new Vector2(-1.2f, 0.8f)),
            (0.875f, new Vector2(-0.8f, -1f)),
            (1.0f, new Vector2(0, 0))
        },
        new (float, Vector2)[]
        {
            (0.125f, new Vector2(-1, 0.2f)),
            (0.25f, new Vector2(1.5f, 1)),
            (0.375f, new Vector2(-2, 2)),
            (0.5f, new Vector2(2, -2)),
            (0.625f, new Vector2(2, 1.4f)),
            (0.75f, new Vector2(-1.2f, -0.8f)),
            (0.875f, new Vector2(-0.8f, 1f)),
            (1.0f, new Vector2(0, 0))
        }
    };

    public static (float, Vector2)[] Shake(int index = 0)
    {
        return s_shakeKeyFrames[index % s_shakeKeyFrames.Length];
    }
}
