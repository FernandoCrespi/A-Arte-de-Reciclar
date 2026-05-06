using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saude : MonoBehaviour
{
    public bool morto;
    public int saude = 100;
    private Animator animator;

    void Start()
    {
        morto = false;
        animator = gameObject.GetComponent<Animator>();
    }

    public void dano(int x)
    {
        if (morto) return; // Proteção contra dano duplo

        saude -= x;
        if (saude <= 0)
        {
            Morrer();
        }
    }

    public void danoMax()
    {
        if (morto) return;

        saude = 0;
        Morrer();
    }

    void Morrer()
    {
        morto = true;
        animator.SetTrigger("Morte");
        GetComponent<DestrutorPorTempo>()?.IniciarDestruicao();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            // Cast correto: Animator não é MonoBehaviour
            if (script != this && script.GetType() != typeof(DestrutorPorTempo))
            {
                script.enabled = false;
            }
        }

        if (gameObject.tag == "Player")
        {
            StartCoroutine(morre());
        }
    }

    IEnumerator morre()
    {
        yield return new WaitForSeconds(2.0f);
        Coletor coletor = GetComponent<Coletor>();
        if (coletor != null) coletor.Resetar();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}