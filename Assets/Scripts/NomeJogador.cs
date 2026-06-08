using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NomeJogador : MonoBehaviour
{
    [Header("Popup")]
    public GameObject painelPopup;

    [Header("Input do nome (3 letras)")]
    public TMP_InputField inputNome;

    [Header("Botão Confirmar")]
    public Button btnConfirmar;

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

        // Popup começa fechado
        if (painelPopup != null)
            painelPopup.SetActive(false);
    }

    // Chamado pelo botão JOGAR
    public void AbrirPopup()
    {
        if (painelPopup != null)
            painelPopup.SetActive(true);

        if (inputNome != null)
            inputNome.text = "";

        if (textoFeedback != null)
            textoFeedback.text = "";
    }

    // Chamado pelo botão CONFIRMAR
    public void Confirmar()
    {
        string nome = inputNome != null ? inputNome.text.Trim() : "";

        if (nome.Length == 0)
        {
            if (textoFeedback != null) textoFeedback.text = "⚠ Digite seu nome!";
            return;
        }

        if (DatabaseManager.Instance != null)
            DatabaseManager.Instance.DefinirNome(nome);
        else
            Debug.LogWarning("[NomeJogador] DatabaseManager não encontrado!");

        Debug.Log("[NomeJogador] Nome definido: " + nome);

        if (painelPopup != null)
            painelPopup.SetActive(false);

        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCena);
    }
}

