using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class ObliqueCamera : MonoBehaviour
{
    private const float Angle = 180.0f;
    private const float ZScale = 1;

    private Camera _mainCamera;

    private int _applied;
    private Vector2 _syncedScreenSize;
    private float _syncedSize;

    public Vector2 ScreenToWorldPoint(Vector2 screenPoint)
    {
        var relativeToCenter = screenPoint - new Vector2(Screen.width, Screen.height) / 2f;
        var scalar = _mainCamera.orthographicSize / (Screen.height * 0.5f);
        var position = (Vector2)transform.position;
        return position + relativeToCenter * scalar;
    }

    private void ApplyOblique()
    {
        if (_mainCamera == null)
        {
            return;
        }

        var zOffset = transform.position.z;
        _mainCamera.orthographic = true;
        var orthoHeight = _mainCamera.orthographicSize;
        var orthoWidth = _mainCamera.aspect * orthoHeight;
        var m = Matrix4x4.Ortho(-orthoWidth, orthoWidth, -orthoHeight, orthoHeight, _mainCamera.nearClipPlane, _mainCamera.farClipPlane);
        var s = ZScale / orthoHeight;
        m[0, 2] = +s * Mathf.Sin(Mathf.Deg2Rad * -Angle);
        m[1, 2] = -s * Mathf.Cos(Mathf.Deg2Rad * -Angle);
        m[0, 3] = -zOffset * m[0, 2];
        m[1, 3] = -zOffset * m[1, 2];
        _mainCamera.projectionMatrix = m;
    }

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _mainCamera.transparencySortMode = TransparencySortMode.CustomAxis;
        _mainCamera.transparencySortAxis = _mainCamera.transform.up;
    }

    private void OnEnable()
    {
        ApplyOblique();
        _applied = 0;
    }

    private void LateUpdate()
    {
        var screenSize = new Vector2(Screen.width, Screen.height);
        if (_applied >= 3 &&
            _syncedScreenSize == screenSize &&
            _syncedSize == _mainCamera.orthographicSize)
        {
            return;
        }
        _applied++;
        _syncedScreenSize = screenSize;
        _syncedSize = _mainCamera.orthographicSize;

        ApplyOblique();
    }
}
