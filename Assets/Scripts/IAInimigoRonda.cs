using System.Collections;
using UnityEngine;

public class IAInimigoRonda : MonoBehaviour
{
    [Header("Patrulha")]
    public Transform[] pontos;
    public float velocidade = 3f;
    public float esperaEntreWaypoints = 1f;
    public bool loop = true;

    [Header("Ataque Vulcão")]
    public GameObject projetilPrefab;
    public float alcanceDeteccao = 8f;
    public float intervaloAtaque = 2f;
    public int quantidadePorAtaque = 8;
    public float espalhamento = 30f;
    public float velocidadeProjeto = 15f;

    private int indiceAtual = 0;
    private bool aguardando = false;
    private bool podeAtacar = true;
    private Transform jogadorTransform;
    private Animator animator;
    private Saude saude;

    void Start()
    {
        animator = GetComponent<Animator>();
        saude = GetComponent<Saude>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            jogadorTransform = player.transform;
        else
            Debug.LogWarning("Player não encontrado!");
    }

    void Update()
    {
        if (saude != null && saude.morto) return;
        if (jogadorTransform == null) { Patrulhar(); return; }

        float distancia = Vector2.Distance(transform.position, jogadorTransform.position);

        if (distancia <= alcanceDeteccao)
        {
            animator.SetBool("Correndo", false);

            if (podeAtacar)
            {
                podeAtacar = false;
                StartCoroutine(CooldownAtaque());
                AtaqueVulcao();
            }
        }
        else
        {
            Patrulhar();
        }
    }

    IEnumerator CooldownAtaque()
    {
        yield return new WaitForSeconds(intervaloAtaque);
        podeAtacar = true;
    }

    void Patrulhar()
    {
        if (pontos.Length == 0 || aguardando) return;
        if (pontos[indiceAtual] == null) return;

        Transform alvo = pontos[indiceAtual];
        VirarParaAlvo(alvo.position);

        transform.position = Vector3.MoveTowards(
            transform.position,
            alvo.position,
            velocidade * Time.deltaTime
        );

        animator.SetBool("Correndo", true);

        if (Vector3.Distance(transform.position, alvo.position) <= 0.05f)
        {
            animator.SetBool("Correndo", false);
            StartCoroutine(AguardarWaypoint());
        }
    }

    IEnumerator AguardarWaypoint()
    {
        aguardando = true;
        yield return new WaitForSeconds(esperaEntreWaypoints);
        indiceAtual++;
        if (indiceAtual >= pontos.Length)
            indiceAtual = loop ? 0 : pontos.Length - 1;
        aguardando = false;
    }

    void VirarParaAlvo(Vector3 alvoPos)
    {
        Vector3 escala = transform.localScale;
        escala.x = alvoPos.x < transform.position.x
            ? -Mathf.Abs(escala.x)
            : Mathf.Abs(escala.x);
        transform.localScale = escala;
    }

    void AtaqueVulcao()
    {
        if (projetilPrefab == null)
        {
            Debug.LogError("PREFAB NULO!");
            return;
        }

        if (animator != null)
            animator.SetTrigger("Ataque");

        float direcaoX = jogadorTransform.position.x > transform.position.x ? 1f : -1f;
        float anguloBase = direcaoX > 0 ? 60f : 120f;

        for (int j = 0; j < quantidadePorAtaque; j++)
        {
            float t = quantidadePorAtaque > 1
                ? (float)j / (quantidadePorAtaque - 1)
                : 0.5f;

            float anguloFinal = anguloBase + Mathf.Lerp(-espalhamento, espalhamento, t);

            Vector2 dir = new Vector2(
                Mathf.Cos(anguloFinal * Mathf.Deg2Rad),
                Mathf.Sin(anguloFinal * Mathf.Deg2Rad)
            );

            // Spawna 1 unidade acima do inimigo
            Vector3 posSpawn = transform.position + new Vector3(0, 1f, 0);
            GameObject proj = Instantiate(projetilPrefab, posSpawn, Quaternion.identity);

            // ← DEBUG
            Debug.Log($"Projétil criado: {proj.name} posição: {proj.transform.position}");

            ProjetilDano pd = proj.GetComponent<ProjetilDano>();

            // ← DEBUG
            Debug.Log($"ProjetilDano encontrado: {pd != null}");

            if (pd != null)
            {
                pd.SetDono(gameObject);
                pd.SetVelocidade(velocidadeProjeto);
                pd.SetDirecao(dir);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcanceDeteccao);
    }
}