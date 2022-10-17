using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioListener AudioListener;
    public Slider Slider;
    private float _volume;

    private void Awake()
    {
        Slider.value = PlayerPrefs.GetFloat("volume", 0.5f);
        SetVolume(Slider.value);
    }

    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat("volume", value);
        _volume = value * value;
        AudioListener.volume = _volume;
    }
}
