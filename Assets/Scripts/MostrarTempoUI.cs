using UnityEngine;
using TMPro; // Biblioteca do TextMeshPro

public class MostrarTempoUI : MonoBehaviour
{
    public TextMeshProUGUI textoTempo;

    void Update()
    {
        if (TemporizadorJogo.Instancia != null)
        {
            textoTempo.text = TemporizadorJogo.Instancia.ObterTempoFormatado();
        }
    }
}