using UnityEngine;

public static class Animations
{
    public static Vector2 Shake(float time, int index = 0)
    {
        return Lerp.Vector2(KeyFrames.Shake(index), Bezier.Lerp(time, 0.36f, 0.07f, 0.19f, 0.97f));
    }
}