using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI textoFeedback;

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
                textoFeedback.text = ok ? nome + " SALVO NO RANKING!" : "ERRO AO SALVAR.";

            if (finalTimeText != null)
                finalTimeText.text = Formatar(f1 + f2 + f3);
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
        int min = (int)(t % 3600 / 60);
        int sec = (int)(t % 60);
        int milli = (int)((t % 1) * 100);
        return string.Format("{0:00}:{1:00}.{2:00}", min, sec, milli);
    }
}