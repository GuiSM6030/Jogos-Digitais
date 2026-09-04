using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 10f;

    // Limites laterais da mesa
    public float minX = -5.5f;
    public float maxX = 5.5f;

    // Limites do campo do jogador
    public float minY = -4f;
    public float maxY = -0.5f;

    private Rigidbody2D rb2d;
    private Camera mainCamera;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (rb2d == null || mainCamera == null)
            return;

        // Pega a posição do mouse
        Vector3 mousePos = Input.mousePosition;

        // Converte a posição da tela para o mundo
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        // Como o jogo é 2D
        worldPos.z = 0f;

        // Limita o movimento horizontal
        worldPos.x = Mathf.Clamp(
            worldPos.x,
            minX,
            maxX
        );

        // Limita o movimento vertical
        worldPos.y = Mathf.Clamp(
            worldPos.y,
            minY,
            maxY
        );

        // Calcula a direção até o mouse
        Vector2 direction =
            (Vector2)worldPos - rb2d.position;

        // Move somente se houver distância
        if (direction.magnitude > 0.05f)
        {
            direction.Normalize();

            rb2d.linearVelocity = direction * speed;
        }
        else
        {
            rb2d.linearVelocity = Vector2.zero;
        }
    }
}