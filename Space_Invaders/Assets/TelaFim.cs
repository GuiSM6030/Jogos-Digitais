using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class TelaFim : MonoBehaviour
{
    [Header("Configuração desta tela")]
    public string mensagem = "Você venceu!";
    public string textoBotao = "Próximo nível";
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
        TMP_Text texto = textoGO.AddComponent<TextMeshProUGUI>();
        texto.text = mensagem;
        texto.fontSize = 60;
        texto.alignment = TextAlignmentOptions.Center;
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
        TMP_Text textoBotaoTMP = textoBotaoGO.AddComponent<TextMeshProUGUI>();
        textoBotaoTMP.text = textoBotao;
        textoBotaoTMP.fontSize = 36;
        textoBotaoTMP.alignment = TextAlignmentOptions.Center;
        textoBotaoTMP.color = Color.white;

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