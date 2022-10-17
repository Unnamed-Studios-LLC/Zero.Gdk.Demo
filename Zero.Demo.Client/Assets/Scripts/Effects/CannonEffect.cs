using UnityEngine;

public class CannonEffect : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    public void Play()
    {
        var emission = ParticleSystem.emission;
        var burst = emission.GetBurst(0);
        burst.time = Random.value * 0.4f;
        emission.SetBurst(0, burst);

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
