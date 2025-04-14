using System;
using Grid;
using UnityEngine;

/// <summary>
/// 负责创建和配置HexCell实例
/// </summary>
public class GridCellFactory : MonoBehaviour
{
    [SerializeField]
    private HexCell cellPrefab;

    public HexCell CreateGridCell(Vector3 position, HexCoordinates coordinates)
    {
        // 实例化单元格
        HexCell cell = Instantiate(cellPrefab, position, Quaternion.identity);

        if (cell != null)
        {
            // 确保游戏对象处于激活状态
            cell.gameObject.SetActive(true);

            // 配置基本属性
            cell.Configure(coordinates);
            cell.transform.position = position;

            //向聚落管理器注册
            RegisterToSettlement(cell);

            return cell;
        }

        throw new InvalidOperationException("Failed to create cell!");
    }

    /// <summary>
    /// 确保格子拥有所有必需的组件
    /// </summary>
    private void RegisterToSettlement(HexCell cell)
    {
        // 注册到领土管理器
        if (TerritoryManager.Instance is not null)
        {
            TerritoryManager.Instance.RegisterTerritoryComponent(
                cell.gameObject.GetComponent<SettlementHexGridComponent>()
            );
        }
    }

    private void OnDestroy()
    {
        // 清理工作...
    }
}
