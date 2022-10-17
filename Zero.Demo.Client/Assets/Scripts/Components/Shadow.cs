using UnityEngine;

public class Shadow : MonoBehaviour
{
    public Follow Follow;

    public void Setup(Vector2 size, Transform target)
    {
        transform.localScale = new Vector3(size.x * 0.5f, size.y * 0.5f, 1);
        Follow.Target = target;
    }
}
