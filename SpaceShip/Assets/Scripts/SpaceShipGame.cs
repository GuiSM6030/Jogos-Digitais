using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class SpaceShipGame : MonoBehaviour
{
    public static float WorldSpeedMultiplier { get; private set; } = 1f;

    [Header("Jogo")]
    public int vidasIniciais = 3;
    public int pontosParaPowerUp = 200;
    public int pontosParaVencer = 1000;
    public float duracaoPowerUp = 15f;

    [Header("Canvas")]
    public TMP_Text textoPontos;
    public TMP_Text textoVidas;
    public TMP_Text textoPowerUp;
    public GameObject painelVitoria;
    public GameObject painelGameOver;

    private int vidas;
    private int pontos;
    private bool desaceleracaoAtiva;
    private bool jogoEncerrado;
    private float fimPowerUp;
    private int proximaPontuacaoPowerUp;

    void Awake()
    {
        WorldSpeedMultiplier = 1f;
        Parallax.SpeedMultiplier = 1f;
        vidas = vidasIniciais;
        proximaPontuacaoPowerUp = pontosParaPowerUp;
        AtualizarCanvas();

        if (painelVitoria != null)
            painelVitoria.SetActive(false);

        if (painelGameOver != null)
            painelGameOver.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame && jogoEncerrado)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (jogoEncerrado)
            return;

        if (desaceleracaoAtiva && Time.time >= fimPowerUp)
        {
            desaceleracaoAtiva = false;
            WorldSpeedMultiplier = 1f;
            Parallax.SpeedMultiplier = 1f;
            proximaPontuacaoPowerUp = pontos + pontosParaPowerUp;
            AtualizarCanvas();
        }

        if (Keyboard.current != null && Keyboard.current.rightArrowKey.wasPressedThisFrame && PodeUsarPowerUp())
        {
            pontos -= pontosParaPowerUp;
            desaceleracaoAtiva = true;
            fimPowerUp = Time.time + duracaoPowerUp;
            WorldSpeedMultiplier = 0.35f;
            Parallax.SpeedMultiplier = WorldSpeedMultiplier;
            AtualizarCanvas();
        }

        if (desaceleracaoAtiva)
            AtualizarCanvas();

    }

    public void AdicionarPontos(int valor)
    {
        pontos += valor;
        AtualizarCanvas();

        if (pontos >= pontosParaVencer)
        {
            jogoEncerrado = true;
            WorldSpeedMultiplier = 0f;
            Parallax.SpeedMultiplier = 0f;

            if (painelVitoria != null)
                painelVitoria.SetActive(true);
        }
    }

    public void PerderVida()
    {
        vidas--;
        AtualizarCanvas();

        if (vidas <= 0)
        {
            jogoEncerrado = true;
            WorldSpeedMultiplier = 0f;
            Parallax.SpeedMultiplier = 0f;

            if (painelGameOver != null)
                painelGameOver.SetActive(true);
        }
    }

    public bool PodeUsarPowerUp()
    {
        return !desaceleracaoAtiva && pontos >= proximaPontuacaoPowerUp;
    }

    public bool JogoEncerrado()
    {
        return jogoEncerrado;
    }

    void AtualizarCanvas()
    {
        if (textoPontos != null)
            textoPontos.SetText("Pontos: {0}", pontos);

        if (textoVidas != null)
            textoVidas.SetText("Vidas: {0}", vidas);

        if (textoPowerUp != null)
        {
            if (desaceleracaoAtiva)
                textoPowerUp.SetText("Power-up: ATIVO ({0}s)", Mathf.CeilToInt(Mathf.Max(0f, fimPowerUp - Time.time)));
            else if (pontos < proximaPontuacaoPowerUp)
                textoPowerUp.SetText("Power-up: faltam {0} pontos", proximaPontuacaoPowerUp - pontos);
            else
                textoPowerUp.SetText("Power-up: seta direita para ativar");
        }
    }

}
