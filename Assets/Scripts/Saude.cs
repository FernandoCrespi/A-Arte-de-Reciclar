using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // ← ADICIONE ISSO

public class Saude : MonoBehaviour
{
    public bool morto;
    public int saude = 100;
    private int saudeMaxima; // ← ADICIONE
    private Animator animator;

    public TMP_Text textoVida; // ← ADICIONE (arraste o objeto "vida" aqui)
    [Header("Configurações")]
    public int saudeMaxima = 100;
    public int saudeAtual;
    public bool morto { get; private set; }

    private Animator animator;

    [Header("Audio")]
    public AudioClip somMorte;
    public float volumeMorte = 1f;
    public float iniciarEm = 0f;
    private AudioSource audioSource;

    void Start()
    {
        morto = false;
        saudeMaxima = saude; // ← ADICIONE
        animator = gameObject.GetComponent<Animator>();
        AtualizarUI(); // ← ADICIONE
    }

    void AtualizarUI()
    {
        if (textoVida != null)
            textoVida.text = saude.ToString(); 
        saudeAtual = saudeMaxima;
        animator = GetComponent<Animator>();
    }

    // Recebe dano normal
    public void dano(int quantidade)
    {
        if (morto) return;

        saudeAtual -= quantidade;
        saudeAtual = Mathf.Max(saudeAtual, 0); // Nunca vai abaixo de 0

        if (saudeAtual <= 0)
            Morrer();
    }

    // Mata instantaneamente
    public void danoMax()
    {
        saude -= x;
        AtualizarUI(); // ← ADICIONE
        if (saude <= 0)
        {
            morto = true;
            animator.SetTrigger("Morte");
            GetComponent<DestrutorPorTempo>()?.IniciarDestruicao();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.velocity = Vector2.zero;
                rb.simulated = false;
            }
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script != this && script != animator)
                    script.enabled = false;
            }
            if (gameObject.tag == "Player")
                StartCoroutine(morre());
        }
        if (morto) return;
        saudeAtual = 0;
        Morrer();
    }

    void Morrer()
    {
        saude = 0;
        AtualizarUI(); // ← ADICIONE
        morto = true;
        animator.SetTrigger("Morte");
        morto = true;

        // Toca animação de morte
        if (animator != null)
            animator.SetTrigger("Morte");

        // Para o Rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && script != animator && script.GetType() != typeof(DestrutorPorTempo))
                script.enabled = false;
        }
        if (gameObject.tag == "Player")
            StartCoroutine(morre());

        // Desativa todos os scripts exceto este e o DestrutorPorTempo
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && !(script is DestrutorPorTempo))
                script.enabled = false;
        }

        // Inicia o destrutor se existir
        GetComponent<DestrutorPorTempo>()?.IniciarDestruicao();

        // Se for o player, reinicia a cena
        if (gameObject.CompareTag("Player"))
            StartCoroutine(ReiniciarCena());
    }

    IEnumerator ReiniciarCena()
    {
        yield return new WaitForSeconds(2f);

        Coletor coletor = GetComponent<Coletor>();
        if (coletor != null) coletor.Resetar();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}