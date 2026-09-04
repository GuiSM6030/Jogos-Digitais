using UnityEngine;

public class AIControls : MonoBehaviour
{
    public float speed = 8f;

    // Limites laterais
    public float minX = -5.5f;
    public float maxX = 5.5f;

    // Campo da IA
    public float minY = 0.5f;
    public float maxY = 4f;

    private Rigidbody2D rb2d;
    private GameObject puck;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        puck = GameObject.FindGameObjectWithTag("Puck");

        if (puck == null)
        {
            Debug.LogWarning(
                "Não foi encontrado nenhum objeto com a tag Puck."
            );
        }
    }

    void FixedUpdate()
    {
        if (rb2d == null || puck == null)
            return;

        // Posição da bola
        Vector2 targetPosition = puck.transform.position;

        // Impede a IA de perseguir a bola no campo adversário
        targetPosition.x = Mathf.Clamp(
            targetPosition.x,
            minX,
            maxX
        );

        targetPosition.y = Mathf.Clamp(
            targetPosition.y,
            minY,
            maxY
        );

        // Direção até a bola
        Vector2 direction =
            targetPosition - rb2d.position;

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