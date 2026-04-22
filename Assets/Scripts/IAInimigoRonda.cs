using System.Collections;
using UnityEngine;

public class IAInimigoRonda : MonoBehaviour
{
    [Header("Pontos de Ronda")]
    public Transform[] pontos;

    [Header("Movimento")]
    public float velocidade = 4f;
    public float tempoEspera = 1f;
    public bool loop = true;

    [Header("Combate")]
    public float distanciaAtaque = 3f;

    private int indice = 0;
    private bool podeMover = true;

    private Rigidbody2D rb;
    private Animator animator;
    private Saude saude;
    private DashInimigo dash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        saude = GetComponent<Saude>();
        dash = GetComponent<DashInimigo>();
    }

    void Update()
    {
        if (saude == null || saude.morto) return;

        if (dash != null && dash.estaDashando)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Movimentar();
    }

    void Movimentar()
    {
        if (pontos == null || pontos.Length == 0 || !podeMover) return;

        // 🔥 PROTEÇÃO CONTRA ERRO
        if (indice >= pontos.Length || pontos[indice] == null)
        {
            Debug.LogWarning("⚠️ Ponto inválido ou deletado!");

            indice = 0;
            return;
        }

        Transform alvo = pontos[indice];

        Vector2 direcao = (alvo.position - transform.position).normalized;

        // só eixo X
        rb.velocity = new Vector2(direcao.x * velocidade, rb.velocity.y);

        if (animator) animator.SetBool("Correndo", true);

        // virar sprite
        if (direcao.x != 0)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Sign(direcao.x) * Mathf.Abs(escala.x);
            transform.localScale = escala;
        }

        // chegou no ponto
        if (Vector2.Distance(transform.position, alvo.position) < 0.2f)
        {
            rb.velocity = Vector2.zero;

            if (animator) animator.SetBool("Correndo", false);

            indice++;

            if (indice >= pontos.Length)
            {
                if (loop) indice = 0;
                else
                {
                    podeMover = false;
                    return;
                }
            }

            StartCoroutine(Esperar());
        }
    }

    IEnumerator Esperar()
    {
        podeMover = false;
        yield return new WaitForSeconds(tempoEspera);
        podeMover = true;
    }

    private void OnTriggerStay2D(Collider2D outro)
    {
        if (!outro.CompareTag("Player")) return;

        float distancia = Vector2.Distance(transform.position, outro.transform.position);

        if (dash != null && dash.podeDash && distancia <= distanciaAtaque)
        {
            Vector2 direcao = (outro.transform.position - transform.position).normalized;
            dash.IniciaDash(direcao);
        }
    }
}