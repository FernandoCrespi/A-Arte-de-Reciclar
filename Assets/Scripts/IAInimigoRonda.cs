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
    public float alcanceDeteccao = 5f;  // distância para detectar o jogador
    public float intervaloMinAtaque = 2f;  // intervalo mínimo entre explosões
    public float intervaloMaxAtaque = 3f;  // intervalo máximo entre explosões
    public float forcaParaCima = 10f;
    public float espalhamento = 3f;
    public int quantidadePorAtaque = 3;

    private new Transform transform;
    private int i = 0;
    private float proxTempo;
    private bool seMovendo;
    private bool jogadorPerto = false;
    private float timerAtaque = 0f;
    private float intervaloAtual;
    private Animator animator;
    private Saude saude;
    private Transform jogadorTransform;

    void Start()
    {
        transform = inimigo.transform;
        proxTempo = 0f;
        seMovendo = true;
        animator = GetComponent<Animator>();
        saude = GetComponent<Saude>();
        jogadorTransform = GameObject.FindGameObjectWithTag("Player").transform;
        intervaloAtual = ProximoIntervalo();
    }

    void Update()
    {
        if (saude.morto) return;

        // verifica distância do jogador
        float dist = Vector2.Distance(transform.position, jogadorTransform.position);
        jogadorPerto = dist <= alcanceDeteccao;

        if (jogadorPerto)
        {
            // para de andar e conta o timer de ataque
            animator.SetBool("Correndo", false);
            timerAtaque += Time.deltaTime;

            if (timerAtaque >= intervaloAtual)
            {
                timerAtaque = 0f;
                intervaloAtual = ProximoIntervalo(); // próximo intervalo aleatório
                Explodir();
            }
        }
        else
        {
            // jogador longe: faz ronda normalmente
            timerAtaque = 0f;

            if (Time.time >= proxTempo && !seMovendo)
            {
                Vector2 escala = transform.localScale;
                escala.x *= -1;
                transform.localScale = escala;
                seMovendo = true;
            }
            movimenta();
        }
    }

    void movimenta()
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
        for (int j = 0; j < quantidadePorAtaque; j++)
        {
            GameObject p = Instantiate(
                projetilPrefab,
                transform.position,
                Quaternion.identity
            );

            float vx = Random.Range(-espalhamento, espalhamento);
            p.GetComponent<Rigidbody2D>()
             .AddForce(new Vector2(vx, forcaParaCima), ForceMode2D.Impulse);
        }

        animator.SetTrigger("Ataque"); // toque a animação se tiver
    }

    float ProximoIntervalo()
    {
        return Random.Range(intervaloMinAtaque, intervaloMaxAtaque);
    }

    // desenha o alcance de detecção no editor (visual apenas)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcanceDeteccao);
    }
}