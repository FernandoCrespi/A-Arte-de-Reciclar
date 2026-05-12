using System.Collections;
using UnityEngine;

public class ProjetilDano : MonoBehaviour
{
    [Header("Configurações")]
    public int dano = 10;
    public float tempoDeVida = 5f;

    private GameObject dono;

    public void SetDono(GameObject d)
    {
        dono = d;
    }

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-2, 10), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Ignora colisão com quem lançou o projétil
        if (dono != null && col.gameObject == dono) return;

        // Ignora colisão com outros inimigos
        if (col.gameObject.CompareTag("Inimigo")) return;

        // Causa dano ao player
        if (col.gameObject.CompareTag("Player"))
        {
            Saude saude = col.gameObject.GetComponent<Saude>();
            if (saude != null)
                saude.dano(dano);
        }

        // Destrói o projétil ao colidir com qualquer coisa (exceto inimigos)
        Destroy(gameObject);
    }
}