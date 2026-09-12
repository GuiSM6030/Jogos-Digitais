using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float velocidade = 5f;
    public GameObject missilPrefab;
    public Transform pontoDeTiro;
    public Transform paredeEsquerda;
    public Transform paredeDireita;

    public int vidas = 3;

    [Header("Som")]
    public AudioClip somTiro;

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

    void Update()
    {
        Mover();

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Atirar();
        }
    }

    void Mover()
    {
        if (Keyboard.current == null) return;

        Vector3 pos = transform.position;

        if (Keyboard.current.rightArrowKey.isPressed)
            pos.x += velocidade * Time.deltaTime;

        if (Keyboard.current.leftArrowKey.isPressed)
            pos.x -= velocidade * Time.deltaTime;

        float limiteMin = paredeEsquerda != null ? paredeEsquerda.position.x + 0.5f : -8f;
        float limiteMax = paredeDireita != null ? paredeDireita.position.x - 0.5f : 8f;

        pos.x = Mathf.Clamp(pos.x, limiteMin, limiteMax);
        pos.y = -3f;
        pos.z = 0f;
        transform.position = pos;
    }

    void Atirar()
    {
        if (missilPrefab == null || pontoDeTiro == null) return;

        GameObject missil = Instantiate(missilPrefab, pontoDeTiro.position, Quaternion.identity);
        missil.GetComponent<Missil>().SetDirecao(1);

        if (somTiro != null)
        {
            audioSource.PlayOneShot(somTiro);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missil_Invader"))
        {
            Destroy(other.gameObject);
            LevarDano();
        }
    }

    void LevarDano()
    {
        vidas--;

        if (GameManager.instancia != null)
        {
            GameManager.instancia.AtualizarVidas(vidas);
        }

        if (vidas <= 0 && GameManager.instancia != null)
        {
            GameManager.instancia.Derrota();
        }
    }
}