using UnityEngine;
using Core;
using Core.Grid;

public class GridCellFactory : MonoBehaviour
{
    [SerializeField] 
    private GameObject gridCellPrefab;

    public GridCell CreateGridCell(Vector3 position, HexCoordinates coordinates)
    {
        if (gridCellPrefab == null)
        {
            Debug.LogError("GridCellFactory: gridCellPrefab未设置!");
            return null;
        }

        GameObject cellObject = Instantiate(gridCellPrefab, position, Quaternion.identity);
        GridCell gridCell = cellObject.GetComponent<GridCell>();

        if (gridCell != null)
        {
            gridCell.Configure(coordinates);
        }
        else
        {
            Debug.LogError("GridCellFactory: 预制体缺少GridCell组件!");
            Destroy(cellObject);
            return null;
        }

        return gridCell;
    }
}