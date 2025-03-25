using UnityEngine;
using System.Collections.Generic;
using System;

public class HexGrid : MonoBehaviour
{
    [Header("网格设置")]
    [SerializeField] private int width = 6;
    [SerializeField] private int height = 6;
    [SerializeField] private HexCell cellPrefab;
    private HexCell[] cells;

    // 使用字典快速查找单元格
    private Dictionary<HexCoordinates, HexCell> cellLookup = new Dictionary<HexCoordinates, HexCell>();

    [Header("可视化设置")]
    [SerializeField] private float cellSpacing = 1.0f;

    // 添加事件支持
    public event EventHandler<HexCell> OnCellCreated;
    public event EventHandler<HexCell> OnCellSelected;

    // 公开属性
    public int Width => width;
    public int Height => height;
    public int CellCount => cells?.Length ?? 0;

    void Awake()
    {
        InitializeGrid();
    }

    /// <summary>
    /// 初始化整个六边形网格
    /// </summary>
    public void InitializeGrid()
    {
        ClearGrid();
        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    /// <summary>
    /// 清除现有网格
    /// </summary>
    public void ClearGrid()
    {
        if (cells != null && cells.Length > 0)
        {
            foreach (HexCell cell in cells)
            {
                if (cell != null) DestroyImmediate(cell.gameObject);
            }
        }

        cellLookup.Clear();
    }

    /// <summary>
    /// 创建单个六边形单元格
    /// </summary>
    private void CreateCell(int x, int z, int i)
    {
        // 计算六边形位置
        Vector3 position = CalculateCellPosition(x, z);

        // 实例化并配置单元格
        HexCell cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        // 设置坐标并添加到查找表
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cellLookup[cell.coordinates] = cell;

        // 触发创建事件
        OnCellCreated?.Invoke(this,cell);
    }

    /// <summary>
    /// 计算六边形单元格位置
    /// </summary>
    private Vector3 CalculateCellPosition(int x, int z)
    {
        Vector3 position;
        // 偏移坐标转换为世界坐标
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f * cellSpacing);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f * cellSpacing);
        return position;
    }

    // /// <summary>
    // /// 通过索引获取单元格
    // /// </summary>
    // public HexCell GetCellByIndex(int index)
    // {
    //     if (cells != null && index >= 0 && index < cells.Length)
    //         return cells[index];
    //     return null;
    // }

    /// <summary>
    /// 通过偏移坐标获取单元格
    /// </summary>
    public HexCell GetCell(int x, int z)
    {
        if (x < 0 || x >= width || z < 0 || z >= height)
            return null;

        return cells[x + z * width];
    }

    /// <summary>
    /// 通过六边形坐标获取单元格
    /// </summary>
    public HexCell GetCellByCoordinates(HexCoordinates coordinates)
    {
        if (cellLookup.TryGetValue(coordinates, out HexCell cell))
            return cell;
        else
            return null;
    }

    /// <summary>
    /// 选择特定单元格
    /// </summary>
    public void SelectCell(HexCell cell)
    {
        if (cell != null)
        {
            OnCellSelected?.Invoke(this,cell);
        }
    }

    /// <summary>
    /// 获取单元格的邻居
    /// </summary>
    public List<HexCell> GetNeighbors(HexCell cell)
    {
        List<HexCell> neighbors = new List<HexCell>();

        foreach (HexCoordinates.Direction direction in Enum.GetValues(typeof(HexCoordinates.Direction)))
        {
            HexCoordinates neighborCoords = cell.coordinates.GetNeighbor(direction);
            HexCell neighbor = GetCellByCoordinates(neighborCoords);
            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    /// <summary>
    /// 自定义大小创建网格
    /// </summary>
    public void CreateGridWithSize(int newWidth, int newHeight)
    {
        this.width = newWidth;
        this.height = newHeight;
        InitializeGrid();
    }
}