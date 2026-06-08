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

    [Header("Game Over Screen (só na Fase 3)")]
    public GameOverScreen gameOverScreen;
    public GameTimer gameTimer;

    [Header("Audio")]
    public AudioSource musicaFundo;
    public AudioClip somMoeda;
    public float volumeMoeda = 1f;
    public AudioClip musicaVitoria;
    public float volumeVitoria = 1f;
    public AudioMixerGroup mixerGroup;

    private int total = 0;

    void Start()
    {
        Resetar();
        AtualizarUI();
        if (telaVitoria != null) telaVitoria.SetActive(false);

        if (gameTimer == null)
            gameTimer = Object.FindFirstObjectByType<GameTimer>();
        if (gameOverScreen == null)
            gameOverScreen = Object.FindFirstObjectByType<GameOverScreen>();
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
            // Só salva no banco na Fase 3
            if (gameOverScreen != null && gameTimer != null)
                gameOverScreen.ShowEndScreen(gameTimer);
            else if (telaVitoria != null)
                telaVitoria.SetActive(true);

            if (musicaFundo != null)
                musicaFundo.Stop();

            if (musicaVitoria != null)
            {
                AudioSource audioVitoria = gameObject.AddComponent<AudioSource>();
                audioVitoria.clip = musicaVitoria;
                audioVitoria.volume = volumeVitoria;
                audioVitoria.spatialBlend = 0f;
                audioVitoria.ignoreListenerPause = true;
                audioVitoria.outputAudioMixerGroup = mixerGroup;
                audioVitoria.Play();
            }

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