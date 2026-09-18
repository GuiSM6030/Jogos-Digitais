using UnityEngine;

public class AsteroidSpaceShip : MonoBehaviour
{
    public SpaceShipGame game;
    public int vidas = 1;
    public int pontos = 10;
    public float velocidade = 2.4f;
    public float rotacao = 35f;

    void Awake()
    {
        if (game == null)
            game = FindFirstObjectByType<SpaceShipGame>();
    }

    void Update()
    {
        transform.position += Vector3.left * velocidade * SpaceShipGame.WorldSpeedMultiplier * Time.deltaTime;
        transform.Rotate(0f, 0f, rotacao * SpaceShipGame.WorldSpeedMultiplier * Time.deltaTime);

        if (transform.position.x < -6.5f)
        {
            game.PerderVida();
            Destroy(gameObject);
        }
    }

    public void ReceberDano()
    {
        vidas--;
        if (vidas <= 0)
        {
            game.AdicionarPontos(pontos);
            Destroy(gameObject);
        }
    }
}
