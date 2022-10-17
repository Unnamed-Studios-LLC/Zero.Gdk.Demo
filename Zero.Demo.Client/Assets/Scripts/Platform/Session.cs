using UnityEngine;

[ExecuteInEditMode]
public class Session : MonoBehaviour
{
    public static DeviceType DeviceType;

    public bool OverrideDeviceType = false;
    public DeviceType DeviceTypeOverride;

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (!OverrideDeviceType)
        {
            DeviceType = SystemInfo.deviceType;
            return;
        }
        DeviceType = DeviceTypeOverride;
#else
        DeviceType = SystemInfo.deviceType;
#endif
    }
}
