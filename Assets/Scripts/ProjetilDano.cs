using UnityEngine;

public class ProjetilDano : MonoBehaviour
{
    [Header("Configurações")]
    public int dano = 10;
    public float tempoDeVida = 5f;
    public float velocidade = 15f;

    private GameObject dono;
    private Vector2 direcaoDefinida;
    private bool temDirecao = false;

    public void SetDono(GameObject d) { dono = d; }
    public void SetVelocidade(float v) { velocidade = v; }
    public void SetDirecao(Vector2 direcao)
    {
        direcaoDefinida = direcao.normalized;
        temDirecao = true;
    }

    void Start()
    {
        Destroy(gameObject, tempoDeVida);

        // Ignora colisão com o inimigo dono
        if (dono != null)
        {
            Collider2D donoCollider = dono.GetComponent<Collider2D>();
            Collider2D meuCollider = GetComponent<Collider2D>();
            if (donoCollider != null && meuCollider != null)
                Physics2D.IgnoreCollision(meuCollider, donoCollider);
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) { Debug.LogError("Projétil sem Rigidbody2D!"); return; }

        // Define a velocidade via Rigidbody
        Vector2 vel = temDirecao ? direcaoDefinida * velocidade : new Vector2(0, 10f);
        rb.velocity = vel;
        rb.gravityScale = 1.5f; // arco bonito
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Ignora o próprio dono
        if (dono != null && col.gameObject == dono) return;
        // Ignora outros inimigos
        if (col.gameObject.CompareTag("Inimigo")) return;

        // Causa dano no Player
        if (col.gameObject.CompareTag("Player"))
        {
            Saude saude = col.gameObject.GetComponent<Saude>();
            if (saude != null)
                saude.dano(dano);
        }

        Destroy(gameObject);
    }
}