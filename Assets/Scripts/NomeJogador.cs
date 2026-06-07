using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Coloque este script no Canvas da cena Inicio.
/// Captura o nome do jogador antes de começar o jogo.
/// </summary>
public class NomeJogador : MonoBehaviour
{
    [Header("Input do nome (3 letras)")]
    public TMP_InputField inputNome;

    [Header("Botão Jogar")]
    public Button btnJogar;

    [Header("Feedback")]
    public TMP_Text textoFeedback;

    [Header("Cena para carregar")]
    public string nomeCena = "Fase1";

    void Start()
    {
        if (inputNome != null)
        {
            inputNome.characterLimit = 3;
            inputNome.onValueChanged.AddListener(v => inputNome.SetTextWithoutNotify(v.ToUpper()));
        }

        if (textoFeedback != null)
            textoFeedback.text = "";
    }

    public void OnJogar()
    {
        string nome = inputNome != null ? inputNome.text.Trim() : "";

        if (nome.Length == 0)
        {
            if (textoFeedback != null) textoFeedback.text = "⚠ Digite seu nome (1-3 letras)!";
            return;
        }

        // Salva o nome no DatabaseManager
        if (DatabaseManager.Instance != null)
            DatabaseManager.Instance.DefinirNome(nome);
        else
            Debug.LogWarning("[NomeJogador] DatabaseManager não encontrado!");

        Debug.Log("[NomeJogador] Nome definido: " + nome);

        // Troca de cena aqui, não pelo botão
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCena);
    }
}