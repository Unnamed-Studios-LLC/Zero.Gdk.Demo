using UnityEngine;

public static class Lerp
{
    public static float Float((float Time, float Value)[] frames, float time)
    {
        var (previousTime, previousValue, frameTime, frameValue) = GetAnimateFrames(frames, time);
        var frameProgression = (time - previousTime) / (frameTime - previousTime);
        return previousValue + (frameValue - previousValue) * frameProgression;
    }

    public static Vector2 Vector2((float Time, Vector2 Value)[] frames, float time)
    {
        var (previousTime, previousValue, frameTime, frameValue) = GetAnimateFrames(frames, time);
        var frameProgression = (time - previousTime) / (frameTime - previousTime);
        return UnityEngine.Vector2.Lerp(previousValue, frameValue, frameProgression);
    }

    private static (float, T, float, T) GetAnimateFrames<T>((float Time, T Value)[] frames, float time)
    {
        float previousTime = 0, frameTime = 0;
        T previousValue = default, frameValue = default;

        for (int i = 0; i < frames.Length; i++)
        {
            if (i == frames.Length - 1)
            {
                (frameTime, frameValue) = (1, default);
                (previousTime, previousValue) = frames[i];
            }
            else
            {
                (frameTime, frameValue) = frames[i];
                if (time >= frameTime)
                {
                    continue;
                }
            }

            if (i == 0)
            {
                previousTime = 0;
                previousValue = default;
            }
            else
            {
                (previousTime, previousValue) = frames[i - 1];
            }

            break;
        }

        return (previousTime, previousValue, frameTime, frameValue);
    }
}
