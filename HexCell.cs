using UnityEngine;
using Core.Interfaces;
using Grid;

[RequireComponent(typeof(TerrainCellComponent))]
// [RequireComponent(typeof(CivilizationTerritoryComponent))]
public class HexCell : MonoBehaviour, IGridCell
{
    public HexCoordinates Coordinates { get; set; }
    public Color color;

    // 存储六边形6个方向上相邻的其他六边形
    [SerializeField]
    private HexCell[] neighbors = new HexCell[6];

    // 缓存组件引用
    private ITerrainCell _terrainComponent;
    private IDiceContainer _diceContainer;// private ICivilizationTerritory _territoryComponent;
    private PolygonCollider2D _polygonCollider;

    [ReadOnly]
    private Civilization civilization; // 用于存储占领该格子的文明

    // 提供只读属性访问组件
    public ITerrainCell TerrainComponent => _terrainComponent;
    public IDiceContainer DiceContainer => _diceContainer;// public ICivilizationTerritory TerritoryComponent => _territoryComponent;

    private void Awake()
    {
        InitializeComponents();
        InitializeHexCollider();
    }

    private void InitializeComponents()
    {
        // 添加网格渲染器组件
        if (GetComponent<MeshRenderer>() == null)
        {
            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }
        if (GetComponent<MeshFilter>() == null)
        {
            gameObject.AddComponent<MeshFilter>();
        }

        // 一次性获取所有需要的组件
        _terrainComponent = GetComponent<TerrainCellComponent>();
        // _territoryComponent = GetComponent<CivilizationTerritoryComponent>();
        _polygonCollider = GetComponent<PolygonCollider2D>();

        // 验证必需组件
        if (_terrainComponent == null)
        {
            Debug.LogError($"[HexCell] 缺少TerrainComponent组件 on {gameObject.name}");
        }
    }

    private void InitializeHexCollider()
    {
        if (_polygonCollider != null)
        {
            // 创建六边形的顶点数组（6个点）
            Vector2[] points = new Vector2[6];
            for (int i = 0; i < 6; i++)
            {
                // 直接使用 x 和 y 坐标，忽略 z
                Vector3 corner = HexMetrics.corners[i];
                points[i] = new Vector2(corner.x, corner.y);
            }

            _polygonCollider.points = points;
            _polygonCollider.isTrigger = false;
        }
    }

    public void Configure(HexCoordinates coordinates)
    {
        Coordinates = coordinates;
        name = $"Cell {coordinates}"; // 设置GameObject的名称，便于在层级视图中识别
    }

    public Vector3 GetPosition() => transform.position;

    /// <summary>
    /// 获取指定方向的相邻格子
    /// </summary>
    /// <param name="direction">方向</param>
    /// <returns>相邻的格子，如果不存在则返回null</returns>
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    /// <summary>
    /// 设置指定方向的相邻格子
    /// </summary>
    /// <param name="direction">方向</param>
    /// <param name="cell">相邻的格子</param>
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        // 以边的枚举值作为下标
        neighbors[(int)direction] = cell;
        // 同时，让相邻格子将自己存储为邻居，方向取反
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        GetComponent<Renderer>().material.color = newColor;
    }


    #region 地形相关方法
    // 地形相关方法
    public void AddTerrain(TerrainType terrain)
    {
        _terrainComponent?.AddTerrainType(this, terrain);
    }

    public bool HasTerrain(TerrainType terrain)
    {
        return _terrainComponent?.HasTerrainType(this, terrain) ?? false;
    }
    #endregion

    #region 骰子相关方法
    // 骰子相关方法
    public bool TryPlaceDice(Dice dice)
    {
        if (_diceContainer?.CanPlaceDice(dice) ?? false)
        {
            _diceContainer.PlaceDice(dice);
            return true;
        }
        return false;
    }

    public Dice RemoveDice()
    {
        return _diceContainer?.RemoveDice();
    }
    #endregion

    //     #region 文明占领等相关方法
    //     public bool TryOccupy(ICivilization civilization)
    //     {
    //         if (_territoryComponent?.CanBeOccupied(civilization) ?? false)
    //         {
    //             _territoryComponent.SetOccupyingCivilization(civilization);
    //             return true;
    //         }
    //         return false;
    //     }
    // /// <summary>
    // /// 得到占领地块的文明
    // /// </summary>
    // /// <returns></returns>
    //     public ICivilization GetOccupyingCivilization()
    //     {
    //         return _territoryComponent?.GetOccupyingCivilization();
    //     }

    //     /// <summary>
    //     /// 调试用，得到格子坐标信息
    //     /// </summary>
    //     /// <returns></returns> <summary>
    //     /// 
    //     /// </summary>
    //     /// <returns></returns>
    //     public override string ToString()
    //     {
    //         return $"HexCell {Coordinates}";
    //     }
    //     #endregion

}