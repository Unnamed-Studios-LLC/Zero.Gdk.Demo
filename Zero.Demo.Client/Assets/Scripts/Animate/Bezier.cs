using System;
using System.Collections.Generic;

public static class Bezier
{
    private const int LookupPrecision = 1000;
    private const float LookupPrecisionF = LookupPrecision;
    private const float LookupPrecisionRatio = 1 / LookupPrecisionF;

    private readonly static Dictionary<BezierCurve, float[]> s_lookups = new Dictionary<BezierCurve, float[]>();

    public static float Lerp(float time, float aX, float aY, float bX, float bY)
    {
        return Lerp(time, new BezierCurve(aX, aY, bX, bY));
    }

    public static float Lerp(float time, BezierCurve curve)
    {
        if (curve.X1 < 0 || curve.X2 < 0 || curve.X1 > 1 || curve.X2 > 1)
        {
            throw new Exception("X values of the lerp points cannot be outside of [0, 1] range");
        }

        time = UnityEngine.Mathf.Clamp01(time);

        if (time == 0)
        {
            return 0;
        }

        if (time == 1)
        {
            return 1;
        }

        if (!s_lookups.TryGetValue(curve, out var lookups))
        {
            lookups = CreateLookups(curve);
            s_lookups[curve] = lookups;
        }

        var index = Array.BinarySearch(lookups, 0, LookupPrecision, time);
        if (index < 0)
        {
            index = ~index;
        }


        float found;
        if (index >= LookupPrecision)
        {
            found = lookups[LookupPrecision - 1];
        }
        else
        {
            found = lookups[index];
        }

        float next;
        if (index + 1 >= LookupPrecision)
        {
            next = 1;
        }
        else
        {
            next = lookups[index + 1];
        }

        var remainder = (time % LookupPrecisionRatio) * (next - found);
        var funcT = found + remainder;

        return CubicBezier(UnityEngine.Mathf.Clamp01(funcT), curve.Y1, curve.Y2);
    }

    private static float[] CreateLookups(BezierCurve curve)
    {
        var lookups = new float[LookupPrecision];
        for (int i = 0; i < LookupPrecision; i++)
        {
            float t = i / (float)LookupPrecision;
            lookups[i] = CubicBezier(t, curve.X1, curve.X2);
        }
        return lookups;
    }

    private static float CubicBezier(float t, float a, float b)
    {
        var tt = t * t;
        var ttt = t * t * t;

        var p1F = 3 * ttt - 6 * tt + 3 * t;
        var p2F = -3 * ttt + 3 * tt;

        var r = p1F * a + p2F * b + ttt;

        return r;
    }
}
