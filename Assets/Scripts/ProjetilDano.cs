using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjetilDano : MonoBehaviour
{
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
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Ignora colisão com quem lançou o projétil
        if (dono != null && col.gameObject == dono) return;

        if (col.gameObject.CompareTag("Player"))
        {
            Saude saude = col.gameObject.GetComponent<Saude>();
            if (saude != null)
                saude.dano(dano);

            Destroy(gameObject);
        }
        else if (!col.gameObject.CompareTag("Inimigo"))
        {
            Destroy(gameObject);
        }
    }
}