using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class Coletor : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text textoUI;

    [Header("Meta")]
    public int moedasParaVencer = 3;

    [Header("Telas")]
    public GameObject telaVitoria;

    [Header("Audio")]
    public AudioClip somMoeda;
    public float volumeMoeda = 1f;
    public AudioMixerGroup mixerGroup;

    private int total = 0;

    void Start()
    {
        Resetar();
        AtualizarUI();
        if (telaVitoria != null) telaVitoria.SetActive(false);
    }

    public void Coletar()
    {
        total++;
        PlayerPrefs.SetInt("moedas", total);
        AtualizarUI();

        if (somMoeda != null)
        {
            AudioSource audioTemp = gameObject.AddComponent<AudioSource>();
            audioTemp.clip = somMoeda;
            audioTemp.volume = volumeMoeda;
            audioTemp.spatialBlend = 0f;
            audioTemp.outputAudioMixerGroup = mixerGroup;
            audioTemp.Play();
            Destroy(audioTemp, somMoeda.length);
        }

        if (total >= moedasParaVencer)
        {
            if (telaVitoria != null)
                telaVitoria.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Resetar()
    {
        total = 0;
        PlayerPrefs.SetInt("moedas", 0);
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (textoUI != null)
            textoUI.text = ": " + total;
    }
}