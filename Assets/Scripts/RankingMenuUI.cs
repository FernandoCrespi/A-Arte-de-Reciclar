using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingMenuUI : MonoBehaviour
{
    [Header("Painel")]
    public GameObject painelRanking;

    [Header("Texto do ranking")]
    public TMP_Text textoRanking;

    [Header("Quantos registros mostrar")]
    public int limite = 10;

    public void AbrirRanking()
    {
        if (painelRanking == null) return;
        painelRanking.SetActive(true);
        PreencherLista();
    }

    public void FecharRanking()
    {
        if (painelRanking != null)
            painelRanking.SetActive(false);
    }

    private void PreencherLista()
    {
        if (textoRanking == null) return;

        if (DatabaseManager.Instance == null)
        {
            textoRanking.text = "Banco de dados não encontrado.";
            return;
        }

        List<EntradaRanking> lista = DatabaseManager.Instance.ObterRanking(limite);

        if (lista == null || lista.Count == 0)
        {
            textoRanking.text = "Nenhum registro ainda.";
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("#   NOME  F1          F2          F3          TOTAL");
        sb.AppendLine("────────────────────────────────────────────────────");

        for (int i = 0; i < lista.Count; i++)
        {
            EntradaRanking e = lista[i];
            sb.AppendLine(string.Format("{0,-3} {1,-4}  {2,8}    {3,8}    {4,8}    {5,8}",
                i + 1, e.Nome,
                Formatar(e.Fase1),
                Formatar(e.Fase2),
                Formatar(e.Fase3),
                Formatar(e.Total)));
        }

        textoRanking.text = sb.ToString();
    }

    private string Formatar(float t)
    {
        int min   = (int)(t % 3600 / 60);
        int sec   = (int)(t % 60);
        int milli = (int)((t % 1) * 100);
        return string.Format("{0:00}:{1:00}.{2:00}", min, sec, milli);
    }
}
