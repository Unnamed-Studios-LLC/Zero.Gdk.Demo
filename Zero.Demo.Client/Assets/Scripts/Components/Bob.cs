using UnityEngine;

public class Bob : MonoBehaviour
{
    private float _offset;

    public float Frequency = 1;
    public float Amplitude = 1;
    public float Offset = 0;

    private void Awake()
    {
        _offset = Random.value * Mathf.PI * 2;
    }

    private void LateUpdate()
    {
        var z = Mathf.Sin(Time.time * Frequency + _offset) * Amplitude + Offset;

        var position = transform.position;
        position.z = z;
        transform.position = position;
    }
}
