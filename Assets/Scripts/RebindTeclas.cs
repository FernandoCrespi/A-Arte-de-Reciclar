using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RebindTeclas : MonoBehaviour
{
    public Controle player;

    [Header("Textos dos botões")]
    public TextMeshProUGUI textoBotaoDireita;
    public TextMeshProUGUI textoBotaoEsquerda;
    public TextMeshProUGUI textoBotaoPulo;
    public TextMeshProUGUI textoBotaoAtaque;

    private bool esperandoTecla = false;
    private string tipoTecla;
    private TextMeshProUGUI textoBotaoAtivo;

    void Start()
    {
        CarregarTeclas();
        AtualizarTextos();
    }

    void CarregarTeclas()
    {
        if (player == null) return;
        player.teclaDireita = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("teclaDireita", player.teclaDireita.ToString()));
        player.teclaEsquerda = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("teclaEsquerda", player.teclaEsquerda.ToString()));
        player.teclaPulo = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("teclaPulo", player.teclaPulo.ToString()));
        player.teclaAtaque = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("teclaAtaque", player.teclaAtaque.ToString()));
    }

    public void AtualizarTextos()
    {
        if (player == null) return;
        if (textoBotaoDireita != null) textoBotaoDireita.text = player.teclaDireita == KeyCode.None ? "Direita: Nao definido" : "Direita: " + player.teclaDireita;
        if (textoBotaoEsquerda != null) textoBotaoEsquerda.text = player.teclaEsquerda == KeyCode.None ? "Esquerda: Nao definido" : "Esquerda: " + player.teclaEsquerda;
        if (textoBotaoPulo != null) textoBotaoPulo.text = player.teclaPulo == KeyCode.None ? "Pulo: Nao definido" : "Pulo: " + player.teclaPulo;
        if (textoBotaoAtaque != null) textoBotaoAtaque.text = player.teclaAtaque == KeyCode.None ? "Ataque: Nao definido" : "Ataque: " + player.teclaAtaque;
    }

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

    public void MudarDireita() => IniciarEspera("Direita", textoBotaoDireita);
    public void MudarEsquerda() => IniciarEspera("Esquerda", textoBotaoEsquerda);
    public void MudarPulo() => IniciarEspera("Pulo", textoBotaoPulo);
    public void MudarAtaque() => IniciarEspera("Ataque", textoBotaoAtaque);

    void IniciarEspera(string tipo, TextMeshProUGUI texto)
    {
        esperandoTecla = true;
        tipoTecla = tipo;
        textoBotaoAtivo = texto;
        if (textoBotaoAtivo != null)
            textoBotaoAtivo.text = "Pressione uma tecla...";
    }

    void LimparConflito(KeyCode novaTecla)
    {
        // Se a tecla já está em outro slot, limpa aquele slot
        if (tipoTecla != "Direita" && player.teclaDireita == novaTecla)
        {
            player.teclaDireita = KeyCode.None;
            PlayerPrefs.SetString("teclaDireita", KeyCode.None.ToString());
            if (textoBotaoDireita != null) textoBotaoDireita.text = "Direita: Nao definido";
        }
        if (tipoTecla != "Esquerda" && player.teclaEsquerda == novaTecla)
        {
            player.teclaEsquerda = KeyCode.None;
            PlayerPrefs.SetString("teclaEsquerda", KeyCode.None.ToString());
            if (textoBotaoEsquerda != null) textoBotaoEsquerda.text = "Esquerda: Nao definido";
        }
        if (tipoTecla != "Pulo" && player.teclaPulo == novaTecla)
        {
            player.teclaPulo = KeyCode.None;
            PlayerPrefs.SetString("teclaPulo", KeyCode.None.ToString());
            if (textoBotaoPulo != null) textoBotaoPulo.text = "Pulo: Nao definido";
        }
        if (tipoTecla != "Ataque" && player.teclaAtaque == novaTecla)
        {
            player.teclaAtaque = KeyCode.None;
            PlayerPrefs.SetString("teclaAtaque", KeyCode.None.ToString());
            if (textoBotaoAtaque != null) textoBotaoAtaque.text = "Ataque: Nao definido";
        }
    }

    void DefinirTecla(KeyCode novaTecla)
    {
        esperandoTecla = false;

        // Limpa conflito no outro slot antes de definir
        LimparConflito(novaTecla);

        switch (tipoTecla)
        {
            case "Direita":
                player.teclaDireita = novaTecla;
                PlayerPrefs.SetString("teclaDireita", novaTecla.ToString());
                break;
            case "Esquerda":
                player.teclaEsquerda = novaTecla;
                PlayerPrefs.SetString("teclaEsquerda", novaTecla.ToString());
                break;
            case "Pulo":
                player.teclaPulo = novaTecla;
                PlayerPrefs.SetString("teclaPulo", novaTecla.ToString());
                break;
            case "Ataque":
                player.teclaAtaque = novaTecla;
                PlayerPrefs.SetString("teclaAtaque", novaTecla.ToString());
                break;
        }

        PlayerPrefs.Save();

        if (textoBotaoAtivo != null)
            textoBotaoAtivo.text = tipoTecla + ": " + novaTecla;
    }
}