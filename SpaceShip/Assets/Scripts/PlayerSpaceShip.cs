using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpaceShip : MonoBehaviour
{
    public SpaceShipGame game;
    public GameObject missilePrefab;
    public float velocidade = 5f;
    public float intervaloTiro = 0.25f;
    public float limiteX = 4.5f;
    public float limiteY = 3.1f;

    private float proximoTiro;

    void Awake()
    {
        if (game == null)
            game = FindFirstObjectByType<SpaceShipGame>();
    }

    void Update()
    {
        if (game == null || game.JogoEncerrado() || Keyboard.current == null)
            return;

        Vector3 movimento = Vector3.zero;
        if (Keyboard.current.leftArrowKey.isPressed) movimento.x -= 1f;
        if (Keyboard.current.rightArrowKey.isPressed) movimento.x += 1f;
        if (Keyboard.current.downArrowKey.isPressed) movimento.y -= 1f;
        if (Keyboard.current.upArrowKey.isPressed) movimento.y += 1f;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && Time.time >= proximoTiro)
        {
            CriarMissil();
            proximoTiro = Time.time + intervaloTiro;
        }

        transform.position += movimento.normalized * velocidade * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -limiteX, limiteX),
            Mathf.Clamp(transform.position.y, -limiteY, limiteY),
            transform.position.z);
    }

    void CriarMissil()
    {
        if (missilePrefab == null)
            return;

        GameObject missile = Instantiate(missilePrefab, transform.position + Vector3.right * 0.7f, Quaternion.identity);
        MissileSpaceShip controller = missile.GetComponent<MissileSpaceShip>();
        controller.game = game;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AsteroidSpaceShip asteroid = other.GetComponent<AsteroidSpaceShip>();
        if (asteroid != null)
        {
            Destroy(asteroid.gameObject);
            game.PerderVida();
        }
    }
}
