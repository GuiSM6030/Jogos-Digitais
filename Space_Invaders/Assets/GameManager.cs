using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public TMP_Text textoPontos;
    public TMP_Text textoVidas;

    [Header("Dificuldade (Opção B)")]
    public float velocidadeBase = 2f;
    public float incrementoPorInvaderDestruido = 0.05f;
    public float velocidadeMaxima = 6f;

    private int pontuacao = 0;
    private int totalInicialDeInvaders = 0;
    private FormacaoInvaders formacaoInvaders;

    void Awake()
    {
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
        totalInicialDeInvaders = GameObject.FindGameObjectsWithTag("Invader").Length;
    }

    public void AdicionarPontos(int valor)
    {
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
        SceneManager.LoadScene("Victory");
    }

    public void Derrota()
    {
        SceneManager.LoadScene("Defeat");
    }
}