using UnityEngine;

public class NaveMae : MonoBehaviour
{
    public float velocidade = 5f;
    public int pontos = 50;

    private float limiteDireita;

    void Start()
    {
        float alturaCam = Camera.main.orthographicSize;
        float larguraCam = alturaCam * Camera.main.aspect;
        Vector3 centroCam = Camera.main.transform.position;

        limiteDireita = centroCam.x + larguraCam + 1f; // +1 para sumir totalmente fora da tela
    }

    void Update()
    {
        transform.position += Vector3.right * velocidade * Time.deltaTime;

        if (transform.position.x > limiteDireita)
        {
            Destroy(gameObject); // some sem ser destruída
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missil_Player"))
        {
            Destroy(other.gameObject);
            GameManager.instancia.AdicionarPontos(pontos);
            Destroy(gameObject);
        }
    }
}