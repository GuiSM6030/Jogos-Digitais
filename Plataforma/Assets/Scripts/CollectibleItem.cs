using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public static bool HasKey { get; private set; }

    private int points;
    private bool isKey;
    private bool restoresHealth;
    private bool collected;
    private float startingY;
    private float floatAmplitude;
    private float floatFrequency;
    private float floatPhase;

    private void Awake()
    {
        startingY = transform.position.y;
        floatPhase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position.y = startingY + Mathf.Sin(Time.unscaledTime * floatFrequency + floatPhase) * floatAmplitude;
        transform.position = position;
    }

    public void Configure(int itemPoints, bool itemIsKey, bool itemRestoresHealth, float amplitude, float frequency)
    {
        points = itemPoints;
        isKey = itemIsKey;
        restoresHealth = itemRestoresHealth;
        floatAmplitude = amplitude;
        floatFrequency = frequency;
    }

    public static void ResetKey()
    {
        HasKey = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (collected || playerHealth == null)
        {
            return;
        }

        collected = true;
        if (isKey)
        {
            HasKey = true;
        }

        if (points > 0)
        {
            GameScore.AddPoints(points);
        }

        if (restoresHealth)
        {
            playerHealth.RestoreHealthToFull();
        }

        Destroy(gameObject);
    }
}