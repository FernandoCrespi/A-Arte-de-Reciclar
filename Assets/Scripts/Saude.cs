using System.Collections;
using System.Collections.Generic;
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
    }

    public void dano(int x)
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
    }

    public void danoMax()
    {
        saude = 0;
        AtualizarUI(); // ← ADICIONE
        morto = true;
        animator.SetTrigger("Morte");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb)
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
    }

    IEnumerator morre()
    {
        yield return new WaitForSeconds(2.0f);
        Coletor coletor = GetComponent<Coletor>();
        if (coletor != null) coletor.Resetar();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}