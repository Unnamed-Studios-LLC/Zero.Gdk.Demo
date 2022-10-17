using UnityEngine;

public class SfxLibrary : MonoBehaviour
{
    public static AudioClip[] DeathSfx;
    public static AudioClip[] PickupSfx;
    public static AudioClip[] SplashSfx;
    public static AudioClip[] UpgradeSfx;

    public AudioClip[] DeathSfxRef;
    public AudioClip[] PickupSfxRef;
    public AudioClip[] SplashSfxRef;
    public AudioClip[] UpgradeSfxRef;

    private void Awake()
    {
        DeathSfx = DeathSfxRef;
        PickupSfx = PickupSfxRef;
        SplashSfx = SplashSfxRef;
        UpgradeSfx = UpgradeSfxRef;
    }
}
