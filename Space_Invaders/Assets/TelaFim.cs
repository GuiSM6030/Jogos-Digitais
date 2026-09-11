using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TelaFim : MonoBehaviour
{
    [Header("Configuração desta tela")]
    public string mensagem = "Você venceu!";
    public Color corDeFundo = new Color(0f, 0f, 0f, 0.85f);
    public string cenaParaReiniciar = "Game";

    void Awake()
    {
        CriarEventSystemSeNaoExistir();
        CriarCanvas();
    }

    void CriarEventSystemSeNaoExistir()
    {
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }
    }

    void CriarCanvas()
    {
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);

        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject fundoGO = new GameObject("Fundo");
        fundoGO.transform.SetParent(canvasGO.transform, false);
        Image fundoImg = fundoGO.AddComponent<Image>();
        fundoImg.color = corDeFundo;
        RectTransform fundoRT = fundoGO.GetComponent<RectTransform>();
        fundoRT.anchorMin = Vector2.zero;
        fundoRT.anchorMax = Vector2.one;
        fundoRT.offsetMin = Vector2.zero;
        fundoRT.offsetMax = Vector2.zero;

        GameObject textoGO = new GameObject("TextoMensagem");
        textoGO.transform.SetParent(canvasGO.transform, false);
        Text texto = textoGO.AddComponent<Text>();
        texto.text = mensagem;
        texto.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        texto.fontSize = 60;
        texto.alignment = TextAnchor.MiddleCenter;
        texto.color = Color.white;

        RectTransform textoRT = textoGO.GetComponent<RectTransform>();
        textoRT.anchorMin = new Vector2(0.1f, 0.55f);
        textoRT.anchorMax = new Vector2(0.9f, 0.75f);
        textoRT.offsetMin = Vector2.zero;
        textoRT.offsetMax = Vector2.zero;

        GameObject botaoGO = new GameObject("BotaoReiniciar");
        botaoGO.transform.SetParent(canvasGO.transform, false);
        Image botaoImg = botaoGO.AddComponent<Image>();
        botaoImg.color = new Color(0.2f, 0.6f, 0.9f);
        Button botao = botaoGO.AddComponent<Button>();

        RectTransform botaoRT = botaoGO.GetComponent<RectTransform>();
        botaoRT.anchorMin = new Vector2(0.35f, 0.35f);
        botaoRT.anchorMax = new Vector2(0.65f, 0.45f);
        botaoRT.offsetMin = Vector2.zero;
        botaoRT.offsetMax = Vector2.zero;

        GameObject textoBotaoGO = new GameObject("TextoBotao");
        textoBotaoGO.transform.SetParent(botaoGO.transform, false);
        Text textoBotao = textoBotaoGO.AddComponent<Text>();
        textoBotao.text = "Jogar novamente";
        textoBotao.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textoBotao.fontSize = 36;
        textoBotao.alignment = TextAnchor.MiddleCenter;
        textoBotao.color = Color.white;

        RectTransform textoBotaoRT = textoBotaoGO.GetComponent<RectTransform>();
        textoBotaoRT.anchorMin = Vector2.zero;
        textoBotaoRT.anchorMax = Vector2.one;
        textoBotaoRT.offsetMin = Vector2.zero;
        textoBotaoRT.offsetMax = Vector2.zero;

        botao.onClick.AddListener(Reiniciar);
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(cenaParaReiniciar);
    }
}