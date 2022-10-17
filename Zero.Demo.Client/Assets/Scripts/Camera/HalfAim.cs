using UnityEngine;

public class HalfAim : MonoBehaviour
{
    public World World;
    public Aim Aim;

    private void Update()
    {
        if (World.Controlled == null)
        {
            return;
        }

        var controlled = (Vector2)World.Controlled.position;
        transform.position = controlled + (Aim.ClippedCoordinates - controlled) * 0.5f;
    }
}
