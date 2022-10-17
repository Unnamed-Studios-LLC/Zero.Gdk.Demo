using System;

[Serializable]
public struct BezierCurve
{
    public float X1;
    public float X2;
    public float Y1;
    public float Y2;

    public BezierCurve(float x1, float y1, float x2, float y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }
}
