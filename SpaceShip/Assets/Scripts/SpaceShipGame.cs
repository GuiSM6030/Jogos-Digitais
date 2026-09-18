using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipGame : MonoBehaviour
{
    public static float WorldSpeedMultiplier { get; private set; } = 1f;

    [Header("Jogo")]
    public int vidasIniciais = 3;
    public int pontosParaPowerUp = 200;
    public int pontosParaVencer = 1000;
    private int vidas;
    private int pontos;
    private bool desaceleracaoAtiva;
    private bool jogoEncerrado;
    private GUIStyle hudStyle;
    private GUIStyle mensagemStyle;

    void Awake()
    {
        WorldSpeedMultiplier = 1f;
        Parallax.SpeedMultiplier = 1f;
        vidas = vidasIniciais;
        CriarHudStyles();
    }

    void Update()
    {
        if (jogoEncerrado)
            return;

        if (Keyboard.current != null && Keyboard.current.rightArrowKey.wasPressedThisFrame && pontos >= pontosParaPowerUp)
        {
            desaceleracaoAtiva = !desaceleracaoAtiva;
            WorldSpeedMultiplier = desaceleracaoAtiva ? 0.35f : 1f;
            Parallax.SpeedMultiplier = WorldSpeedMultiplier;
        }

    }

    public void AdicionarPontos(int valor)
    {
        pontos += valor;
        if (pontos >= pontosParaVencer)
        {
            jogoEncerrado = true;
            WorldSpeedMultiplier = 0f;
            Parallax.SpeedMultiplier = 0f;
        }
    }

    public void PerderVida()
    {
        vidas--;
        if (vidas <= 0)
        {
            jogoEncerrado = true;
            WorldSpeedMultiplier = 0f;
            Parallax.SpeedMultiplier = 0f;
        }
    }

    public bool PodeUsarPowerUp()
    {
        return pontos >= pontosParaPowerUp;
    }

    public bool JogoEncerrado()
    {
        return jogoEncerrado;
    }

    void CriarHudStyles()
    {
        hudStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            normal = { textColor = Color.white }
        };
        mensagemStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 42,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };
    }

    void OnGUI()
    {
        if (hudStyle == null)
            CriarHudStyles();

        GUI.Label(new Rect(20, 15, 260, 35), "Pontos: " + pontos, hudStyle);
        GUI.Label(new Rect(20, 45, 260, 35), "Vidas: " + vidas, hudStyle);
        string powerUp = !PodeUsarPowerUp() ? "Power-up: " + pontosParaPowerUp + " pontos" : desaceleracaoAtiva ? "Power-up: ATIVO" : "Power-up: UP para ativar";
        GUI.Label(new Rect(20, 75, 360, 35), powerUp, hudStyle);

        if (jogoEncerrado)
        {
            string mensagem = pontos >= pontosParaVencer ? "VOCÊ VENCEU!" : "GAME OVER";
            GUI.Label(new Rect(Screen.width / 2f - 250, Screen.height / 2f - 35, 500, 70), mensagem, mensagemStyle);
        }
    }
}
