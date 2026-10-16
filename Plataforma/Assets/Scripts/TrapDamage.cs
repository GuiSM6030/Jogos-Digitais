using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(TilemapCollider2D))]
public class TrapDamage : MonoBehaviour
{
    [SerializeField, Min(1)] private int damage = 1;
    [SerializeField, Min(0.01f)] private float damageInterval = 1f;

    private readonly Collider2D[] overlappingColliders = new Collider2D[16];
    private PlayerHealth playerHealth;
    private TilemapCollider2D trapCollider;
    private ContactFilter2D overlapFilter;
    private float nextDamageTime;

    private void Awake()
    {
        trapCollider = GetComponent<TilemapCollider2D>();
        overlapFilter = new ContactFilter2D();
        overlapFilter.useTriggers = true;
        overlapFilter.SetLayerMask(Physics2D.AllLayers);
    }

    private void FixedUpdate()
    {
        int overlapCount = trapCollider.Overlap(overlapFilter, overlappingColliders);
        bool playerIsTouching = false;

        for (int index = 0; index < overlapCount; index++)
        {
            PlayerHealth touchingPlayer = overlappingColliders[index].GetComponentInParent<PlayerHealth>();
            if (touchingPlayer == null)
            {
                continue;
            }

            playerIsTouching = true;
            if (Time.time >= nextDamageTime)
            {
                playerHealth = touchingPlayer;
                playerHealth.TakeDamage(damage);
                nextDamageTime = Time.time + damageInterval;
            }

            break;
        }

        if (!playerIsTouching)
        {
            playerHealth = null;
            nextDamageTime = 0f;
        }
    }
}