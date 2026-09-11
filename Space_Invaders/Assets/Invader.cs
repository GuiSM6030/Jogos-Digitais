using UnityEngine;

public class Invader : MonoBehaviour
{
    public float speed = 2.0f;
    public int pontos = 10;
    public GameObject missilPrefab;

    public void Atirar()
    {
        if (missilPrefab == null) return;

        GameObject missil = Instantiate(missilPrefab, transform.position, Quaternion.identity);
        missil.GetComponent<Missil>().SetDirecao(-1);
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

            if (GameManager.instancia != null)
            {
                GameManager.instancia.AdicionarPontos(pontos);
            }

            Destroy(gameObject);
        }
    }
}