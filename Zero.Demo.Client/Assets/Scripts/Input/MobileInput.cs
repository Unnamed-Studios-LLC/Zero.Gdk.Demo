using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour
{
    public Joystick AimJoystick;
    public Joystick MoveJoystick;
    public Aim Aim;
    public Movement Movement;
    public World World;
    public Sprite AimOnSprite;
    public Sprite AimOffSprite;
    public Image AimImage;
    public SpriteRenderer Direction;

    public void ToggleAiming()
    {
        Aim.Aiming = !Aim.Aiming;
    }

    private void LateUpdate()
    {
        Movement.Heading = MoveJoystick.Active ? MoveJoystick.Vector : Vector2.zero;
        Direction.enabled = World.Controlled != null && MoveJoystick.Active;
        if (World.Controlled != null)
        {
            Aim.Coordinates = (Vector2)World.Controlled.position + AimJoystick.Vector * World.CurrentRange;
        }
        AimImage.sprite = Aim.Aiming ? AimOnSprite : AimOffSprite;
    }
}
