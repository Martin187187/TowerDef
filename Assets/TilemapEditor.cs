using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase defaultTile;

    void Start()
    {
        // Example: Set a specific tile at a given position
        Vector3Int tilePosition = new Vector3Int(0, 0, 0);
        tilemap.SetTile(tilePosition, defaultTile);

        // Example: Loop through a region and set default tile
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                tilemap.SetTile(position, defaultTile);
            }
        }
    }
}