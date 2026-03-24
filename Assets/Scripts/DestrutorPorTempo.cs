using System.Collections;
using UnityEngine;

public class DestrutorPorTempo : MonoBehaviour
{
    public float tempoParaSumir = 1.5f;

    public void IniciarDestruicao()
    {
        StartCoroutine(Destruir());
    }

    IEnumerator Destruir()
    {
        yield return new WaitForSeconds(tempoParaSumir);
        Destroy(gameObject);
    }
}
