using UnityEngine;

public class TemporizadorJogo : MonoBehaviour
{
    public static TemporizadorJogo Instancia { get; private set; }

    private float tempoDecorrido = 0f;
    private bool estaContando = false;

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
        if (estaContando)
        {
            tempoDecorrido += Time.deltaTime;
        }
    }

    public void IniciarCronometro()
    {
        tempoDecorrido = 0f;
        estaContando = true;
    }

    public void PararCronometro()
    {
        estaContando = false;
    }

    public string ObterTempoFormatado()
    {
        int minutos = Mathf.FloorToInt(tempoDecorrido / 60f);
        int segundos = Mathf.FloorToInt(tempoDecorrido % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}