using UnityEngine;
using UnityEngine.UI;

public class SliderPause : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        // Sincroniza com o volume salvo
        float volumeSalvo = PlayerPrefs.GetFloat("Volume", 100f);
        slider.value = volumeSalvo;

        // Quando o slider mudar chama o AudioManager
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float valor)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.MudarVolume(valor);
    }
}