using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Coloque este script no Prefab de cada linha do ranking.
///
/// Hierarquia do Prefab "LinhaRanking":
///
/// LinhaRanking  (Image de fundo + CanvasGroup + este script)
/// ├── TextoPosicao   ← TMP_Text  ex: "1"
/// ├── TextoNome      ← TMP_Text  ex: "AAA"
/// ├── TextoFase1     ← TMP_Text  ex: "01:23.45"
/// ├── TextoFase2     ← TMP_Text  ex: "02:10.00"
/// └── TextoTotal     ← TMP_Text  ex: "03:33.45"
/// </summary>
public class LinhaRanking : MonoBehaviour
{
    [Header("Campos de texto")]
    public TMP_Text textoPosicao;
    public TMP_Text textoNome;
    public TMP_Text textoFase1;
    public TMP_Text textoFase2;
    public TMP_Text textoTotal;

    [Header("Destaque (ouro/prata/bronze)")]
    public Image fundoLinha;
    public Color corOuro = new Color(1f, 0.84f, 0f, 1f);
    public Color corPrata = new Color(0.75f, 0.75f, 0.75f, 1f);
    public Color corBronze = new Color(0.8f, 0.5f, 0.2f, 1f);
    public Color corNormal = new Color(1f, 1f, 1f, 0.05f);

    // ── API pública ────────────────────────────────────────
    public void Preencher(int posicao, string nome, float fase1, float fase2, float total)
    {
        if (textoPosicao != null) textoPosicao.text = posicao.ToString();
        if (textoNome != null) textoNome.text = nome;
        if (textoFase1 != null) textoFase1.text = Formatar(fase1);
        if (textoFase2 != null) textoFase2.text = Formatar(fase2);
        if (textoTotal != null) textoTotal.text = Formatar(total);

        if (fundoLinha != null)
        {
            switch (posicao)
            {
                case 1: fundoLinha.color = corOuro; break;
                case 2: fundoLinha.color = corPrata; break;
                case 3: fundoLinha.color = corBronze; break;
                default: fundoLinha.color = corNormal; break;
            }
        }
    }

    // ── Formata segundos → MM:SS.ms ───────────────────────
    private string Formatar(float t)
    {
        int min = (int)(t % 3600 / 60);
        int sec = (int)(t % 60);
        int milli = (int)((t % 1) * 100);
        return string.Format("{0:00}:{1:00}.{2:00}", min, sec, milli);
    }
}