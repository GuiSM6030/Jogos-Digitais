using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private Sprite goldCoinSprite;
    [SerializeField] private Sprite silverCoinSprite;
    [SerializeField] private Sprite jewelsBagSprite;
    [SerializeField] private Sprite coinsBagSprite;
    [SerializeField] private Sprite keySprite;
    [SerializeField] private Sprite lifeSprite;
    [SerializeField, Min(0f)] private float heightOffset = 0.22f;
    [SerializeField, Min(0.1f)] private float itemScale = 1.5f;
    [SerializeField, Min(0f)] private float floatAmplitude = 0.08f;
    [SerializeField, Min(0f)] private float floatFrequency = 2.5f;

    private void Start()
    {
        foreach (Tilemap tilemap in FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
        {
            SpawnItemsOnTilemap(tilemap);
        }
    }

    private void SpawnItemsOnTilemap(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int cell in bounds.allPositionsWithin)
        {
            TileBase marker = tilemap.GetTile(cell);
            if (marker == null)
            {
                continue;
            }

            bool isItemMap = TryGetItem(
                tilemap.gameObject.name,
                out Sprite sprite,
                out int points,
                out bool isKey,
                out bool restoresHealth);
            if (!isItemMap && !TryGetItem(marker.name, out sprite, out points, out isKey, out restoresHealth))
            {
                continue;
            }

            if (sprite == null)
            {
                Debug.LogError($"Atribua o sprite do item marcado como '{tilemap.gameObject.name}'.", this);
                continue;
            }

            SpawnItem(tilemap, cell, tilemap.gameObject.name, sprite, points, isKey, restoresHealth);
            tilemap.SetTile(cell, null);
        }
    }

    private bool TryGetItem(
        string markerName,
        out Sprite sprite,
        out int points,
        out bool isKey,
        out bool restoresHealth)
    {
        switch (markerName.Replace(" ", "").ToLowerInvariant())
        {
            case "moedaouro":
                sprite = goldCoinSprite;
                points = 20;
                isKey = false;
                restoresHealth = false;
                return true;
            case "moedaprata":
                sprite = silverCoinSprite;
                points = 10;
                isKey = false;
                restoresHealth = false;
                return true;
            case "sacojoia":
            case "sacojoias":
                sprite = jewelsBagSprite;
                points = 100;
                isKey = false;
                restoresHealth = false;
                return true;
            case "sacomoeda":
            case "sacomoedas":
                sprite = coinsBagSprite;
                points = 50;
                isKey = false;
                restoresHealth = false;
                return true;
            case "chave":
                sprite = keySprite;
                points = 0;
                isKey = true;
                restoresHealth = false;
                return true;
            case "vida":
                sprite = lifeSprite;
                points = 0;
                isKey = false;
                restoresHealth = true;
                return true;
            default:
                sprite = null;
                points = 0;
                isKey = false;
                restoresHealth = false;
                return false;
        }
    }

    private void SpawnItem(
        Tilemap tilemap,
        Vector3Int cell,
        string markerName,
        Sprite sprite,
        int points,
        bool isKey,
        bool restoresHealth)
    {
        Vector3 position = tilemap.GetCellCenterWorld(cell);
        Vector3 cellHeight = tilemap.CellToWorld(cell + Vector3Int.up) - tilemap.CellToWorld(cell);
        position.y += Mathf.Abs(cellHeight.y) * 0.5f + heightOffset;

        GameObject itemObject = new GameObject($"Coletavel - {markerName}");
        itemObject.transform.position = position;
        itemObject.transform.localScale = Vector3.one * itemScale;

        SpriteRenderer spriteRenderer = itemObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        TilemapRenderer tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
        spriteRenderer.sortingOrder = tilemapRenderer != null ? tilemapRenderer.sortingOrder + 1 : 1;

        BoxCollider2D itemCollider = itemObject.AddComponent<BoxCollider2D>();
        itemCollider.size = sprite.bounds.size;
        itemCollider.isTrigger = true;

        CollectibleItem collectible = itemObject.AddComponent<CollectibleItem>();
        collectible.Configure(points, isKey, restoresHealth, floatAmplitude, floatFrequency);
    }
}