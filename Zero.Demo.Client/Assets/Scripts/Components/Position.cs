using UnityEngine;
using Zero.Demo.Model.Data;

public class Position : MonoBehaviour, IDataHandler<PositionData>
{
    private const float LerpScalar = 8;

    public SpriteRenderer SpriteRenderer;

    private Vector2? _receivedCoordinates;
    private Vector2 _trajectory;
    private float _receivedTime;
    private Vector2 _lastCoordinates;

    public Vector2 Coordinates { get; private set; }

    public void Process(ref PositionData data)
    {
        if (_receivedCoordinates == null)
        {
            SetCoordinates(data.Coordinates.ToUnity());
        }
        _receivedCoordinates = data.Coordinates.ToUnity();
        _trajectory = data.Trajectory.ToUnity();
        _receivedTime = Time.time;
    }

    private void Update()
    {
        UpdateCoordinates();
    }

    private void OnEnable()
    {
        _receivedCoordinates = null;
    }

    private void SetCoordinates(Vector2 coordinates)
    {
        Coordinates = coordinates;
        transform.position = coordinates;
    }

    private void UpdateCoordinates()
    {
        if (_receivedCoordinates == null)
        {
            return;
        }

        _lastCoordinates = transform.position;
        var delta = Time.time - _receivedTime;
        var targetCoordinates = _receivedCoordinates.Value + _trajectory * delta;
        var setCoordinates = Vector2.Lerp(Coordinates, targetCoordinates, Time.deltaTime * LerpScalar);
        SetCoordinates(setCoordinates);

        var moveVector = setCoordinates - _lastCoordinates;
        if (Mathf.Abs(moveVector.x) > 0.1f * Time.deltaTime)
        {
            SpriteRenderer.transform.localScale = new Vector3(moveVector.x > 0 ? -1 : 1, 1, 1);
        }
    }
}
