using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TrapDamage : MonoBehaviour
{
    [SerializeField, Min(1)] private int damage = 1;
    [SerializeField, Min(0.01f)] private float damageInterval = 1f;

    private Tilemap trapTilemap;
    private PlayerHealth playerHealth;
    private Collider2D playerCollider;
    private bool wasTouchingTrap;
    private float nextDamageTime;

    private void Awake()
    {
        trapTilemap = GetComponent<Tilemap>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerCollider = playerHealth.GetComponent<Collider2D>();
        }

        if (playerHealth == null || playerCollider == null)
        {
            Debug.LogError("A TrapDamage precisa encontrar o PlayerHealth e o Collider2D do jogador.", this);
        }
    }

    private void FixedUpdate()
    {
        bool touchingTrap = playerHealth != null &&
            playerCollider != null &&
            IsTouchingTrapTile(playerCollider.bounds);

        if (!touchingTrap)
        {
            wasTouchingTrap = false;
            return;
        }

        if (!wasTouchingTrap || Time.time >= nextDamageTime)
        {
            playerHealth.TakeDamage(damage);
            nextDamageTime = Time.time + damageInterval;
        }

        wasTouchingTrap = true;
    }

    private bool IsTouchingTrapTile(Bounds playerBounds)
    {
        playerBounds.Expand(0.04f);

        Vector3Int minimumCell = trapTilemap.WorldToCell(playerBounds.min) - Vector3Int.one;
        Vector3Int maximumCell = trapTilemap.WorldToCell(playerBounds.max) + Vector3Int.one;

        for (int x = minimumCell.x; x <= maximumCell.x; x++)
        {
            for (int y = minimumCell.y; y <= maximumCell.y; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, minimumCell.z);
                if (!trapTilemap.HasTile(cell))
                {
                    continue;
                }

                Vector3 center = trapTilemap.GetCellCenterWorld(cell);
                Vector3 horizontalCellSize = trapTilemap.CellToWorld(cell + Vector3Int.right) -
                    trapTilemap.CellToWorld(cell);
                Vector3 verticalCellSize = trapTilemap.CellToWorld(cell + Vector3Int.up) -
                    trapTilemap.CellToWorld(cell);
                Bounds tileBounds = new Bounds(
                    center,
                    new Vector3(Mathf.Abs(horizontalCellSize.x), Mathf.Abs(verticalCellSize.y), 0f));
                tileBounds.Expand(0.04f);

                if (playerBounds.Intersects(tileBounds))
                {
                    return true;
                }
            }
        }

        return false;
    }
}