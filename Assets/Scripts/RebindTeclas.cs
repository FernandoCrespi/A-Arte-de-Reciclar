using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RebindTeclas : MonoBehaviour
{
    public Controle player;
    public TextMeshProUGUI textoBotao;

    private bool esperandoTecla = false;
    private string tipoTecla;

    void Update()
    {
        if (esperandoTecla && Input.anyKeyDown)
        {
            foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(k))
                {
                    DefinirTecla(k);
                    break;
                }
            }
        }
    }

    public void MudarDireita()
    {
        esperandoTecla = true;
        tipoTecla = "Direita";
        textoBotao.text = "Pressione uma tecla...";
    }

    public void MudarEsquerda()
    {
        esperandoTecla = true;
        tipoTecla = "Esquerda";
        textoBotao.text = "Pressione uma tecla...";
    }

    public void MudarPulo()
    {
        esperandoTecla = true;
        tipoTecla = "Pulo";
        textoBotao.text = "Pressione uma tecla...";
    }

    void DefinirTecla(KeyCode novaTecla)
    {
        esperandoTecla = false;

        if (tipoTecla == "Direita")
        {
            player.teclaDireita = novaTecla;
            textoBotao.text = "Direita: " + novaTecla;
        }
        else if (tipoTecla == "Esquerda")
        {
            player.teclaEsquerda = novaTecla;
            textoBotao.text = "Esquerda: " + novaTecla;
        }
        else if (tipoTecla == "Pulo")
        {
            player.teclaPulo = novaTecla;
            textoBotao.text = "Pulo: " + novaTecla;
        }
    }
}
