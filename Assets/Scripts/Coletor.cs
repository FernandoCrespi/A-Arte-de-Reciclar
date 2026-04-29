using UnityEngine;
using TMPro;

public class Coletor : MonoBehaviour
{
    public TMP_Text textoUI;
    private int total = 0;

    void Start()
    {
        Resetar();
        AtualizarUI();
    }

    public void Coletar()
    {
        total++;
        PlayerPrefs.SetInt("moedas", total);
        AtualizarUI();
    }
    public void Resetar()
    {
        total = 0;
        PlayerPrefs.SetInt("moedas", 0);
        AtualizarUI();
    }
    void AtualizarUI()
    {
        if (textoUI != null)
            textoUI.text = "Moedas: " + total;
    }
}