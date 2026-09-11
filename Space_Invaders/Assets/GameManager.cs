using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("HUD")]
    public TMP_Text textoPontos;
    public TMP_Text textoVidas;

    [Header("Tela de resultado")]
    public GameObject painelResultado;
    public TMP_Text textoResultado;
    public TMP_Text textoBotaoResultado;
    public Button botaoResultado;
    public string cenaRetry = "Level1";
    public string cenaProximoNivel = "Level2";

    [Header("Dificuldade (Opção B)")]
    public float velocidadeBase = 2.0f;
    public float incrementoPorInvaderDestruido = 0.15f;
    public float velocidadeMaxima = 8.0f;

    private int pontuacao = 0;
    private int totalInicialDeInvaders = 0;
    private FormacaoInvaders formacaoInvaders;
    private bool jogoFinalizado = false;

    void Awake()
    {
        Time.timeScale = 1f;

        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        formacaoInvaders = FindFirstObjectByType<FormacaoInvaders>();
        AtualizarPontos();
        AtualizarVidas(3);
        totalInicialDeInvaders = GameObject.FindGameObjectsWithTag("Invader").Length;

        if (painelResultado != null)
            painelResultado.SetActive(false);

        if (botaoResultado != null)
            botaoResultado.onClick.RemoveAllListeners();
    }

    void Update()
    {
        if (jogoFinalizado) return;

        if (GameObject.FindGameObjectsWithTag("Invader").Length == 0)
        {
            Vitoria();
        }
    }

    public void AdicionarPontos(int valor)
    {
        if (jogoFinalizado) return;

        pontuacao += valor;
        AtualizarPontos();
        AumentarDificuldade();
        VerificarVitoria();
    }

    void AtualizarPontos()
    {
        if (textoPontos != null)
            textoPontos.SetText("Pontos: " + pontuacao);
    }

    public void AtualizarVidas(int vidas)
    {
        if (textoVidas != null)
            textoVidas.SetText("Vidas: " + vidas);
    }

    void AumentarDificuldade()
    {
        GameObject[] restantes = GameObject.FindGameObjectsWithTag("Invader");
        if (restantes.Length == 0 || totalInicialDeInvaders == 0) return;

        int destruidos = totalInicialDeInvaders - restantes.Length;
        float fator = 1f + destruidos * incrementoPorInvaderDestruido;
        float novaVelocidade = Mathf.Min(velocidadeBase * fator, velocidadeMaxima);

        if (formacaoInvaders != null)
        {
            formacaoInvaders.AumentarVelocidade(novaVelocidade);
        }
    }

    void VerificarVitoria()
    {
        if (GameObject.FindGameObjectsWithTag("Invader").Length == 0)
        {
            Vitoria();
        }
    }

    public void Vitoria()
    {
        if (jogoFinalizado) return;
        jogoFinalizado = true;
        MostrarResultado("Você venceu", "Próximo nível", cenaProximoNivel);
    }

    public void Derrota()
    {
        if (jogoFinalizado) return;
        jogoFinalizado = true;
        MostrarResultado("Você Morreu", "Tentar novamente", cenaRetry);
    }

    void MostrarResultado(string mensagem, string textoBotao, string cenaDestino)
    {
        if (painelResultado != null)
            painelResultado.SetActive(true);

        if (textoResultado != null)
            textoResultado.SetText(mensagem);

        if (textoBotaoResultado != null)
            textoBotaoResultado.SetText(textoBotao);

        if (botaoResultado != null)
        {
            botaoResultado.onClick.RemoveAllListeners();
            botaoResultado.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(cenaDestino);
            });
        }

        Time.timeScale = 0f;
    }
}