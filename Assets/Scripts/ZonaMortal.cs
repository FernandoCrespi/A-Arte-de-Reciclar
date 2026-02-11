using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaMortal : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // se for o jogador, entao cause sua morte com dano maximo
            other.gameObject.GetComponent<Saude>().danoMax();
        }
        else
        { // se for qualquer outra coisa, como um inimigo caindo por ex, destrua o objeto
            Object.Destroy(other.gameObject);
        }
    }
}