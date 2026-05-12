using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saude : MonoBehaviour
{
    [Header("Configurações")]
    public int saudeMaxima = 100;
    public int saudeAtual;
    public bool morto { get; private set; }

    private Animator animator;

    void Start()
    {
        morto = false;
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
        if (morto) return;
        saudeAtual = 0;
        Morrer();
    }

    void Morrer()
    {
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