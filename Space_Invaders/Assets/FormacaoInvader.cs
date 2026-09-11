using UnityEngine;

public class FormacaoInvaders : MonoBehaviour
{
    [Header("Prefabs por linha (3 linhas)")]
    public GameObject[] invaderPrefabsPorLinha = new GameObject[3];

    [Header("Posição inicial")]
    public Vector3 origem = new Vector3(-6f, 4f, 0f);
    public float espacamentoX = 1.2f;
    public float espacamentoY = 1f;
    public int colunas = 6;

    [Header("Limites da formação")]
    public Transform paredeEsquerda;
    public Transform paredeDireita;
    public float velocidade = 1.0f;

    private int direcao = 1;

    void Start()
    {
        CriarFormacao();
    }

    void Update()
    {
        MoverFormacao();
    }

    void CriarFormacao()
    {
        int linhas = invaderPrefabsPorLinha.Length;

        for (int linha = 0; linha < linhas; linha++)
        {
            if (invaderPrefabsPorLinha[linha] == null)
                continue;

            for (int coluna = 0; coluna < colunas; coluna++)
            {
                Vector3 pos = origem + new Vector3(coluna * espacamentoX, -linha * espacamentoY, 0f);
                GameObject invaderGO = Instantiate(invaderPrefabsPorLinha[linha], pos, Quaternion.identity, transform);
                invaderGO.tag = "Invader";
            }
        }
    }

    void MoverFormacao()
    {
        Vector3 novaPos = transform.position;
        novaPos.x += direcao * velocidade * Time.deltaTime;
        novaPos.y = 0f;
        novaPos.z = 0f;
        transform.position = novaPos;

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (Transform filho in transform)
        {
            if (filho == null) continue;
            minX = Mathf.Min(minX, filho.position.x);
            maxX = Mathf.Max(maxX, filho.position.x);
        }

        float limiteEsquerdaAtual = paredeEsquerda != null ? paredeEsquerda.position.x + 0.5f : -8f;
        float limiteDireitaAtual = paredeDireita != null ? paredeDireita.position.x - 0.5f : 8f;

        if (maxX >= limiteDireitaAtual || minX <= limiteEsquerdaAtual)
        {
            direcao *= -1;
        }
    }

    public void AumentarVelocidade(float novaVelocidade)
    {
        velocidade = novaVelocidade;
    }
}