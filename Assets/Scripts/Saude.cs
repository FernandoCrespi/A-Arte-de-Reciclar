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

    void Update()
    {
    }

    public void dano(int x)
    {
        saude -= x;
        if (saude <= 0)
        {
            morto = true;
            animator.SetTrigger("Morte");
            GetComponent<DestrutorPorTempo>()?.IniciarDestruicao();

            // === DESABILITA MOVIMENTO ===
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
                {
                    script.enabled = false;
                }
            }

            if (gameObject.tag == "Player")
            {
                StartCoroutine(morre());
            }
        }
    }

    public void danoMax()
    {
        saude = 0;
        morto = true;
        animator.SetTrigger("Morte");

        // === DESABILITA MOVIMENTO ===
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}