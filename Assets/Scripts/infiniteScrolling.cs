using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Infinite scrolling background with diagonal motion.
/// Attach this to a parent object containing multiple tiled background pieces.
/// Tiles should cover a rectangular grid (e.g., 2x2 or 3x3) to allow recycling in both directions.
/// </summary>
public class InfiniteScroling : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Scroll speed in world units per second.")]
    public float speed = 2f;

    [Tooltip("Direction of scrolling (normalized automatically).")]
    public Vector2 direction = new Vector2(0, 1); // e.g. (1, -1) = down-right

    [Header("Tile Settings")]
    [Tooltip("Width of one tile (auto-detected if <= 0).")]
    public float tileWidth = 0f;
    [Tooltip("Height of one tile (auto-detected if <= 0).")]
    public float tileHeight = 0f;

    private List<Transform> tiles = new List<Transform>();
    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
        tiles.Clear();

        foreach (Transform child in transform)
            tiles.Add(child);

        if (tiles.Count == 0)
        {
            Debug.LogWarning("DiagonalScrollingTiler: No child tiles found under " + name);
            enabled = false;
            return;
        }

        // Auto-detect tile size from first child's renderer
        Renderer r = tiles[0].GetComponent<Renderer>();
        if (r != null)
        {
            if (tileWidth <= 0f) tileWidth = r.bounds.size.x;
            if (tileHeight <= 0f) tileHeight = r.bounds.size.y;
        }

        //// Normalize scroll direction
        //if (direction.sqrMagnitude > 0)
        //    direction.Normalize();
        //else
        //    direction = new Vector2(1, -1).normalized;
    }

    void Update()
    {
        // Move the entire group diagonally
        Vector3 move = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        transform.position += move;

        RecycleTiles();
    }

    private void RecycleTiles()
    {
        if (tiles.Count == 0) return;

        // Determine camera world rect
        Vector3 camBL = mainCam.ViewportToWorldPoint(new Vector3(0, 0, Mathf.Abs(mainCam.transform.position.z - transform.position.z)));
        Vector3 camTR = mainCam.ViewportToWorldPoint(new Vector3(1, 1, Mathf.Abs(mainCam.transform.position.z - transform.position.z)));

        float camLeft = camBL.x;
        float camRight = camTR.x;
        float camBottom = camBL.y;
        float camTop = camTR.y;

        foreach (Transform t in tiles)
        {
            Vector3 pos = t.position;

            // Move tile from right to left or bottom to top when out of view
            if (pos.x + tileWidth / 2 < camLeft - tileWidth)
                pos.x += tileWidth * Mathf.CeilToInt(ScreenWidthInTiles());
            else if (pos.x - tileWidth / 2 > camRight + tileWidth)
                pos.x -= tileWidth * Mathf.CeilToInt(ScreenWidthInTiles());

            if (pos.y + tileHeight / 2 < camBottom - tileHeight)
                pos.y += tileHeight * Mathf.CeilToInt(ScreenHeightInTiles());
            else if (pos.y - tileHeight / 2 > camTop + tileHeight)
                pos.y -= tileHeight * Mathf.CeilToInt(ScreenHeightInTiles());

            t.position = pos;
        }
    }

    private float ScreenWidthInTiles()
    {
        float screenWorldWidth = mainCam.orthographicSize * 2f * mainCam.aspect;
        return screenWorldWidth / tileWidth + 2; // +2 margin
    }

    private float ScreenHeightInTiles()
    {
        float screenWorldHeight = mainCam.orthographicSize * 2f;
        return screenWorldHeight / tileHeight + 2;
    }
}
