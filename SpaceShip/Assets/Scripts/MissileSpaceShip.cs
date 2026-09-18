using UnityEngine;

public class MissileSpaceShip : MonoBehaviour
{
    public SpaceShipGame game;
    public float velocidade = 8f;

    void Awake()
    {
        if (game == null)
            game = FindFirstObjectByType<SpaceShipGame>();
    }

    void Update()
    {
        transform.position += Vector3.right * velocidade * Time.deltaTime;
        if (transform.position.x > 6.5f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AsteroidSpaceShip asteroid = other.GetComponent<AsteroidSpaceShip>();
        if (asteroid != null)
        {
            asteroid.ReceberDano();
            Destroy(gameObject);
        }
    }
}
