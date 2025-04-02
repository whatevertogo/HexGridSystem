# HexGridSystem

#### [EN](README.EN.md)

一个基于Unity的3D六边形网格系统，提供完整的六边形地图创建、管理和交互功能。

## 系统概述

HexGridSystem是一个模块化的六边形网格系统，专为Unity开发的游戏提供灵活且强大的地图系统。它支持：

- 3D六边形网格的动态生成和管理
- 高效的立方体坐标系统
- 完整的地形系统
- 网格单元的高亮和选择功能
- 灵活的格子管理和交互系统

## 核心功能

### 1. 网格系统
- **动态网格生成**: 支持任意大小的六边形地图创建
- **自定义网格参数**: 可调整六边形大小、间距等参数
- **网格立方体坐标系**: 高效的六边形坐标转换和计算

### 2. 地形系统
- **多地形支持**: 内置水域、山地、平原等地形类型
- **地形叠加**: 支持地形类型的组合
- **扩展性**: 易于添加新的地形类型

### 3. 交互系统
- **选择高亮**: 支持鼠标悬停和选择效果
- **事件系统**: 提供完整的格子交互事件
- **碰撞检测**: 精确的六边形碰撞边界

## 系统要求

- Unity 2020.3 或更高版本
- .NET Framework 4.x

## 快速开始

### 1. 安装
1. 克隆或下载此仓库
2. 将整个 HexGridSystem 文件夹复制到你的 Unity 项目的 Assets 目录中

### 2. 基础设置
1. 创建场景
   ```
   在Unity中创建新场景
   ```

2. 创建网格管理器
   ```
   创建空物体并命名为 "HexGrid"
   添加 HexGrid 组件到该物体
   如果你需要其他系统与HexGrid交互，也许你可以像我一样写一个上下文类或者交互Component
   ```

3. 配置参数
   ```
   设置网格尺寸 (width, height)
   调整格子间距 (cellSpacing)
   指定网格预制体 (GridCell Prefab)
   ```

### 3. 代码示例

1. 创建网格
```csharp
// 获取或创建网格管理器
HexGrid hexGrid = FindObjectOfType<HexGrid>();

// 访问特定坐标的格子
HexCoordinates coords = HexCoordinates.FromOffsetCoordinates(x, y);
GridCell cell = hexGrid.GetCellAtCoordinates(coords);
```

2. 地形操作
```csharp
// 设置地形
cell.AddTerrain(TerrainType.Mountain);

// 检查地形
bool isMountain = cell.HasTerrain(TerrainType.Mountain);
```

3. 获取相邻格子
```csharp
var neighbors = hexGrid.GetNeighbors(cell);
foreach (var neighbor in neighbors) {
    // 处理相邻格子
}
```

## 高级功能

### 1. 自定义地形类型
在 `TerrainType.cs` 中扩展地形类型：
```csharp
public enum TerrainType
{
    None = 0,
    Water = 1 << 1,
    Mountain = 1 << 2,
    Plane = 1 << 3,
    // 添加新地形
    Forest = 1 << 4,
    Desert = 1 << 5
}
```

### 2. 调整网格参数
在 `HexMetrics.cs` 中修改网格参数：
```csharp
public static class HexMetrics
{
    public const float outerRadius = 1f;
    public const float innerRadius = outerRadius * 0.866025404f;
}
```

## 优化建议

1. **性能优化**
   - 大型地图考虑使用对象池
   - 实现视野范围内的动态加载
   - 使用网格合并减少绘制调用

2. **内存管理**
   - 及时释放不需要的网格资源
   - 使用共享材质减少内存占用

## 注意事项

- 这是一个3D六边形网格系统，如需2D效果需修改相关代码
- 在大规模地图中注意性能优化
- 确保正确设置所有必要组件和引用

## 技术支持

如有问题，请提交 Issue 或查看示例场景。

## 许可证

MIT许可证 - 详见 [LICENSE](LICENSE)

## 系统集成指南

### 与HexGrid系统交互

在你的项目中有几种方式可以与HexGrid系统进行集成和交互：

### 1. 上下文类模式

创建一个上下文类来管理与HexGrid的交互：

```csharp
public class HexGridContext
{
    private HexGrid hexGrid;
    private Dictionary<HexCoordinates, GameEntity> entityMap;

    public HexGridContext(HexGrid grid)
    {
        hexGrid = grid;
        entityMap = new Dictionary<HexCoordinates, GameEntity>();
    }

    // 在网格上放置游戏实体
    public void PlaceEntity(GameEntity entity, HexCoordinates coordinates)
    {
        if (IsValidPosition(coordinates))
        {
            entityMap[coordinates] = entity;
            GridCell cell = hexGrid.GetCellAtCoordinates(coordinates);
            // 更新单元格状态或视觉反馈
        }
    }

    // 检查移动是否有效
    public bool CanMoveTo(HexCoordinates from, HexCoordinates to)
    {
        GridCell fromCell = hexGrid.GetCellAtCoordinates(from);
        GridCell toCell = hexGrid.GetCellAtCoordinates(to);
        
        return IsValidPosition(to) && !entityMap.ContainsKey(to) 
               && !toCell.HasTerrain(TerrainType.Mountain);
    }

    // 获取范围内的实体
    public List<GameEntity> GetEntitiesInRange(HexCoordinates center, int range)
    {
        var cells = hexGrid.GetCellsInRange(center, range);
        return cells.Where(c => entityMap.ContainsKey(c.Coordinates))
                   .Select(c => entityMap[c.Coordinates])
                   .ToList();
    }
}
```

### 2. 基于组件的集成

创建一个MonoBehaviour组件来处理网格交互：

```csharp
public class HexGridInteractionManager : MonoBehaviour
{
    public HexGrid hexGrid;
    private GridCell selectedCell;
    private List<GridCell> highlightedCells;

    void Start()
    {
        highlightedCells = new List<GridCell>();
        // 订阅网格事件
        hexGrid.OnCellSelected += HandleCellSelection;
    }

    // 处理单元格选择
    private void HandleCellSelection(GridCell cell)
    {
        selectedCell = cell;
        // 清除之前的高亮
        ClearHighlights();
        
        // 高亮移动范围
        var cellsInRange = hexGrid.GetCellsInRange(cell.Coordinates, 3);
        foreach (var rangeCell in cellsInRange)
        {
            if (IsCellValidForMovement(rangeCell))
            {
                HighlightCell(rangeCell);
                highlightedCells.Add(rangeCell);
            }
        }
    }

    // 寻路示例
    public List<GridCell> FindPath(GridCell start, GridCell end)
    {
        // 实现A*或其他寻路算法
        return hexGrid.FindPath(start.Coordinates, end.Coordinates);
    }
}
```

### 3. 基于事件的系统

订阅网格事件实现响应式游戏玩法：

```csharp
public class GameplayManager : MonoBehaviour
{
    private HexGrid hexGrid;
    
    void Start()
    {
        hexGrid = FindObjectOfType<HexGrid>();
        
        // 订阅网格事件
        hexGrid.OnCellSelected += OnCellSelected;
        hexGrid.OnCellHighlighted += OnCellHighlighted;
    }
    
    private void OnCellSelected(GridCell cell)
    {
        // 处理单元格选择
        if (IsUnitSelected && cell.IsValidDestination)
        {
            MoveUnitTo(cell.Coordinates);
        }
    }
    
    private void OnCellHighlighted(GridCell cell)
    {
        // 更新UI或显示单元格信息
        UpdateCellInfoPanel(cell);
    }
}
```

### 最佳实践

1. **关注点分离**
   - 将网格逻辑与游戏逻辑分离
   - 使用接口定义交互契约
   - 实现观察者模式监听网格状态变化

2. **状态管理**
   - 缓存频繁访问的数据
   - 使用事件通知状态变化
   - 根据需要实现撤销/重做系统

3. **性能考虑**
   - 批量更新网格
   - 对视觉效果使用对象池
   - 缓存寻路结果

4. **示例集成模式**

```csharp
public interface IGridInteraction
{
    void OnCellSelected(GridCell cell);
    void OnCellHighlighted(GridCell cell);
    bool IsValidMove(HexCoordinates from, HexCoordinates to);
}

public class TurnBasedGameManager : MonoBehaviour, IGridInteraction
{
    private HexGridContext gridContext;
    private GameState currentState;

    void Start()
    {
        var hexGrid = FindObjectOfType<HexGrid>();
        gridContext = new HexGridContext(hexGrid);
        
        // 初始化游戏状态
        currentState = new GameState(gridContext);
    }

    public void OnCellSelected(GridCell cell)
    {
        // 根据当前游戏状态处理
        currentState.HandleCellSelection(cell);
    }

    public bool IsValidMove(HexCoordinates from, HexCoordinates to)
    {
        return gridContext.CanMoveTo(from, to);
    }
}
```

这些集成模式提供了与HexGrid系统交互的灵活方式，同时保持了清晰的代码架构和关注点分离。
