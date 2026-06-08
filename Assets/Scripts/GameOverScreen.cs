using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI finalTimeText;
    public GameObject panel;
    public TextMeshProUGUI textoFeedback;

    [Header("Tempos por fase (opcional)")]
    public TextMeshProUGUI textoFase1;
    public TextMeshProUGUI textoFase2;
    public TextMeshProUGUI textoFase3;

    [Header("Botao sair")]
    public Button btnSair;

    public void ShowEndScreen(GameTimer timer)
    {
        timer.StopTimer();

        if (DatabaseManager.Instance != null)
        {
            DatabaseManager.Instance.SalvarTempoFase(3, timer.GetElapsedTime());

            string nome = DatabaseManager.Instance.ObterNome();
            if (nome == "") nome = "TST";

            float f1 = DatabaseManager.Instance.GetFase1();
            float f2 = DatabaseManager.Instance.GetFase2();
            float f3 = timer.GetElapsedTime();

            bool ok = DatabaseManager.Instance.SalvarRegistroFinal();

            if (textoFeedback != null)
                textoFeedback.text = ok
                    ? nome + " SALVO NO RANKING!"
                    : "ERRO AO SALVAR.";

            if (textoFase1 != null) textoFase1.text = "FASE 1: " + Formatar(f1);
            if (textoFase2 != null) textoFase2.text = "FASE 2: " + Formatar(f2);
            if (textoFase3 != null) textoFase3.text = "FASE 3: " + Formatar(f3);

            if (finalTimeText != null)
                finalTimeText.text = "TOTAL: " + Formatar(f1 + f2 + f3);
        }
        else
        {
            Debug.LogWarning("[GameOverScreen] DatabaseManager nao encontrado!");
            if (finalTimeText != null)
                finalTimeText.text = "TOTAL: " + timer.GetFormattedTime();
        }

        if (panel != null)
            panel.SetActive(true);
    }

    private string Formatar(float t)
    {
        int min   = (int)(t % 3600 / 60);
        int sec   = (int)(t % 60);
        int milli = (int)((t % 1) * 100);
        return string.Format("{0:00}:{1:00}.{2:00}", min, sec, milli);
    }
}