using System.Drawing;
using UnityEngine;

public class Coletavel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            outro.GetComponent<Coletor>().Coletar();
            Destroy(gameObject);
        }
    }
}