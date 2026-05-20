using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BotoesUI : MonoBehaviour
{
    [Header("Referência")]
    public Controle controle;

    [Header("Tamanho dos botões")]
    public float tamanhoBotao = 120f;
    public float tamanhoJump = 140f;

    [Header("Cores")]
    public Color corMovimento = new Color(1f, 1f, 1f, 0.15f);
    public Color corPulo = new Color(0.4f, 0.9f, 0.5f, 0.35f);
    public Color corAtaque = new Color(0.9f, 0.3f, 0.3f, 0.35f);
    public Color corBorda = new Color(1f, 1f, 1f, 0.25f);

    void Start()
    {
        if (controle == null)
            controle = FindFirstObjectByType<Controle>();

        Canvas canvas = GetOrCreateCanvas();
        CriarBotao(canvas, "ESQ", corMovimento, AnchorEsquerdo(0), tamanhoBotao, v => controle.btnEsquerda = v);
        CriarBotao(canvas, "DIR", corMovimento, AnchorEsquerdo(1), tamanhoBotao, v => controle.btnDireita = v);
        CriarBotao(canvas, "PULO", corPulo, AnchorDireito(0), tamanhoJump, v => controle.btnPulo = v, isOneShot: true);
        CriarBotao(canvas, "ATK", corAtaque, AnchorDireito(1), tamanhoBotao, v => controle.btnAtaque = v, isOneShot: true);
    }

    Canvas GetOrCreateCanvas()
    {
        Canvas c = FindFirstObjectByType<Canvas>();
        if (c != null) return c;

        GameObject go = new GameObject("Canvas_HUD");
        c = go.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        c.sortingOrder = 10;

        CanvasScaler cs = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.matchWidthOrHeight = 0.5f;

        go.AddComponent<GraphicRaycaster>();

        if (FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }
        return c;
    }

    Vector2 AnchorEsquerdo(int indice)
    {
        float margem = 10f;
        float espaco = tamanhoBotao + 10f;
        return new Vector2(margem + indice * espaco, 20f);
    }

    Vector2 AnchorDireito(int indice)
    {
        float margem = 10f;
        float espaco = tamanhoBotao + 10f;
        return new Vector2(-(margem + indice * espaco), 20f);
    }

    void CriarBotao(Canvas canvas, string label, Color cor, Vector2 posicao,
                    float tamanho, System.Action<bool> setter, bool isOneShot = false)
    {
        GameObject go = new GameObject("Btn_" + label);
        go.transform.SetParent(canvas.transform, false);

        RectTransform rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(tamanho, tamanho);

        if (posicao.x >= 0)
        {
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 0f);
            rt.anchoredPosition = posicao;
        }
        else
        {
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(1f, 0f);
            rt.anchoredPosition = posicao;
        }

        // Fundo circular
        Image img = go.AddComponent<Image>();
        img.sprite = CriarCirculo();
        img.color = cor;
        img.raycastTarget = true;

        // Borda
        GameObject bordaGo = new GameObject("Borda");
        bordaGo.transform.SetParent(go.transform, false);
        RectTransform bordaRt = bordaGo.AddComponent<RectTransform>();
        bordaRt.anchorMin = Vector2.zero;
        bordaRt.anchorMax = Vector2.one;
        bordaRt.offsetMin = bordaRt.offsetMax = Vector2.zero;
        Image bordaImg = bordaGo.AddComponent<Image>();
        bordaImg.sprite = CriarCirculoBorda();
        bordaImg.color = corBorda;
        bordaImg.raycastTarget = false;

        // Texto
        GameObject textoGo = new GameObject("Label");
        textoGo.transform.SetParent(go.transform, false);
        RectTransform textoRt = textoGo.AddComponent<RectTransform>();
        textoRt.anchorMin = Vector2.zero;
        textoRt.anchorMax = Vector2.one;
        textoRt.offsetMin = textoRt.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = textoGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = tamanho * 0.22f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = new Color(1f, 1f, 1f, 0.95f);
        tmp.raycastTarget = false;

        // Handler
        BotaoHandler handler = go.AddComponent<BotaoHandler>();
        handler.setter = setter;
        handler.isOneShot = isOneShot;
        handler.imagemFundo = img;
        handler.corNormal = cor;
        handler.corPressionado = new Color(cor.r, cor.g, cor.b, Mathf.Min(cor.a + 0.25f, 1f));
    }

    Sprite CriarCirculo()
    {
        int res = 128;
        Texture2D tex = new Texture2D(res, res, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        float c = res / 2f, raio = c - 2f;
        for (int y = 0; y < res; y++)
            for (int x = 0; x < res; x++)
            {
                float d = Vector2.Distance(new Vector2(x, y), new Vector2(c, c));
                tex.SetPixel(x, y, new Color(1, 1, 1, Mathf.Clamp01(1f - (d - raio))));
            }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, res, res), new Vector2(0.5f, 0.5f));
    }

    Sprite CriarCirculoBorda()
    {
        int res = 128;
        Texture2D tex = new Texture2D(res, res, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        float c = res / 2f, raioExt = c - 1f, raioInt = c - 5f;
        for (int y = 0; y < res; y++)
            for (int x = 0; x < res; x++)
            {
                float d = Vector2.Distance(new Vector2(x, y), new Vector2(c, c));
                tex.SetPixel(x, y, new Color(1, 1, 1,
                    Mathf.Clamp01(1f - (d - raioExt)) * Mathf.Clamp01(d - raioInt)));
            }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, res, res), new Vector2(0.5f, 0.5f));
    }
}

public class BotaoHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public System.Action<bool> setter;
    public bool isOneShot;
    public Image imagemFundo;
    public Color corNormal;
    public Color corPressionado;

    public void OnPointerDown(PointerEventData e)
    {
        setter?.Invoke(true);
        if (imagemFundo) imagemFundo.color = corPressionado;
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (!isOneShot) setter?.Invoke(false);
        if (imagemFundo) imagemFundo.color = corNormal;
    }
}