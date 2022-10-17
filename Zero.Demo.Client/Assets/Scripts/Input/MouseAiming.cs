using UnityEngine;

public class MouseAiming : MonoBehaviour
{
    public ObliqueCamera Camera;
    public Aim Aim;

    private void LateUpdate()
    {
        Aim.Aiming = Input.GetMouseButton(0);
        Aim.Coordinates = Camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
