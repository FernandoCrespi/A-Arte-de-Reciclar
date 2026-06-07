using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Coloque na tela de fim de jogo (Fase2).
/// Quando ShowEndScreen() for chamado, para o timer,
/// salva o tempo da fase 2 e registra tudo automaticamente.
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI finalTimeText;
    public GameObject panel;
    public TextMeshProUGUI textoFeedback;

    // ── Chame isso quando o jogador zerar a Fase 2 ────────
    public void ShowEndScreen(GameTimer timer)
    {
        timer.StopTimer();

        if (DatabaseManager.Instance != null)
        {
            DatabaseManager.Instance.SalvarTempoFase(2, timer.GetElapsedTime());
            bool ok = DatabaseManager.Instance.SalvarRegistroFinal();

            if (textoFeedback != null)
                textoFeedback.text = ok
                    ? "✔ " + DatabaseManager.Instance.ObterNome() + " salvo no ranking!"
                    : "✖ Erro ao salvar.";
        }
        else
        {
            Debug.LogWarning("[GameOverScreen] DatabaseManager não encontrado!");
            if (textoFeedback != null)
                textoFeedback.text = "✖ Erro: banco não encontrado.";
        }

        if (finalTimeText != null)
            finalTimeText.text = "Seu tempo: " + timer.GetFormattedTime();

        if (panel != null)
            panel.SetActive(true);
    }
}