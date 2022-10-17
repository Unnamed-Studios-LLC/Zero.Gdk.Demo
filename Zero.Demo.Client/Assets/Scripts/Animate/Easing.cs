public static class Easing
{
    public static float EaseInBack(float time) => Bezier.Lerp(time, 0.36f, 0, 0.66f, -0.56f);
    public static float EaseInCirc(float time) => Bezier.Lerp(time, 0.55f, 0, 1, 0.45f);
    public static float EaseInCubic(float time) => Bezier.Lerp(time, 0.32f, 0, 0.67f, 0);
    public static float EaseInExpo(float time) => Bezier.Lerp(time, 0.7f, 0, 0.84f, 0);
    public static float EaseInQuad(float time) => Bezier.Lerp(time, 0.11f, 0, 0.5f, 0);
    public static float EaseInQuart(float time) => Bezier.Lerp(time, 0.5f, 0, 0.75f, 0);
    public static float EaseInQuint(float time) => Bezier.Lerp(time, 0.64f, 0, 0.78f, 0);
    public static float EaseInSine(float time) => Bezier.Lerp(time, 0.12f, 0, 0.39f, 0);

    public static float EaseInOutBack(float time) => Bezier.Lerp(time, 0.68f, -0.6f, 0.32f, 1.6f);
    public static float EaseInOutCirc(float time) => Bezier.Lerp(time, 0.85f, 0, 0.15f, 1);
    public static float EaseInOutCubic(float time) => Bezier.Lerp(time, 0.65f, 0, 0.35f, 1);
    public static float EaseInOutExpo(float time) => Bezier.Lerp(time, 0.87f, 0, 0.13f, 1);
    public static float EaseInOutQuad(float time) => Bezier.Lerp(time, 0.45f, 0, 0.55f, 1);
    public static float EaseInOutQuart(float time) => Bezier.Lerp(time, 0.75f, 0, 0.24f, 1);
    public static float EaseInOutQuint(float time) => Bezier.Lerp(time, 0.83f, 0, 0.17f, 1);
    public static float EaseInOutSine(float time) => Bezier.Lerp(time, 0.37f, 0, 0.63f, 1);

    public static float EaseOutBack(float time) => Bezier.Lerp(time, 0.34f, 1.56f, 0.64f, 1);
    public static float EaseOutCirc(float time) => Bezier.Lerp(time, 0, 0.55f, 0.45f, 1);
    public static float EaseOutCubic(float time) => Bezier.Lerp(time, 0.33f, 1, 0.68f, 1);
    public static float EaseOutExpo(float time) => Bezier.Lerp(time, 0.16f, 1, 0.3f, 1);
    public static float EaseOutQuad(float time) => Bezier.Lerp(time, 0.5f, 1, 0.89f, 1);
    public static float EaseOutQuart(float time) => Bezier.Lerp(time, 0.25f, 1, 0.5f, 1);
    public static float EaseOutQuint(float time) => Bezier.Lerp(time, 0.22f, 1, 0.36f, 1);
    public static float EaseOutSine(float time) => Bezier.Lerp(time, 0.61f, 1, 0.88f, 1);

    public static float Function(EasingType type, float time) => type switch
    {
        EasingType.EaseInBack => EaseInBack(time),
        EasingType.EaseInCirc => EaseInCirc(time),
        EasingType.EaseInCubic => EaseInCubic(time),
        EasingType.EaseInExpo => EaseInExpo(time),
        EasingType.EaseInQuad => EaseInQuad(time),
        EasingType.EaseInQuart => EaseInQuart(time),
        EasingType.EaseInQuint => EaseInQuint(time),
        EasingType.EaseInSine => EaseInSine(time),

        EasingType.EaseInOutBack => EaseInOutBack(time),
        EasingType.EaseInOutCirc => EaseInOutCirc(time),
        EasingType.EaseInOutCubic => EaseInOutCubic(time),
        EasingType.EaseInOutExpo => EaseInOutExpo(time),
        EasingType.EaseInOutQuad => EaseInOutQuad(time),
        EasingType.EaseInOutQuart => EaseInOutQuart(time),
        EasingType.EaseInOutQuint => EaseInOutQuint(time),
        EasingType.EaseInOutSine => EaseInOutSine(time),

        EasingType.EaseOutBack => EaseOutBack(time),
        EasingType.EaseOutCirc => EaseOutCirc(time),
        EasingType.EaseOutCubic => EaseOutCubic(time),
        EasingType.EaseOutExpo => EaseOutExpo(time),
        EasingType.EaseOutQuad => EaseOutQuad(time),
        EasingType.EaseOutQuart => EaseOutQuart(time),
        EasingType.EaseOutQuint => EaseOutQuint(time),
        EasingType.EaseOutSine => EaseOutSine(time),
        _ => time
    };
}