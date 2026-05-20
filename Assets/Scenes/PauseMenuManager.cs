using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Referências")]
    public GameObject pauseMenu;      // Arraste o objeto "PauseMenu" aqui
    public GameObject ajustesMenu;    // Arraste o objeto "Canvas_Ajustes" aqui

    [Header("Configurações")]
    public bool isPaused = false;
    public bool isSettingsOpen = false;

    void Update()
    {
        // Se estiver nos ajustes, ESC fecha os ajustes
        if (Input.GetKeyDown(KeyCode.Escape) && isSettingsOpen)
        {
            FecharAjustes();
        }
        // Se estiver jogando, ESC abre o pause
        else if (Input.GetKeyDown(KeyCode.Escape) && !isSettingsOpen && !isPaused)
        {
            Pausar();
        }
    }

    public void Pausar()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Continuar()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AbrirAjustes()
    {
        Debug.Log("Abrindo Ajustes...");

        // Fecha o pause primeiro
        isPaused = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;

        // Abre os ajustes
        isSettingsOpen = true;
        ajustesMenu.SetActive(true);
    }

    public void FecharAjustes()
    {
        Debug.Log("Voltando do Ajustes...");

        // Fecha os ajustes
        isSettingsOpen = false;
        ajustesMenu.SetActive(false);

        // Reabre o pause
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void VoltarMenu()
    {
        Debug.Log("Voltar ao Menu Principal");
        SceneManager.LoadScene("Inicio");
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do Jogo");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}