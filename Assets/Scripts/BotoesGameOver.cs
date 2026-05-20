using UnityEngine;
using UnityEngine.SceneManagement;

public class BotoesGameOver : MonoBehaviour
{
    public void JogarNovamente()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // cena 0 = tela inicial
    }
}