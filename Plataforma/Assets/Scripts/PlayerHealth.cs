using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private const int MaximumHealth = 5;

    [Header("Vida")]
    [SerializeField] private Sprite heartSprite;

    [Header("Morte")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private RuntimeAnimatorController deathAnimatorController;
    [SerializeField, Min(0f)] private float deathAnimationDuration = 0.65f;

    private readonly Image[] heartImages = new Image[MaximumHealth];
    private int currentHealth = MaximumHealth;
    private bool isDead;
    private Canvas hudCanvas;
    private Text scoreText;
    private int displayedScore = -1;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }

        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        ShowHUD();
    }

    private void Update()
    {
        if (scoreText != null && displayedScore != GameScore.CurrentScore)
        {
            displayedScore = GameScore.CurrentScore;
            scoreText.text = $"PONTUAÇÃO: {displayedScore}";
        }
    }

    public void ShowHUD()
    {
        if (heartSprite == null)
        {
            Debug.LogError("Atribua o sprite itens_tileset_32x32_12 ao PlayerHealth.", this);
            return;
        }

        if (hudCanvas == null)
        {
            CreateHUD();
        }

        hudCanvas.gameObject.SetActive(true);
        UpdateHealthBar();
    }

    public void TakeDamage(int amount = 1)
    {
        if (isDead)
        {
            return;
        }

        if (amount <= 0)
        {
            Debug.LogWarning("A quantidade de dano precisa ser maior que zero.", this);
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHealthBar();

        if (currentHealth == 0)
        {
            StartCoroutine(PlayDeathAnimation());
        }
    }

    public void RestoreHealthToFull()
    {
        if (isDead)
        {
            return;
        }

        currentHealth = MaximumHealth;
        UpdateHealthBar();
    }

    private void CreateHUD()
    {
        GameObject canvasObject = new GameObject("HUD da Partida", typeof(RectTransform));
        hudCanvas = canvasObject.AddComponent<Canvas>();
        hudCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        hudCanvas.sortingOrder = 10;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.matchWidthOrHeight = 0.5f;
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject backgroundObject = new GameObject("Fundo da Vida", typeof(RectTransform), typeof(Image));
        backgroundObject.transform.SetParent(hudCanvas.transform, false);
        RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
        backgroundRect.anchorMin = new Vector2(0f, 1f);
        backgroundRect.anchorMax = new Vector2(0f, 1f);
        backgroundRect.pivot = new Vector2(0f, 1f);
        backgroundRect.anchoredPosition = new Vector2(16f, -14f);
        backgroundRect.sizeDelta = new Vector2(236f, 62f);

        Image backgroundImage = backgroundObject.GetComponent<Image>();
        backgroundImage.color = new Color(0.025f, 0.055f, 0.09f, 0.85f);
        backgroundImage.raycastTarget = false;

        for (int index = 0; index < heartImages.Length; index++)
        {
            GameObject heartObject = new GameObject(
                $"Coracao {index + 1}",
                typeof(RectTransform),
                typeof(Image));
            heartObject.transform.SetParent(hudCanvas.transform, false);

            RectTransform heartRect = heartObject.GetComponent<RectTransform>();
            heartRect.anchorMin = new Vector2(0f, 1f);
            heartRect.anchorMax = new Vector2(0f, 1f);
            heartRect.pivot = new Vector2(0f, 1f);
            heartRect.anchoredPosition = new Vector2(26f + index * 43f, -29f);
            heartRect.sizeDelta = new Vector2(36f, 36f);

            Image heartImage = heartObject.GetComponent<Image>();
            heartImage.sprite = heartSprite;
            heartImage.preserveAspect = true;
            heartImage.raycastTarget = false;
            heartImages[index] = heartImage;
        }

        GameObject scoreBackground = new GameObject("Fundo da Pontuacao", typeof(RectTransform), typeof(Image));
        scoreBackground.transform.SetParent(hudCanvas.transform, false);
        RectTransform scoreBackgroundRect = scoreBackground.GetComponent<RectTransform>();
        scoreBackgroundRect.anchorMin = new Vector2(1f, 1f);
        scoreBackgroundRect.anchorMax = new Vector2(1f, 1f);
        scoreBackgroundRect.pivot = new Vector2(1f, 1f);
        scoreBackgroundRect.anchoredPosition = new Vector2(-16f, -14f);
        scoreBackgroundRect.sizeDelta = new Vector2(250f, 62f);

        Image scoreBackgroundImage = scoreBackground.GetComponent<Image>();
        scoreBackgroundImage.color = new Color(0.025f, 0.055f, 0.09f, 0.85f);
        scoreBackgroundImage.raycastTarget = false;

        GameObject scoreObject = new GameObject("Pontuacao", typeof(RectTransform), typeof(Text));
        scoreObject.transform.SetParent(scoreBackground.transform, false);
        RectTransform scoreRect = scoreObject.GetComponent<RectTransform>();
        scoreRect.anchorMin = Vector2.zero;
        scoreRect.anchorMax = Vector2.one;
        scoreRect.offsetMin = new Vector2(8f, 4f);
        scoreRect.offsetMax = new Vector2(-8f, -4f);

        scoreText = scoreObject.GetComponent<Text>();
        scoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (scoreText.font == null)
        {
            scoreText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        if (scoreText.font == null)
        {
            Debug.LogError("Nao foi possivel carregar uma fonte para a pontuacao da HUD.", this);
            scoreText = null;
            return;
        }

        scoreText.fontSize = 24;
        scoreText.fontStyle = FontStyle.Bold;
        scoreText.alignment = TextAnchor.MiddleCenter;
        scoreText.color = new Color(0.94f, 0.93f, 0.87f, 1f);
        scoreText.raycastTarget = false;
        displayedScore = -1;
    }

    private void UpdateHealthBar()
    {
        for (int index = 0; index < heartImages.Length; index++)
        {
            if (heartImages[index] != null)
            {
                heartImages[index].enabled = index < currentHealth;
            }
        }
    }

    private IEnumerator PlayDeathAnimation()
    {
        isDead = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (body != null)
        {
            body.linearVelocity = Vector2.zero;
            body.gravityScale = 0f;
            body.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        foreach (Collider2D playerCollider in GetComponents<Collider2D>())
        {
            playerCollider.enabled = false;
        }

        if (animator == null || deathAnimatorController == null)
        {
            Debug.LogError("Atribua o Animator e o controlador da animacao de morte.", this);
        }
        else
        {
            animator.runtimeAnimatorController = deathAnimatorController;
            animator.speed = 1f;
            animator.Play(0, 0, 0f);
        }

        yield return new WaitForSecondsRealtime(deathAnimationDuration);

        GameStartMenu gameMenu = FindFirstObjectByType<GameStartMenu>();
        if (gameMenu == null)
        {
            Debug.LogError("Nao foi encontrado o GameStartMenu para exibir a tela de derrota.", this);
            yield break;
        }

        gameMenu.ShowGameOver(GameScore.CurrentScore);
    }
}