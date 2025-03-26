using UnityEngine;
using UnityEngine.Tilemaps;

public class HexGridHighlight : MonoBehaviour
{
    public Camera mainCamera;
    public Tilemap hexTilemap;
    public Tile normalTile;
    public Tile highlightTile;
    public int offset;

    private Vector3Int lastHoverPosition;

    void Update()
    {
        //Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //Vector3Int position = hexTilemap.WorldToCell(mousePosition);
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
        //mousePosition.y -= hexTilemap.cellSize.y; // ����hexTilemap.tileAnchor.y��0.5��cellSize.y����Ƭ�ĸ߶�
        Vector3Int position = hexTilemap.WorldToCell(mousePosition);
        position.x += offset;

        if (position != lastHoverPosition)  
        {
            // ���֮ǰ�ĸ���
            if (lastHoverPosition != Vector3Int.zero)
            {
                hexTilemap.SetTile(lastHoverPosition, normalTile);
            }

            // �����µĸ���
            if (hexTilemap.HasTile(position))
            {
                hexTilemap.SetTile(position, highlightTile);
            }

            lastHoverPosition = position;
        }
    }
}