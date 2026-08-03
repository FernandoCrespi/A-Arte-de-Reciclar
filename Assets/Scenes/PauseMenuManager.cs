using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Referências")]
    public GameObject pauseMenu;
    public GameObject ajustesMenu;
    public RebindTeclas rebindTeclas; // arrasta o RebindManager aqui

    [Header("Configurações")]
    public bool isPaused = false;
    public bool isSettingsOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isSettingsOpen)
            FecharAjustes();
        else if (Input.GetKeyDown(KeyCode.Escape) && !isSettingsOpen && !isPaused)
            Pausar();
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
        isPaused = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;

        isSettingsOpen = true;
        ajustesMenu.SetActive(true);

        // Força atualização do rebind ao abrir ajustes
        if (rebindTeclas != null)
            rebindTeclas.AtualizarTextos();
    }

    public void FecharAjustes()
    {
        isSettingsOpen = false;
        ajustesMenu.SetActive(false);

        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Inicio");
    }

    public void SairDoJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}