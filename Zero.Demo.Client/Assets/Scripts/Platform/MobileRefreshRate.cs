using UnityEngine;

public class MobileRefreshRate : MonoBehaviour
{
    private void Awake()
    {
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            return;
        }
        Application.targetFrameRate = 60;
    }
}
