using UnityEngine;
using Core;

public class GridCellFactory : MonoBehaviour
{
    [SerializeField] 
    private GameObject gridCellPrefab;

    public HexCell CreateGridCell(Vector3 position, HexCoordinates coordinates)
    {
        if (gridCellPrefab == null)
        {
            Debug.LogError("GridCellFactory: gridCellPrefab未设置!");
            return null;
        }

        GameObject cellObject = Instantiate(gridCellPrefab, position, Quaternion.identity);
        HexCell hexCell = cellObject.GetComponent<HexCell>();

        if (hexCell != null)
        {
            hexCell.Configure(coordinates);
        }
        else
        {
            Debug.LogError("GridCellFactory: 预制体缺少GridCell组件!");
            Destroy(cellObject);
            return null;
        }

        return hexCell;
    }
}