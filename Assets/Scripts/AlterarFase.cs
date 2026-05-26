using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicioScript : MonoBehaviour
{
    public void TrocarCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}