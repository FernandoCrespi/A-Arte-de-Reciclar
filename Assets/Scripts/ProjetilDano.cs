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

    public void SetDono(GameObject d)
    {
        dono = d;
    }

    public void SetVelocidade(float v)
    {
        velocidade = v;
    }

    public void SetDirecao(Vector2 direcao)
    {
        direcaoDefinida = direcao.normalized;
        temDirecao = true;
    }

    void Start()
    {
        Destroy(gameObject, tempoDeVida);

        // ? IGNORA COLISÃO COM O INIMIGO QUE SPAWNOU
        if (dono != null)
        {
            Collider2D donoCollider = dono.GetComponent<Collider2D>();
            Collider2D meuCollider = GetComponent<Collider2D>();
            if (donoCollider != null && meuCollider != null)
                Physics2D.IgnoreCollision(meuCollider, donoCollider);
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) { Debug.LogError("SEM RIGIDBODY!"); return; }

        if (temDirecao)
        {
            rb.velocity = direcaoDefinida * velocidade;
            rb.gravityScale = 2f;
        }
        else
        {
            rb.velocity = new Vector2(0, 10f);
            rb.gravityScale = 2f;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (dono != null && col.gameObject == dono) return;
        if (col.gameObject.CompareTag("Inimigo")) return;

        if (col.gameObject.CompareTag("Player"))
        {
            Saude saude = col.gameObject.GetComponent<Saude>();
            if (saude != null)
                saude.dano(dano);
        }

        Destroy(gameObject);
    }
}