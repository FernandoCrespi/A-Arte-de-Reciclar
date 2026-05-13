using UnityEngine;
using TMPro;

public class TemporizadorJogo : MonoBehaviour
{
    public static TemporizadorJogo Instancia;
    public TMP_Text textoCronometro;

    private float tempo = 0f;
    private bool contando = true;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!contando) return;

        tempo += Time.deltaTime;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        int minutos = (int)(tempo / 60);
        int segundos = (int)(tempo % 60);
        textoCronometro.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void PararTemporizador()
    {
        contando = false;
        AtualizarUI();
    }

    public float PegarTempo()
    {
        return tempo;
    }
}