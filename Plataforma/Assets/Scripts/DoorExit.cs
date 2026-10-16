using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(TilemapRenderer))]
public class DoorExit : MonoBehaviour
{
    private Tilemap doorTilemap;
    private Collider2D playerCollider;
    private bool hasShownMessage;

    private void Awake()
    {
        doorTilemap = GetComponent<Tilemap>();
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerCollider = playerHealth.GetComponent<Collider2D>();
        }
    }

    private void FixedUpdate()
    {
        bool playerIsAtDoor = playerCollider != null && IsOverlappingDoorTile(playerCollider.bounds);

        if (!playerIsAtDoor)
        {
            hasShownMessage = false;
            return;
        }

        if (hasShownMessage)
        {
            return;
        }

        hasShownMessage = true;
        GameStartMenu gameMenu = FindFirstObjectByType<GameStartMenu>();
        if (gameMenu == null)
        {
            Debug.LogError("Nao foi encontrado o GameStartMenu para interagir com a porta.", this);
            return;
        }

        if (CollectibleItem.HasKey)
        {
            AudioManager.PlaySfx("Porta");
            gameMenu.ShowVictory(GameScore.CurrentScore);
        }
        else
        {
            gameMenu.ShowLockedDoor();
        }
    }

    private bool IsOverlappingDoorTile(Bounds playerBounds)
    {
        BoundsInt cells = doorTilemap.cellBounds;
        foreach (Vector3Int cell in cells.allPositionsWithin)
        {
            if (!doorTilemap.HasTile(cell))
            {
                continue;
            }

            Vector3 cellOrigin = doorTilemap.CellToWorld(cell);
            Vector3 cellRight = doorTilemap.CellToWorld(cell + Vector3Int.right);
            Vector3 cellUp = doorTilemap.CellToWorld(cell + Vector3Int.up);
            Vector3 cellSize = new Vector3(
                Mathf.Abs(cellRight.x - cellOrigin.x) + Mathf.Abs(cellUp.x - cellOrigin.x),
                Mathf.Abs(cellRight.y - cellOrigin.y) + Mathf.Abs(cellUp.y - cellOrigin.y),
                0f);

            Bounds tileBounds = new Bounds(doorTilemap.GetCellCenterWorld(cell), cellSize);
            tileBounds.Expand(new Vector3(0.02f, 0.02f, 0f));
            if (playerBounds.Intersects(tileBounds))
            {
                return true;
            }
        }

        return false;
    }
}