using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSpaceShip : MonoBehaviour
{
    public SpaceShipGame game;

    void Awake()
    {
        if (game == null)
            game = FindFirstObjectByType<SpaceShipGame>();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if (scaler == null)
            scaler = gameObject.AddComponent<CanvasScaler>();

        if (GetComponent<GraphicRaycaster>() == null)
            gameObject.AddComponent<GraphicRaycaster>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1024f, 768f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        CriarInterface();
    }

    void CriarInterface()
    {
        TMP_Text textoPontos = CriarTexto("TextoPontos", "Pontos: 0", new Vector2(20f, -20f), new Vector2(260f, 40f), 24f, TextAlignmentOptions.Left);
        TMP_Text textoVidas = CriarTexto("TextoVidas", "Vidas: 3", new Vector2(20f, -55f), new Vector2(260f, 40f), 24f, TextAlignmentOptions.Left);
        TMP_Text textoPowerUp = CriarTexto("TextoPowerUp", "Power-up: faltam 200 pontos", new Vector2(20f, -90f), new Vector2(420f, 40f), 20f, TextAlignmentOptions.Left);

        GameObject painelVitoria = CriarPainel("PainelVitoria", "VOCE VENCEU!\nPressione R para jogar novamente");
        GameObject painelGameOver = CriarPainel("PainelGameOver", "GAME OVER\nPressione R para jogar novamente");

        if (game != null)
        {
            game.textoPontos = textoPontos;
            game.textoVidas = textoVidas;
            game.textoPowerUp = textoPowerUp;
            game.painelVitoria = painelVitoria;
            game.painelGameOver = painelGameOver;
        }
    }

    TMP_Text CriarTexto(string nome, string conteudo, Vector2 posicao, Vector2 tamanho, float fonte, TextAlignmentOptions alinhamento)
    {
        GameObject objeto = new GameObject(nome, typeof(RectTransform), typeof(TextMeshProUGUI));
        objeto.transform.SetParent(transform, false);

        RectTransform rect = objeto.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = posicao;
        rect.sizeDelta = tamanho;

        TextMeshProUGUI texto = objeto.GetComponent<TextMeshProUGUI>();
        texto.text = conteudo;
        texto.fontSize = fonte;
        texto.alignment = alinhamento;
        texto.color = Color.white;
        texto.raycastTarget = false;
        return texto;
    }

    GameObject CriarPainel(string nome, string mensagem)
    {
        GameObject painel = new GameObject(nome, typeof(RectTransform), typeof(Image));
        painel.transform.SetParent(transform, false);

        RectTransform rect = painel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(520f, 220f);

        Image imagem = painel.GetComponent<Image>();
        imagem.color = new Color(0f, 0f, 0f, 0.85f);

        CriarTextoFilho(painel.transform, mensagem);
        painel.SetActive(false);
        return painel;
    }

    void CriarTextoFilho(Transform pai, string mensagem)
    {
        GameObject objeto = new GameObject("Mensagem", typeof(RectTransform), typeof(TextMeshProUGUI));
        objeto.transform.SetParent(pai, false);

        RectTransform rect = objeto.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(20f, 20f);
        rect.offsetMax = new Vector2(-20f, -20f);

        TextMeshProUGUI texto = objeto.GetComponent<TextMeshProUGUI>();
        texto.text = mensagem;
        texto.fontSize = 28f;
        texto.alignment = TextAlignmentOptions.Center;
        texto.color = Color.white;
    }
}
