using UnityEngine;
using UnityEngine.EventSystems;

public class ControleUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Botao { Direita, Esquerda, Pulo, Ataque }
    public Botao tipoBotao;

    private Controle controle;

    void Start()
    {
        // Busca o script Controle na cena automaticamente
        controle = FindObjectOfType<Controle>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (tipoBotao)
        {
            case Botao.Direita: controle.btnDireita = true; break;
            case Botao.Esquerda: controle.btnEsquerda = true; break;
            case Botao.Pulo: controle.btnPulo = true; break;
            case Botao.Ataque: controle.btnAtaque = true; break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (tipoBotao)
        {
            case Botao.Direita: controle.btnDireita = false; break;
            case Botao.Esquerda: controle.btnEsquerda = false; break;
                // Pulo e Ataque já se resetam sozinhos no Update
        }
    }
}