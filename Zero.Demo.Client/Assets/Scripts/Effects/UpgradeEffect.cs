using UnityEngine;

public class UpgradeEffect : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    public void Play()
    {
        ParticleSystem.Simulate(0.01f, true, true);
        ParticleSystem.Play(true);
    }

    private void LateUpdate()
    {
        if (!ParticleSystem.isPlaying)
        {
            ComponentLibrary.Return(this);
        }
    }
}
