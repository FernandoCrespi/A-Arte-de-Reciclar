using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicioScript : MonoBehaviour
{
    public void TrocarCena(string nomeDaCena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeDaCena);
    }
}