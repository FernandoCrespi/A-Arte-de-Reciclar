using UnityEngine;

public class AtaqueInimigo : MonoBehaviour
{
    // Este script gerencia apenas a detecção de proximidade.
    // O ataque (vulcão) é controlado pelo IAInimigoRonda.
    // NÃO desativa o IAInimigoRonda para não cancelar o ataque.

    private IAInimigoRonda iaRonda;

    void Start()
    {
        iaRonda = GetComponentInParent<IAInimigoRonda>();
        if (iaRonda == null)
            iaRonda = GetComponent<IAInimigoRonda>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Não interfere com o IAInimigoRonda
        // Adicione aqui outros comportamentos de proximidade se necessário
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Não interfere com o IAInimigoRonda
    }
}