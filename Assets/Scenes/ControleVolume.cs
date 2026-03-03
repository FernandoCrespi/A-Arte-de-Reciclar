using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;

    public void MudarVolume(float sliderValue)
    {
        // Slider vai de 0 a 100
        // Vamos converter para dB manualmente

        float volumeEmDB = Mathf.Lerp(-80f, 0f, sliderValue / 100f);

        mixer.SetFloat("MasterVolume", volumeEmDB);
    }
}
