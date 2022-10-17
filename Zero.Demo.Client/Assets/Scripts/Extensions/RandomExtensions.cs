public static class RandomExtensions
{
    public static T Random<T>(this T[] array)
    {
        if (array == null ||
            array.Length == 0)
        {
            return default;
        }

        return array[UnityEngine.Random.Range(0, array.Length)];
    }
}
