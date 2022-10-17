using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    public Movement Movement;

    private Vector2 GetHeading()
    {
        var heading = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            heading.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            heading.x += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            heading.y -= 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            heading.y += 1;
        }

        if (heading.sqrMagnitude == 0)
        {
            return Vector2.zero;
        }

        return heading.normalized;
    }

    private void Update()
    {
        Movement.Heading = GetHeading();
    }
}
