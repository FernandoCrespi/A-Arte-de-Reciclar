using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class Saude : MonoBehaviour
{
    [Header("Configurações")]
    public int saudeMaxima = 100;
    public int saudeAtual;
    public bool morto { get; private set; }

    [Header("UI")]
    public TMP_Text textoVida;

    [Header("Telas")]
    public GameObject telaGameOver;

    [Header("Dano Visual")]
    public float tempoPiscar = 0.1f;
    public int quantidadePiscar = 3;

    [Header("Audio")]
    public AudioSource musicaFundo;
    public AudioClip somMorte;
    public float volumeMorte = 1f;
    public float iniciarEm = 0f;
    public AudioMixerGroup mixerGroup;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        morto = false;
        saudeAtual = saudeMaxima;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        AtualizarUI();
        if (telaGameOver != null) telaGameOver.SetActive(false);
    }

    void AtualizarUI()
    {
        if (textoVida != null)
            textoVida.text = saudeAtual.ToString();
    }

    public void dano(int quantidade)
    {
        if (morto) return;
        saudeAtual -= quantidade;
        saudeAtual = Mathf.Max(saudeAtual, 0);
        AtualizarUI();
        if (gameObject.CompareTag("Player"))
            StartCoroutine(PiscarVermelho());
        if (saudeAtual <= 0)
            Morrer();
    }

    public void danoMax()
    {
        if (morto) return;
        saudeAtual = 0;
        AtualizarUI();
        Morrer();
    }

    IEnumerator PiscarVermelho()
    {
        for (int i = 0; i < quantidadePiscar; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(tempoPiscar);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(tempoPiscar);
        }
    }

    void Morrer()
    {
        morto = true;

        if (musicaFundo != null)
            musicaFundo.Stop();

        if (somMorte != null)
        {
            AudioSource audioTemp = gameObject.AddComponent<AudioSource>();
            audioTemp.clip = somMorte;
            audioTemp.volume = volumeMorte;
            audioTemp.spatialBlend = 0f;
            audioTemp.time = iniciarEm;
            audioTemp.ignoreListenerPause = true;
            audioTemp.outputAudioMixerGroup = mixerGroup;
            audioTemp.Play();
            Destroy(audioTemp, somMorte.length - iniciarEm);
        }

        if (animator != null)
            animator.SetTrigger("Morte");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && !(script is DestrutorPorTempo))
                script.enabled = false;
        }

        GetComponent<DestrutorPorTempo>()?.IniciarDestruicao();

        if (gameObject.CompareTag("Player"))
            StartCoroutine(MorrerPlayer());
    }

    IEnumerator MorrerPlayer()
    {
        yield return new WaitForSeconds(1f);
        if (telaGameOver != null)
            telaGameOver.SetActive(true);
        Time.timeScale = 0f;
    }
}