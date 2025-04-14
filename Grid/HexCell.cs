using GameData;
using Grid;
using UnityEngine;

[RequireComponent(typeof(TerrainCellComponent))]
[RequireComponent(typeof(SettlementHexGridComponent))]
[RequireComponent(typeof(PolygonCollider2D))]
public class HexCell : MonoBehaviour, IGridCell
{
    public HexCoordinates coordinates { get; set; }

    public Vector3 GetPosition() => transform.position;

    public Color color;


    [SerializeField]
    private HexCell[] neighbors = new HexCell[6];

    //组件
    private TerrainCellComponent _terrainComponent;
    private SettlementHexGridComponent _settlementnHexGridComponent;

    public PolygonCollider2D PolygonCollider2D { get; private set; }

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        // // 添加事件监听
        // if (_terrainComponent != null)
        //     _terrainComponent.OnTerrainChanged += OnTerrainChanged;

        //订阅占领状态变化事件
        if (_settlementnHexGridComponent != null)
            _settlementnHexGridComponent.OnOccupationChanged += OnOccupationChanged;
    }

    private void InitializeComponents()
    {
        _terrainComponent = GetComponent<TerrainCellComponent>();
        _settlementnHexGridComponent = GetComponent<SettlementHexGridComponent>();
        PolygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    public void Configure(HexCoordinates coordinates)
    {
        this.coordinates = coordinates;
        name = $"Cell {coordinates}";
    }

    #region 颜色管理
    public void SetColor(Color newColor)
    {
        if (color != newColor)
        {
            color = newColor;
            // 标记单元格为脏，需要更新
            GridManager.Instance?.MarkCellDirty(this);
        }
    }

    public Color GetColor()
    {
        return color;
    }
    #endregion

    #region 邻居管理
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public HexCell[] GetAllNeighbors()
    {
        return neighbors;
    }

    public bool HasNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction] is not null;
    }
    #endregion

    #region 地形相关
    /// <summary>
    /// 设置地形
    /// </summary>
    public void SetTerrain(TerrainType terrain)
    {
        _terrainComponent?.SetTerrain(terrain);
    }

    /// <summary>
    /// 获取地形类型
    /// </summary>
    public TerrainType GetTerrain()
    {
        // 兼容旧逻辑
        return _terrainComponent?.GetTerrain() ?? TerrainType.None;
    }
    #endregion

    #region 文明占领相关
    public Settlement GetOccupyingSettlement()
    {
        return _settlementnHexGridComponent?.GetOccupyingSettlement();
    }

    public bool CanBeOccupiedBySettlemnt(Settlement settlement)
    {
        return _settlementnHexGridComponent?.CanBeOccupied(settlement) ?? false;
    }
    #endregion

    public override string ToString()
    {
        return $"HexCell {coordinates}";
    }

    #region 暂时无用
    private void OnOccupationChanged(
        object sender,
        SettlementHexGridComponent.OccupationChangedEventArgs e
    )
    {
        // 处理占领状态变化
        if (e.NewSettlement != null)
        {
            Debug.Log($"Cell {coordinates} occupied by {e.NewSettlement.GetName()}");
        }
        else
        {
            Debug.Log($"Cell {coordinates} is now unoccupied");
        }
        
    }
    #endregion
}
