using System.Collections;
using UnityEngine;

public class DashInimigo : MonoBehaviour
{
    [Header("Configurações")]
    public float velocidadeDash = 18f;
    public float tempoDash = 0.5f;
    public float tempoCooldown = 2.5f;
    public int danoDash = 30;

    [HideInInspector] public bool estaDashando = false;
    [HideInInspector] public bool podeDash = true;

    private IAInimigoRonda iaRonda;
    private Saude saude;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 direcaoDash;

    void Start()
    {
        iaRonda = GetComponent<IAInimigoRonda>();
        saude = GetComponent<Saude>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void IniciaDash(Vector2 direcao)
    {
        if (!podeDash || estaDashando) return;

        Debug.Log("🚀 DASH INICIADO!");

        podeDash = false;
        estaDashando = true;
        direcaoDash = direcao.normalized;

        animator.SetTrigger("Ataque");
        animator.speed = 1.5f;

        if (rb) rb.velocity = Vector2.zero;
        if (iaRonda) iaRonda.enabled = false;

        StartCoroutine(ExecutaDash());
    }

    IEnumerator ExecutaDash()
    {
        float tempoInicio = Time.time;

        while (Time.time < tempoInicio + tempoDash)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direcaoDash, 0.5f);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                AplicaDano(hit.collider);
                yield break;
            }

            transform.position += (Vector3)(direcaoDash * velocidadeDash * Time.deltaTime);
            yield return null;
        }

        FinalizaDash();
    }

    void FinalizaDash()
    {
        Debug.Log("✅ DASH FINALIZADO!");

        estaDashando = false;
        animator.speed = 1f;

        if (iaRonda) iaRonda.enabled = true;

        StartCoroutine(CooldownDash());
    }

    IEnumerator CooldownDash()
    {
        yield return new WaitForSeconds(tempoCooldown);
        podeDash = true;
    }

    void AplicaDano(Collider2D player)
    {
        Debug.Log($"💥 DANO {danoDash}!");
        Saude s = player.GetComponent<Saude>();
        if (s != null)
        {
            s.dano(danoDash);
            Debug.Log($"❤️ Vida: {s.saude}");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (estaDashando && other.CompareTag("Player"))
            AplicaDano(other);
    }
}