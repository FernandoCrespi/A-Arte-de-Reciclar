using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicioScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TrocarCena(string nomeDaCena)
    {
        // Verifica se a cena que vai ser carregada é a primeira fase
        // ATENÇÃO: Troque "Fase1" pelo nome exato da sua cena onde o jogo começa pra valer!
        if (nomeDaCena == "Fase1")
        {
            // Tenta acessar o nosso temporizador e inicia a contagem
            if (TemporizadorJogo.Instancia != null)
            {
                TemporizadorJogo.Instancia.IniciarCronometro();
            }
        }

        SceneManager.LoadScene(nomeDaCena);
    }
}