using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Saude : MonoBehaviour
{

    public bool morto;
    public int saude;
    private Animator animator;

    // Use this for initialization
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
        Debug.Log("Entrei dano");
        saude -= x;
        if (saude <= 0)
        {
            morto = true;
            animator.SetTrigger("Morte");
            if (gameObject.tag == "Player")
            {  // Só reicicia a fase se quem morreu foi o jogador.
                StartCoroutine(morre());
            }
        }
    }

    public void danoMax()
    {
        saude = 0;
        morto = true;
        animator.SetTrigger("Morte");
        if (gameObject.tag == "Player")
        {  // Só reicicia a fase se quem morreu foi o jogador.
            StartCoroutine(morre());
        }
    }

    IEnumerator morre()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}