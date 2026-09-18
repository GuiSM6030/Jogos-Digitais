using UnityEngine;

public class Parallax : MonoBehaviour
{
    public static float SpeedMultiplier { get; set; } = 1f;
    public float parallaxEffect = 0.5f;

    private float length;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            length = spriteRenderer.bounds.size.x;
        }
    }

    void Update()
    {
        if (length <= 0f)
            return;

        transform.position += Vector3.left * Time.deltaTime * parallaxEffect * SpeedMultiplier;

        if (transform.position.x < -length)
        {
            transform.position = new Vector3(length, transform.position.y, transform.position.z);
        }
    }
}
