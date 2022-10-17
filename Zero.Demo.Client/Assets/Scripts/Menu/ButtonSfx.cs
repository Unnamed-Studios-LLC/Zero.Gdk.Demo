using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSfx : MonoBehaviour, IPointerDownHandler
{
    public AudioSource AudioSource;
    public AudioClip Pressed;
    public float PresedVolume = 1;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (AudioSource == null ||
            Pressed == null)
        {
            return;
        }

        AudioSource.PlayOneShot(Pressed, PresedVolume);
    }
}
