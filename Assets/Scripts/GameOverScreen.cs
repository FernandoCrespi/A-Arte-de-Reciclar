using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Coloque na tela de fim de jogo (última fase).
/// Quando ShowEndScreen() for chamado, para o timer,
/// salva o tempo da fase 2 e mostra o painel com input de nome.
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI finalTimeText;
    public GameObject panel;

    [Header("Input do nome (3 letras)")]
    public TMP_InputField inputNome;
    public Button btnSalvar;
    public TextMeshProUGUI textoFeedback;

    private GameTimer timerAtual;

    // ?? Chame isso quando o jogador zerar o jogo ??????????
    public void ShowEndScreen(GameTimer timer)
    {
        timerAtual = timer;
        timer.StopTimer();

        // Salva o tempo da fase 2 (última fase) no DatabaseManager
        if (DatabaseManager.Instance != null)
            DatabaseManager.Instance.SalvarTempoFase(2, timer.GetElapsedTime());
        else
            Debug.LogWarning("[GameOverScreen] DatabaseManager não encontrado!");

        finalTimeText.text = "Seu tempo: " + timer.GetFormattedTime();
        panel.SetActive(true);

        if (inputNome != null)
        {
            inputNome.characterLimit = 3;
            inputNome.onValueChanged.AddListener(v => inputNome.SetTextWithoutNotify(v.ToUpper()));
        }

        btnSalvar?.onClick.AddListener(OnSalvar);
    }

    // ?? Botão Salvar ??????????????????????????????????????
    private void OnSalvar()
    {
        string nome = inputNome != null ? inputNome.text.Trim() : "";

        if (nome.Length == 0)
        {
            if (textoFeedback != null) textoFeedback.text = "? Digite um nome (1-3 letras)!";
            return;
        }

        if (DatabaseManager.Instance == null)
        {
            if (textoFeedback != null) textoFeedback.text = "? Erro: banco não encontrado.";
            return;
        }

        bool ok = DatabaseManager.Instance.SalvarRegistroFinal(nome);
        if (textoFeedback != null)
            textoFeedback.text = ok
                ? "? " + nome + " salvo no ranking!"
                : "? Erro ao salvar.";
    }
}