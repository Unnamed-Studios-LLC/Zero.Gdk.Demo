using UnityEngine;

[ExecuteInEditMode]
public class DeviceOnly : MonoBehaviour
{
    public DeviceType TargetDevice;
    public GameObject[] Objects;

    private void OnEnable()
    {
        if (Objects == null)
        {
            return;
        }
        foreach (var obj in Objects)
        {
            obj.SetActive(Session.DeviceType == TargetDevice);
        }
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        if (Objects == null)
        {
            return;
        }
        foreach (var obj in Objects)
        {
            obj.SetActive(Session.DeviceType == TargetDevice);
        }
    }
#endif
}
