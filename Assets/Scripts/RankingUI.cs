using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingUI : MonoBehaviour
{
    public static RankingUI Instance { get; private set; }

    [Header("Referências UI")]
    public TMP_InputField inputNome;
    public Button btnSalvar;
    public Button btnVerRanking;
    public Button btnFecharRanking;
    public GameObject painelRanking;
    public TMP_Text textoRanking;
    public TMP_Text textoFeedback;

    [Header("GameTimer (auto-detectado se vazio)")]
    public GameTimer gameTimer;

    private float tempoFase1, tempoFase2;
    private bool fase1Ok, fase2Ok;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (gameTimer == null)
            gameTimer = Object.FindFirstObjectByType<GameTimer>();

        if (gameTimer == null)
            Debug.LogWarning("[RankingUI] GameTimer não encontrado!");

        if (inputNome != null)
        {
            inputNome.characterLimit = 3;
            inputNome.onValueChanged.AddListener(v => inputNome.SetTextWithoutNotify(v.ToUpper()));
        }

        btnSalvar?.onClick.AddListener(OnSalvar);
        btnVerRanking?.onClick.AddListener(OnVerRanking);
        btnFecharRanking?.onClick.AddListener(FecharRanking);

        if (painelRanking != null)
            painelRanking.SetActive(false);

        Feedback("");
    }

    // ── REGISTRAR FIM DE FASE ──────────────────────────────
    public void RegistrarFase(int fase)
    {
        if (gameTimer == null)
        {
            Debug.LogWarning("[RankingUI] GameTimer ausente.");
            return;
        }

        float t = gameTimer.GetElapsedTime();
        gameTimer.StopTimer();

        switch (fase)
        {
            case 1: tempoFase1 = t; fase1Ok = true; break;
            case 2: tempoFase2 = t; fase2Ok = true; break;
            default:
                Debug.LogWarning("[RankingUI] Fase inválida: " + fase);
                return;
        }
        Debug.Log("[RankingUI] Fase " + fase + " registrada: " + t.ToString("F2") + "s");
    }

    // ── BOTÃO SALVAR ───────────────────────────────────────
    public void OnSalvar()
    {
        string nome = inputNome != null ? inputNome.text.Trim() : "";

        if (nome.Length == 0)
        {
            Feedback("⚠ Digite um nome (1-3 letras)!");
            return;
        }

        if (!fase1Ok || !fase2Ok)
        {
            string faltam = "";
            if (!fase1Ok) faltam += " F1";
            if (!fase2Ok) faltam += " F2";
            Feedback("⚠ Complete as fases:" + faltam);
            return;
        }

        bool ok = DatabaseManager.Instance.SalvarTempo(nome, tempoFase1, tempoFase2);
        if (ok)
            Feedback("✔ " + nome + " salvo! Total: " + FormatarTempo(tempoFase1 + tempoFase2));
        else
            Feedback("✖ Erro ao salvar.");
    }

    // ── BOTÃO VER RANKING ──────────────────────────────────
    public void OnVerRanking()
    {
        if (painelRanking == null || textoRanking == null) return;

        List<EntradaRanking> lista = DatabaseManager.Instance.ObterRanking(10);

        if (lista.Count == 0)
        {
            textoRanking.text = "Nenhum registro ainda.";
        }
        else
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("#   NOME  FASE 1    FASE 2    TOTAL");
            sb.AppendLine("──────────────────────────────────────");
            for (int i = 0; i < lista.Count; i++)
            {
                EntradaRanking e = lista[i];
                sb.AppendLine(string.Format("{0,-3} {1,-4}  {2,8}  {3,8}  {4,8}",
                    i + 1,
                    e.Nome,
                    FormatarTempo(e.Fase1),
                    FormatarTempo(e.Fase2),
                    FormatarTempo(e.Total)));
            }
            textoRanking.text = sb.ToString();
        }

        painelRanking.SetActive(true);
    }

    // ── FECHAR PAINEL ──────────────────────────────────────
    public void FecharRanking()
    {
        if (painelRanking != null)
            painelRanking.SetActive(false);
    }

    // ── HELPERS ────────────────────────────────────────────
    private string FormatarTempo(float t)
    {
        int min = (int)(t % 3600 / 60);
        int sec = (int)(t % 60);
        int milli = (int)((t % 1) * 100);
        return string.Format("{0:00}:{1:00}.{2:00}", min, sec, milli);
    }

    private void Feedback(string msg)
    {
        if (textoFeedback != null)
            textoFeedback.text = msg;
    }
}