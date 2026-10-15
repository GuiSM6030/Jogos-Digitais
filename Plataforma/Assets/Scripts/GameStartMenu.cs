using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    private static readonly Color BackgroundColor = new Color(0.025f, 0.055f, 0.09f, 0.98f);
    private static readonly Color PanelColor = new Color(0.055f, 0.105f, 0.15f, 0.98f);
    private static readonly Color AccentColor = new Color(0.95f, 0.67f, 0.24f, 1f);
    private static readonly Color MainTextColor = new Color(0.94f, 0.93f, 0.87f, 1f);
    private static readonly Color MutedTextColor = new Color(0.68f, 0.76f, 0.81f, 1f);

    [SerializeField] private PlayerMovement player;
    [SerializeField] private CameraFollow2D cameraFollow;

    private Canvas menuCanvas;
    private Font uiFont;
    private float previousTimeScale;

    private void Awake()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerMovement>();
        }

        if (cameraFollow == null)
        {
            cameraFollow = FindFirstObjectByType<CameraFollow2D>();
        }

        if (player != null)
        {
            player.enabled = false;
        }

        if (cameraFollow != null)
        {
            cameraFollow.enabled = false;
        }

        uiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (uiFont == null)
        {
            uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        if (uiFont == null)
        {
            Debug.LogError("Nao foi possivel carregar a fonte padrao para a tela inicial.", this);
            ResumeGame();
            return;
        }

        EnsureEventSystem();
        CreateMenu();
    }

    private void EnsureEventSystem()
    {
        EventSystem eventSystem = FindFirstObjectByType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystem = eventSystemObject.AddComponent<EventSystem>();
        }

        if (eventSystem.GetComponent<BaseInputModule>() == null)
        {
            eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }

    private void CreateMenu()
    {
        GameObject canvasObject = new GameObject("Tela Inicial", typeof(RectTransform));
        menuCanvas = canvasObject.AddComponent<Canvas>();
        menuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        menuCanvas.sortingOrder = 1000;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.matchWidthOrHeight = 0.5f;
        canvasObject.AddComponent<GraphicRaycaster>();

        CreatePanel("Fundo", menuCanvas.transform, Vector2.zero, new Vector2(1280f, 720f), BackgroundColor);
        CreatePanel("Faixa superior", menuCanvas.transform, new Vector2(0f, 350f), new Vector2(1280f, 7f), AccentColor);
        CreatePanel("Friso decorativo", menuCanvas.transform, new Vector2(0f, -350f), new Vector2(1280f, 7f), AccentColor);

        CreateText(
            "Categoria",
            menuCanvas.transform,
            new Vector2(0f, 316f),
            new Vector2(700f, 28f),
            "AVENTURA  |  PLATAFORMA",
            17,
            AccentColor,
            FontStyle.Bold,
            TextAnchor.MiddleCenter);
        CreateText(
            "Titulo",
            menuCanvas.transform,
            new Vector2(0f, 265f),
            new Vector2(1120f, 68f),
            "A Lenda dos Tesouros",
            48,
            MainTextColor,
            FontStyle.Bold,
            TextAnchor.MiddleCenter);
        CreateText(
            "Subtitulo",
            menuCanvas.transform,
            new Vector2(0f, 218f),
            new Vector2(940f, 34f),
            "Uma jornada por ruínas antigas, riquezas perdidas e perigos a cada passo.",
            19,
            MutedTextColor,
            FontStyle.Normal,
            TextAnchor.MiddleCenter);

        CreatePanel("Historia", menuCanvas.transform, new Vector2(0f, 112f), new Vector2(1020f, 132f), PanelColor);
        CreateText(
            "Titulo da historia",
            menuCanvas.transform,
            new Vector2(0f, 157f),
            new Vector2(920f, 28f),
            "A JORNADA COMEÇA",
            17,
            AccentColor,
            FontStyle.Bold,
            TextAnchor.MiddleCenter);
        CreateText(
            "Texto da historia",
            menuCanvas.transform,
            new Vector2(0f, 103f),
            new Vector2(900f, 76f),
            "Uma antiga trilha foi descoberta sob as ruínas. Dizem que seus tesouros estão protegidos por monstros e que apenas as chaves escondidas pelo caminho abrem a saída. Explore, enfrente os perigos e encontre o caminho de volta.",
            18,
            MainTextColor,
            FontStyle.Normal,
            TextAnchor.MiddleCenter);

        CreatePanel("Missao", menuCanvas.transform, new Vector2(-260f, -65f), new Vector2(500f, 156f), PanelColor);
        CreateText(
            "Titulo da missao",
            menuCanvas.transform,
            new Vector2(-260f, -20f),
            new Vector2(440f, 28f),
            "SUA MISSAO",
            17,
            AccentColor,
            FontStyle.Bold,
            TextAnchor.MiddleCenter);
        CreateText(
            "Objetivos",
            menuCanvas.transform,
            new Vector2(-260f, -77f),
            new Vector2(430f, 82f),
            "Explore o mapa e colete os tesouros.\nEncontre as chaves para abrir a passagem.\nDerrote os monstros e alcance a saída.",
            17,
            MainTextColor,
            FontStyle.Normal,
            TextAnchor.MiddleCenter);

        CreatePanel("Controles", menuCanvas.transform, new Vector2(260f, -65f), new Vector2(500f, 156f), PanelColor);
        CreateText(
            "Titulo dos controles",
            menuCanvas.transform,
            new Vector2(260f, -20f),
            new Vector2(440f, 28f),
            "CONTROLES",
            17,
            AccentColor,
            FontStyle.Bold,
            TextAnchor.MiddleCenter);
        CreateText(
            "Instrucoes",
            menuCanvas.transform,
            new Vector2(260f, -77f),
            new Vector2(430f, 82f),
            "A / D ou setas: mover\nEspaço: pular (pulo duplo)\nW / S ou setas: subir escadas\nE: atacar",
            17,
            MainTextColor,
            FontStyle.Normal,
            TextAnchor.MiddleCenter);

        Button startButton = CreateStartButton();
        CreateText(
            "Lembrete",
            menuCanvas.transform,
            new Vector2(0f, -298f),
            new Vector2(900f, 28f),
            "COLETE  |  ENCONTRE AS CHAVES  |  CHEGUE ATÉ A SAÍDA",
            14,
            MutedTextColor,
            FontStyle.Bold,
            TextAnchor.MiddleCenter);

        EventSystem.current?.SetSelectedGameObject(startButton.gameObject);
    }

    private Button CreateStartButton()
    {
        GameObject buttonObject = new GameObject("Botao Iniciar", typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(menuCanvas.transform, false);

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, -232f);
        rect.sizeDelta = new Vector2(280f, 62f);

        Image image = buttonObject.GetComponent<Image>();
        image.color = AccentColor;

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;
        ColorBlock colors = button.colors;
        colors.normalColor = AccentColor;
        colors.highlightedColor = new Color(1f, 0.79f, 0.4f, 1f);
        colors.pressedColor = new Color(0.78f, 0.49f, 0.16f, 1f);
        button.colors = colors;
        button.onClick.AddListener(StartGame);

        CreateText(
            "Texto Iniciar",
            buttonObject.transform,
            Vector2.zero,
            new Vector2(280f, 62f),
            "INICIAR A AVENTURA",
            20,
            BackgroundColor,
            FontStyle.Bold,
            TextAnchor.MiddleCenter);

        return button;
    }

    private void StartGame()
    {
        ResumeGame();
        Destroy(menuCanvas.gameObject);
    }

    private void ResumeGame()
    {
        Time.timeScale = previousTimeScale > 0f ? previousTimeScale : 1f;

        if (player != null)
        {
            player.enabled = true;
        }

        if (cameraFollow != null)
        {
            cameraFollow.enabled = true;
        }
    }

    private void CreatePanel(string objectName, Transform parent, Vector2 position, Vector2 size, Color color)
    {
        GameObject panel = new GameObject(objectName, typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image = panel.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
    }

    private void CreateText(
        string objectName,
        Transform parent,
        Vector2 position,
        Vector2 size,
        string content,
        int fontSize,
        Color color,
        FontStyle style,
        TextAnchor alignment)
    {
        GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(parent, false);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Text text = textObject.GetComponent<Text>();
        text.font = uiFont;
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.alignment = alignment;
        text.color = color;
        text.text = content;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
    }
}
