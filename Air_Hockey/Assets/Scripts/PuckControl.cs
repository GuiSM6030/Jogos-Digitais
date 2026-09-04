using UnityEngine;

public class PuckControl : MonoBehaviour
{
    public float maxSpeed = 15f;

    private Rigidbody2D rb2d;
    private AudioSource source;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();

        GoBall();
    }

    void GoBall()
    {
        float directionX = Random.Range(0, 2) == 0 ? -1f : 1f;

        rb2d.linearVelocity = new Vector2(
            directionX * 8f,
            Random.Range(-6f, 6f)
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Som da colisão
        if (source != null)
        {
            source.Play();
        }

        // Colisão com os jogadores
        if (collision.collider.CompareTag("Player") ||
            collision.collider.CompareTag("AI"))
        {
            Rigidbody2D playerRb = collision.collider.attachedRigidbody;

            float newYVelocity = rb2d.linearVelocity.y;

            if (playerRb != null)
            {
                newYVelocity += playerRb.linearVelocity.y * 0.5f;
            }

            rb2d.linearVelocity = new Vector2(
                rb2d.linearVelocity.x,
                newYVelocity
            );
        }

        // Limita velocidade
        if (rb2d.linearVelocity.magnitude > maxSpeed)
        {
            rb2d.linearVelocity =
                rb2d.linearVelocity.normalized * maxSpeed;
        }
    }

    public void ResetBall()
    {
        CancelInvoke();

        rb2d.linearVelocity = Vector2.zero;
        rb2d.angularVelocity = 0f;

        transform.position = Vector2.zero;
    }

    public void RestartGame()
    {
        ResetBall();

        Invoke(nameof(GoBall), 1f);
    }
}