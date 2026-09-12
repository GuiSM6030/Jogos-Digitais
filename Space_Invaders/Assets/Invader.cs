using UnityEngine;

public class Invader : MonoBehaviour
{
    [Header("Configuração do invader")]
    public float speed = 1.2f;
    public int vida = 1;
    public int pontos = 10;
    public GameObject missilPrefab;

    [Header("Cadência de tiro")]
    public float intervaloMinimoTiro = 8f;
    public float intervaloMaximoTiro = 12f;

    [Header("Som")]
    public AudioClip somTiro;

    private int vidaAtual;
    private float proximoTiro;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    void Start()
    {
        vidaAtual = vida;

        float offset = Mathf.Abs(transform.position.x) * 0.08f + Mathf.Abs(transform.position.y) * 0.05f;
        proximoTiro = Time.time + offset + Random.Range(intervaloMinimoTiro, intervaloMaximoTiro);
    }

    void Update()
    {
        if (Time.time >= proximoTiro)
        {
            Atirar();
            proximoTiro = Time.time + Random.Range(intervaloMinimoTiro, intervaloMaximoTiro);
        }
    }

    public void Atirar()
    {
        if (missilPrefab == null) return;

        GameObject missil = Instantiate(missilPrefab, transform.position, Quaternion.identity);
        missil.GetComponent<Missil>().SetDirecao(-1);

        if (somTiro != null)
        {
            audioSource.PlayOneShot(somTiro);
        }
    }

    public void AumentarVelocidade(float novaVelocidade)
    {
        speed = novaVelocidade;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missil_Player"))
        {
            Destroy(other.gameObject);
            ReceberDano();
        }
    }

    void ReceberDano()
    {
        vidaAtual--;

        if (vidaAtual <= 0)
        {
            if (GameManager.instancia != null)
            {
                GameManager.instancia.AdicionarPontos(pontos);
            }

            Destroy(gameObject);
        }
    }
}