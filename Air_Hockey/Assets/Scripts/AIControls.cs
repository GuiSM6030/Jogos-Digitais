using UnityEngine;

public class AIControls : MonoBehaviour
{
    public float speed = 8f;
    public float boundY = 4f;
    public float boundX = 5.5f;
    public GameObject puck;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        puck = GameObject.FindGameObjectWithTag("Puck");
    }

    void Update()
    {
        if (puck == null) return;

        // A IA segue a bola
        Vector2 targetPosition = puck.transform.position;

        // Limita onde a IA pode ir
        targetPosition.x = Mathf.Clamp(targetPosition.x, -boundX, boundX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -boundY, boundY);

        // Move a IA em direção à bola
        Vector2 newPosition = Vector2.MoveTowards(rb2d.position, targetPosition, speed * Time.deltaTime);
        rb2d.MovePosition(newPosition);
    }
}