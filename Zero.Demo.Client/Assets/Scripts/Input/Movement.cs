using System;
using UnityEngine;
using Zero.Demo.Model.Data;

public class Movement : MonoBehaviour
{
    public DemoGameService Service;
    public Transform Direction;
    private Vector2 _sentHeading;
    
    public Vector2 Heading { get; set; }

    private void FixedUpdate()
    {
        UpdateHeading();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var data = new ChangeShipInputData();
            Service.Push(ref data);
        }

        Direction.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(Heading.y, Heading.x) * Mathf.Rad2Deg);
    }

    private void UpdateHeading()
    {
        if (_sentHeading == Heading)
        {
            return;
        }

        var data = new HeadingInputData(Heading.ToZero());
        Service.Push(ref data);

        _sentHeading = Heading;
    }
}
