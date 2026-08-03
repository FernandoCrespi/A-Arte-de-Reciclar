using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicioScript : MonoBehaviour
{
    [Header("Fase atual (1=Fase1, 2=Fase2, 3=Fase3, 0=Menu)")]
    public int faseAtual = 0;

    [Header("GameTimer (auto-detectado se vazio)")]
    public GameTimer gameTimer;

    void Start()
    {
        if (gameTimer == null)
            gameTimer = Object.FindFirstObjectByType<GameTimer>();
    }

    public void TrocarCena(string nomeDaCena)
    {
        if (faseAtual > 0 && DatabaseManager.Instance != null)
        {
            float tempo = gameTimer != null ? gameTimer.GetElapsedTime() : 0f;
            DatabaseManager.Instance.SalvarTempoFase(faseAtual, tempo);
            Debug.Log("[AlterarFase] Fase " + faseAtual + " salva: " + tempo.ToString("F2") + "s");
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeDaCena);
    }
}