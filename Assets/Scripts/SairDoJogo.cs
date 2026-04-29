using UnityEngine;

public class SairDoJogo : MonoBehaviour
{
    public void Sair()
    {
        Debug.Log("Botão Sair clicado");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
