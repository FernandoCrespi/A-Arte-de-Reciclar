using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAInimigoRonda : MonoBehaviour
{
    [Header("Ronda")]
    public GameObject inimigo;
    public GameObject[] pontos;
    public float velocidade = 5f;
    public float espera = 0f;
    public bool loop = true;

    [Header("Vulcão")]
    public GameObject projetilPrefab;
    public float alcanceDeteccao = 5f;
    public float intervaloMinAtaque = 2f;
    public float intervaloMaxAtaque = 3f;
    public float forcaParaCima = 10f;
    public float espalhamento = 3f;
    public int quantidadePorAtaque = 3;

    private int i = 0;
    private float proxTempo;
    private bool seMovendo;
    private float timerAtaque = 0f;
    private float intervaloAtual;
    private Animator animator;
    private Saude saude;
    private Transform jogadorTransform;

    void Start()
    {
        proxTempo = 0f;
        seMovendo = true;
        animator = GetComponent<Animator>();
        saude = GetComponent<Saude>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            jogadorTransform = player.transform;

        intervaloAtual = ProximoIntervalo();
    }

    void Update()
    {
        if (saude != null && saude.morto) return;
        if (jogadorTransform == null) return;

        float dist = Vector2.Distance(transform.position, jogadorTransform.position);
        bool jogadorPerto = dist <= alcanceDeteccao;

        if (jogadorPerto)
        {
            animator.SetBool("Correndo", false);
            timerAtaque += Time.deltaTime;

            if (timerAtaque >= intervaloAtual)
            {
                timerAtaque = 0f;
                intervaloAtual = ProximoIntervalo();
                Explodir();
            }
        }
        else
        {
            timerAtaque = 0f;

            if (Time.time >= proxTempo && !seMovendo)
            {
                Vector2 escala = transform.localScale;
                escala.x *= -1;
                transform.localScale = escala;
                seMovendo = true;
            }

            Movimenta();
        }
    }

    void Movimenta()
    {
        if (pontos.Length == 0 || !seMovendo) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            pontos[i].transform.position,
            velocidade * Time.deltaTime
        );

        animator.SetBool("Correndo", true);

        if (Vector3.Distance(pontos[i].transform.position, transform.position) <= 0.1f)
        {
            i++;
            proxTempo = Time.time + espera;
            seMovendo = false;
            animator.SetBool("Correndo", false);

            if (i >= pontos.Length)
                i = loop ? 0 : i - 1;
        }
    }

    void Explodir()
    {
        if (projetilPrefab == null) return;

        for (int j = 0; j < quantidadePorAtaque; j++)
        {
            GameObject p = Instantiate(
                projetilPrefab,
                transform.position,
                Quaternion.identity
            );

            ProjetilDano projetil = p.GetComponent<ProjetilDano>();
            if (projetil != null)
                projetil.SetDono(gameObject);

            Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float vx = Random.Range(-espalhamento, espalhamento);
                rb.AddForce(new Vector2(vx, forcaParaCima), ForceMode2D.Impulse);
            }
        }

        if (animator != null)
            animator.SetTrigger("Ataque");
    }

    float ProximoIntervalo()
    {
        return Random.Range(intervaloMinAtaque, intervaloMaxAtaque);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (transform != null)
            Gizmos.DrawWireSphere(transform.position, alcanceDeteccao);
    }
}