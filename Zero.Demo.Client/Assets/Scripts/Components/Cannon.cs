using UnityEngine;

public class Cannon : Component
{
    private Vector3 _origin;
    private Vector3 _target;
    private float _startTime;
    private float _endTime;
    private float _duration;
    private float _maxHeight;
    private Shadow _shadow;

    public void Setup(Vector3 origin, Vector2 target, float duration, World world)
    {
        transform.position = origin;
        _origin = origin;
        _target = target;
        _startTime = Time.time;

        _duration = duration;
        _endTime = _startTime + _duration;
        _maxHeight = _duration * 2;

        _shadow = ComponentLibrary.Get<Shadow>();
        _shadow.Setup(new Vector2(2, 2), transform);

        World = world;
    }

    private void LateUpdate()
    {
        if (Time.time > _endTime)
        {
            var splash = ComponentLibrary.Get<SplashEffect>();
            splash.transform.position = _target;
            splash.Play();

            ComponentLibrary.Return(_shadow);
            _shadow = null;

            ComponentLibrary.Return(this);

            if (SfxLibrary.SplashSfx.Length != 0)
            {
                World.AudioSource.PlayOneShot(SfxLibrary.SplashSfx.Random(), World.GetVolumeFromPlayer(_target) * 0.6f);
            }
            return;
        }

        var delta = Time.time - _startTime;
        var step = delta / _duration;
        var position = _origin + (_target - _origin) * step;
        var x = step * 2 - 1;
        var heightStep = x * x * -1 + 1;
        position.z -= heightStep * _maxHeight;
        transform.position = position;
    }
}
