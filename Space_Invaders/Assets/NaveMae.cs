using UnityEngine;

public class NaveMae : MonoBehaviour
{
    [Header("Configuração da nave mãe")]
    public float velocidade = 5f;
    public int vida = 5;
    public int pontos = 200;

    private int vidaAtual;
    private float limiteDireita;

    void Start()
    {
        vidaAtual = vida;

        Camera cameraPrincipal = Camera.main;

        if (cameraPrincipal == null)
        {
            limiteDireita = 12f;
            return;
        }

        float alturaCam = cameraPrincipal.orthographicSize;
        float larguraCam = alturaCam * Camera.main.aspect;
        Vector3 centroCam = cameraPrincipal.transform.position;

        limiteDireita = centroCam.x + larguraCam + 2f;
    }

    void Update()
    {
        transform.position += Vector3.right * velocidade * Time.deltaTime;

        if (transform.position.x > limiteDireita)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missil_Player"))
        {
            Destroy(other.gameObject);

            vidaAtual--;

            if (vidaAtual <= 0)
            {
                if (GameManager.instancia != null)
                {
                    GameManager.instancia.AdicionarPontos(pontos);
                    GameManager.instancia.VitoriaFinal();
                }

                Destroy(gameObject);
            }
        }
    }
}