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
    public float alcanceDeteccao = 6f;
    public float intervaloMinAtaque = 2f;
    public float intervaloMaxAtaque = 4f;
    public float forcaParaCima = 12f;
    public float espalhamento = 4f;
    public int quantidadePorAtaque = 5;

    // Referências privadas
    private int indiceAtual = 0;
    private bool aguardando = false;
    private float timerAtaque = 0f;
    private float intervaloAtual;
    private Transform jogadorTransform;
    private Animator animator;
    private Saude saude;

    void Start()
    {
        animator = GetComponent<Animator>();
        saude = GetComponent<Saude>();

        // Busca o player pela tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            jogadorTransform = player.transform;
        else
            Debug.LogWarning("[IAInimigoRonda] Player não encontrado! Verifique a tag 'Player'.");

        intervaloAtual = ProximoIntervalo();
    }

    void Update()
    {
        // Para tudo se estiver morto
        if (saude != null && saude.morto) return;

        // Sem player, só patrulha
        if (jogadorTransform == null)
        {
            Patrulhar();
            return;
        }

        float distancia = Vector2.Distance(transform.position, jogadorTransform.position);

        if (distancia <= alcanceDeteccao)
        {
            // Player detectado → para e ataca
            animator.SetBool("Correndo", false);

            timerAtaque += Time.deltaTime;
            if (timerAtaque >= intervaloAtual)
            {
                timerAtaque = 0f;
                intervaloAtual = ProximoIntervalo();
                AtaqueVulcao();
            }
        }
        else
        {
            // Player longe → patrulha normalmente
            timerAtaque = 0f;
            Patrulhar();
        }
    }

    // ──────────────────────────────────────────
    // PATRULHA
    // ──────────────────────────────────────────
    void Patrulhar()
    {
        if (pontos.Length == 0 || aguardando) return;

        Transform alvo = pontos[indiceAtual];

        // Vira para o lado correto
        VirarParaAlvo(alvo.position);

        // Move em direção ao waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            alvo.position,
            velocidade * Time.deltaTime
        );

        animator.SetBool("Correndo", true);

        // Chegou no waypoint
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
        if (alvoPos.x < transform.position.x)
            escala.x = -Mathf.Abs(escala.x);
        else
            escala.x = Mathf.Abs(escala.x);
        transform.localScale = escala;
    }

    // ──────────────────────────────────────────
    // ATAQUE VULCÃO
    // ──────────────────────────────────────────
    void AtaqueVulcao()
    {
        if (projetilPrefab == null)
        {
            Debug.LogWarning("[IAInimigoRonda] ProjetilPrefab não atribuído no Inspector!");
            return;
        }

        // Dispara animação de ataque
        if (animator != null)
            animator.SetTrigger("Ataque");

        // Lança os projéteis em arco de vulcão
        for (int j = 0; j < quantidadePorAtaque; j++)
        {
            GameObject proj = Instantiate(projetilPrefab, transform.position, Quaternion.identity);

            // Define o dono para não se machucar
            ProjetilDano projetilDano = proj.GetComponent<ProjetilDano>();
            if (projetilDano != null)
                projetilDano.SetDono(gameObject);

            // Aplica física de vulcão
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Distribui os projéteis em leque no eixo X
                float t = quantidadePorAtaque > 1
                    ? (float)j / (quantidadePorAtaque - 1)
                    : 0.5f;

                float vx = Mathf.Lerp(-espalhamento, espalhamento, t);
                float vy = forcaParaCima;

                rb.AddForce(new Vector2(vx, vy), ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogWarning("[IAInimigoRonda] Projétil sem Rigidbody2D! Adicione um Rigidbody2D ao prefab.");
            }
        }
    }

    float ProximoIntervalo()
    {
        return Random.Range(intervaloMinAtaque, intervaloMaxAtaque);
    }

    // Mostra o alcance de detecção no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcanceDeteccao);
    }
}