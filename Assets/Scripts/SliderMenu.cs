using UnityEngine;
using UnityEngine.UI;

public class SliderMenu : MonoBehaviour
{
    private Slider slider;

    void OnEnable()
    {
        slider = GetComponent<Slider>();

        // Carrega o valor salvo
        float volumeSalvo = PlayerPrefs.GetFloat("Volume", 100f);
        slider.value = volumeSalvo;

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float valor)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.MudarVolume(valor);
    }
}