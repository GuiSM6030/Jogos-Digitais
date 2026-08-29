using UnityEngine;

public class PuckControl : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float maxSpeed = 15f;
    public AudioSource hitSound;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hitSound = GetComponent<AudioSource>();
        GoBall();
    }

    // Lança a bola
    void GoBall()
    {
        float rand = Random.Range(0, 2);
        if (rand < 1)
        {
            rb2d.AddForce(new Vector2(20, -15));
        }
        else
        {
            rb2d.AddForce(new Vector2(-20, -15));
        }
    }

    // Colisão com as raquetes
    void OnCollisionEnter2D(Collision2D coll)
    {
        // Toca o som
        if (hitSound != null)
            hitSound.Play();

        // Colisão com os players (raquetes)
        if (coll.collider.CompareTag("Player") || coll.collider.CompareTag("AI"))
        {
            Vector2 vel;
            vel.x = rb2d.linearVelocity.x;
            vel.y = (rb2d.linearVelocity.y / 2) + (coll.collider.attachedRigidbody.linearVelocity.y / 3);
            rb2d.linearVelocity = vel;
        }

        // Limita a velocidade máxima
        if (rb2d.linearVelocity.magnitude > maxSpeed)
        {
            rb2d.linearVelocity = rb2d.linearVelocity.normalized * maxSpeed;
        }
    }

    // Reseta a bola
    void ResetBall()
    {
        rb2d.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    // Reinicia o jogo
    void RestartGame()
    {
        ResetBall();
        Invoke("GoBall", 1);
    }
}