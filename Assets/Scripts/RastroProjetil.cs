using UnityEngine;

public class RastroProjetil : MonoBehaviour
{
    [Header("Rastro")]
    public float tempo = 0.3f;
    public float larguraInicio = 0.15f;
    public float larguraFim = 0f;
    public Color corInicio = new Color(1f, 0.5f, 0f, 1f);
    public Color corFim = new Color(1f, 0.1f, 0f, 0f);

    [Header("Ordenacao")]
    public string sortingLayerName = "Personagens";
    public int orderInLayer = 1;

    void Awake()
    {
        TrailRenderer trail = gameObject.AddComponent<TrailRenderer>();

        trail.time = tempo;
        trail.startWidth = larguraInicio;
        trail.endWidth = larguraFim;
        trail.minVertexDistance = 0.05f;

        trail.sortingLayerName = sortingLayerName;
        trail.sortingOrder = orderInLayer;

        GradientColorKey[] cores = new GradientColorKey[2];
        cores[0] = new GradientColorKey(corInicio, 0f);
        cores[1] = new GradientColorKey(corFim, 1f);

        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(1f, 0f);
        alphas[1] = new GradientAlphaKey(0f, 1f);

        Gradient grad = new Gradient();
        grad.SetKeys(cores, alphas);
        trail.colorGradient = grad;

        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.autodestruct = true;
    }
}