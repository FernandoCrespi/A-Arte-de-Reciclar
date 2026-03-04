using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer mixer;
    public Slider sliderVolume; //  ARRASTA O SLIDER AQUI NO INSPECTOR

    private void Awake()
    {
        // Singleton pra não duplicar ao trocar de cena
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float volumeSalvo = PlayerPrefs.GetFloat("Volume", 100f);

        // Atualiza o slider visualmente
        if (sliderVolume != null)
            sliderVolume.value = volumeSalvo;

        AplicarVolume(volumeSalvo);
    }

    public void MudarVolume(float sliderValue)
    {
        AplicarVolume(sliderValue);

        PlayerPrefs.SetFloat("Volume", sliderValue);
        PlayerPrefs.Save();
    }

    private void AplicarVolume(float sliderValue)
    {
        float volumeEmDB = Mathf.Lerp(-80f, 0f, sliderValue / 100f);
        mixer.SetFloat("MasterVolume", volumeEmDB);
    }
}
