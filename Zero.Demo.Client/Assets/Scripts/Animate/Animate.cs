using AnimateInternal;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class Animate
{
    private static AnimateInstance _instance;

    public static DurationTask Alpha(Graphic graphic, float duration, float targetAlpha)
    {
        Init();
        var startAlpha = graphic.color.a;
        return Duration(duration, step =>
        {
            var color = graphic.color;
            color.a = startAlpha + (targetAlpha - startAlpha) * step;
            graphic.color = color;
        });
    }

    public static DurationTask FadeIn(Graphic graphic, float alpha = 1, float duration = 0.4f)
    {
        var color = graphic.color;
        color.a = 0;
        graphic.color = color;

        return Alpha(graphic, duration, alpha);
    }

    public static DurationTask FadeOut(Graphic graphic, float alpha = 0, float duration = 0.4f)
    {
        return Alpha(graphic, duration, alpha);
    }

    public static AnimateTaskCollection Group()
    {
        return new AnimateGroup();
    }

    public static AnimateTaskCollection Group(params AnimateTask[] tasks)
    {
        return Collection(Group(), tasks);
    }

    public static DurationTask Int(int from, int to, float duration, Action<int> valueAction)
    {
        if (valueAction is null)
        {
            throw new ArgumentNullException(nameof(valueAction));
        }

        Init();
        return Duration(duration, step =>
        {
            valueAction((int)(from + (to - from) * step));
        });
    }

    public static DurationTask MoveTo(RectTransform rectTransform, Vector2 anchoredPosition, float duration = 0.4f)
    {
        Init();
        var startPosition = rectTransform.anchoredPosition;
        return Duration(duration, step =>
        {
            rectTransform.anchoredPosition = startPosition + (anchoredPosition - startPosition) * step;
        });
    }

    public static DurationTask MoveTo(Transform transform, Vector3 position, float duration = 0.4f)
    {
        Init();
        var startPosition = transform.position;
        return Duration(duration, step =>
        {
            transform.position = startPosition + (position - startPosition) * step;
        });
    }

    public static void Play(AnimateTask task)
    {
        _instance.AddTask(task);
    }

    public static AnimateTaskCollection Sequence()
    {
        return new AnimateSequence();
    }

    public static AnimateTaskCollection Sequence(params AnimateTask[] tasks)
    {
        return Collection(Sequence(), tasks);
    }

    public static DurationTask Scale(Transform transform, float xyz, float duration = 0.4f)
    {
        return Scale(transform, new Vector3(xyz, xyz, xyz), duration);
    }

    public static DurationTask Scale(Transform transform, Vector3 scale, float duration = 0.4f)
    {
        Init();
        var startScale = transform.localScale;
        return Duration(duration, step =>
        {
            transform.localScale = startScale + (scale - startScale) * step;
        });
    }

    public static DurationTask Shake(RectTransform rectTransform, float scale = 1, float duration = 0.4f)
    {
        Init();
        return AnchoredPosition(rectTransform, duration, (step, start) =>
        {
            return start + Animations.Shake(step, 0) * scale;
        });
    }

    public static DurationTask Size(RectTransform rectTransform, Vector2 size, float duration = 0.4f)
    {
        Init();
        var startScale = rectTransform.sizeDelta;
        return Duration(duration, step =>
        {
            rectTransform.sizeDelta = startScale + (size - startScale) * step;
        });
    }

    public static DurationTask UInt(uint from, uint to, float duration, Action<uint> valueAction)
    {
        if (valueAction is null)
        {
            throw new ArgumentNullException(nameof(valueAction));
        }

        Init();
        return Duration(duration, step =>
        {
            valueAction((uint)(from + (to - from) * step));
        });
    }

    public static DurationTask Wait(float duration)
    {
        Init();
        return Duration(duration, null);
    }

    public static AnimateTaskCollection Collection(AnimateTaskCollection collection, AnimateTask[] tasks)
    {
        foreach (var task in tasks)
        {
            collection.Add(task);
        }
        return collection;
    }

    private static DurationTask Duration(float duration, Action<float> stepAction)
    {
        var task = new DurationTask();
        task.Setup(duration, stepAction);
        return task;
    }

    private static void Init()
    {
        if (_instance != null)
        {
            return;
        }

        var obj = new GameObject("_animateInternal");
        UnityEngine.Object.DontDestroyOnLoad(obj);

        _instance = obj.AddComponent<AnimateInstance>();
    }

    private static DurationTask AnchoredPosition(RectTransform rectTransform, float duration, Func<float, Vector2, Vector2> stepAction)
    {
        var startPosition = rectTransform.anchoredPosition;
        return Duration(duration, step =>
        {
            rectTransform.anchoredPosition = stepAction(step, startPosition);
        });
    }
}
