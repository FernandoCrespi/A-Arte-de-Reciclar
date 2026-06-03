using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer mixer;
    public string nomeSliderMenu = "SliderVolume"; // nome do objeto slider no menu

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float volumeSalvo = PlayerPrefs.GetFloat("Volume", 100f);
        AplicarVolume(volumeSalvo);
        AtualizarSlider(volumeSalvo);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        float volumeSalvo = PlayerPrefs.GetFloat("Volume", 100f);
        AplicarVolume(volumeSalvo);
        AtualizarSlider(volumeSalvo);
    }

    private void AtualizarSlider(float valor)
    {
        // Acha o slider pelo nome na cena atual e sincroniza
        GameObject sliderObj = GameObject.Find(nomeSliderMenu);
        if (sliderObj != null)
        {
            Slider slider = sliderObj.GetComponent<Slider>();
            if (slider != null)
                slider.value = valor;
        }
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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}