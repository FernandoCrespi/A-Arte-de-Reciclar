using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingMenuUI : MonoBehaviour
{
    [Header("Painel")]
    public GameObject painelRanking;

    [Header("Lista")]
    public Transform containerLinhas;
    public GameObject prefabLinha;
    public TMP_Text textoVazio;

    [Header("Quantos registros mostrar")]
    public int limite = 10;

    // ── ABRIR ──────────────────────────────────────────────
    public void AbrirRanking()
    {
        if (painelRanking == null) return;
        painelRanking.SetActive(true);
        PreencherLista();
    }

    // ── FECHAR ─────────────────────────────────────────────
    public void FecharRanking()
    {
        if (painelRanking != null)
            painelRanking.SetActive(false);
    }

    // ── PREENCHER ──────────────────────────────────────────
    private void PreencherLista()
    {
        // Limpa linhas anteriores
        foreach (Transform filho in containerLinhas)
            Destroy(filho.gameObject);

        // Se não existe DatabaseManager na cena, cria um automaticamente
        if (DatabaseManager.Instance == null)
        {
            Debug.Log("[RankingMenuUI] Criando DatabaseManager automaticamente...");
            GameObject go = new GameObject("DatabaseManager");
            go.AddComponent<DatabaseManager>();
        }

        List<EntradaRanking> lista = DatabaseManager.Instance.ObterRanking(limite);

        if (lista == null || lista.Count == 0)
        {
            MostrarVazio("Nenhum registro ainda.");
            return;
        }

        if (textoVazio != null) textoVazio.gameObject.SetActive(false);

        for (int i = 0; i < lista.Count; i++)
        {
            EntradaRanking e = lista[i];
            GameObject linha = Instantiate(prefabLinha, containerLinhas);
            LinhaRanking comp = linha.GetComponent<LinhaRanking>();
            if (comp != null)
            {
                comp.Preencher(
                    posicao: i + 1,
                    nome: e.Nome,
                    fase1: e.Fase1,
                    fase2: e.Fase2,
                    total: e.Total
                );
            }

            CanvasGroup cg = linha.GetComponent<CanvasGroup>();
            if (cg != null)
                StartCoroutine(AnimarEntrada(cg, i * 0.07f));
        }
    }

    private void MostrarVazio(string msg)
    {
        if (textoVazio != null)
        {
            textoVazio.text = msg;
            textoVazio.gameObject.SetActive(true);
        }
    }

    private IEnumerator AnimarEntrada(CanvasGroup cg, float delay)
    {
        cg.alpha = 0f;
        yield return new WaitForSeconds(delay);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.25f;
            cg.alpha = Mathf.Clamp01(t);
            yield return null;
        }
        cg.alpha = 1f;
    }
}