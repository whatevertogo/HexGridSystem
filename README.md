# Hex Grid System (六边形网格系统)

一个基于 Unity 的可扩展六边形网格系统，支持地形管理、网格渲染和交互功能。

## 功能特性

- 🔷 高效的六边形坐标系统
- 🎨 支持自定义地形类型和外观
- 🖌 可自定义的网格渲染和边线效果
- 📐 智能的网格生成和管理
- 🎯 内置碰撞检测系统
- 🔄 支持动态更新和修改
- 🎲 预留骰子放置系统接口
- 🏰 预留文明占领系统接口

## 系统结构

### 核心组件

1. **HexCell.cs**
   - 六边形单元格的核心类
   - 管理单元格的状态、地形和组件
   - 提供邻居单元格访问接口
   - 支持骰子放置和文明占领功能（预留接口）

2. **HexCoordinates.cs**
   - 六边形坐标系统实现
   - 支持世界坐标和网格坐标转换
   - 提供邻居坐标计算功能

3. **HexMesh.cs**
   - 负责网格的生成和渲染
   - 支持自定义边线效果
   - 动态网格更新系统

4. **TerrainSystem**
   - `TerrainType.cs`: 地形类型定义
   - `TerrainSettings.cs`: 地形配置管理
   - `TerrainCellComponent.cs`: 单元格地形组件

### 工厂模式

- **GridCellFactory.cs**
  - 负责创建和初始化六边形单元格
  - 确保单元格组件的正确配置

### 着色器系统

- **HexGridShader**
  - 自定义网格渲染
  - 支持边线效果
  - 可配置的材质属性

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
=======

1. 克隆仓库到本地：

   ```bash
   git clone https://github.com/whatevertogo/HexGridSystem.git
   ```

2. 将整个 Grid 文件夹导入到你的 Unity 项目中的 Assets 目录下

3. 确保项目使用的 Unity 版本兼容（推荐 Unity 2020.3 或更高版本）

### 2. 创建六边形网格

1. 创建网格管理器
   - 在 Hierarchy 窗口中右键，选择 Create Empty 创建空物体
   - 将其命名为 "GridManager"
   - 在 Inspector 窗口中点击 Add Component，添加以下组件：
     - Grid Manager 脚本
     - Hex Mesh 脚本
     - Mesh Filter
     - Mesh Renderer

2. 配置材质
   - 在 Project 窗口中找到 Shader/hexGridMat 材质
   - 将此材质拖拽到 GridManager 的 Mesh Renderer 组件的 Materials 字段中
   - 在材质设置中可以调整：
     - Line Color：网格线颜色
     - Line Width：网格线宽度
     - Use Texture：是否使用贴图

### 3. 配置地形系统

1. 创建地形配置文件
   - 在 Project 窗口中右键
   - 选择 Create > Game > Terrain Settings
   - 命名为 "TerrainSettings"

2. 配置地形类型
   - 在 Inspector 中编辑 TerrainSettings
   - 设置每种地形类型的：
     - Type：地形类型（Plains、Water、Mountain等）
     - Display Name：显示名称
     - Color：地形颜色
     - Description：地形描述

3. 确保 TerrainSettings 被正确引用：
   - 将 TerrainSettings 文件拖拽到 Resources 文件夹中
   - 或直接将其赋值给 TerrainCellComponent 的 Terrain Settings 字段

### 4. 配置六边形单元格

1. 设置 HexCell 预制体
   - 找到 HexCell 预制体
   - 确保它包含以下组件：
     - HexCell 脚本
     - TerrainCellComponent 脚本
     - Polygon Collider 2D
     - Mesh Filter
     - Mesh Renderer

2. 配置 GridCellFactory
   - 选中 GridManager 物体
   - 添加 GridCellFactory 组件
   - 将 HexCell 预制体拖拽到 Grid Cell Prefab 字段中

### 5. 运行时配置

在 Grid Manager 组件中设置：

- Width：网格宽度（列数）
- Height：网格高度（行数）
- Cell Size：单元格大小
- Default Color：默认颜色
- Line Color：网格线颜色
- Line Width：网格线宽度

## 使用提示

1. 地形操作

   ```csharp
   // 为单元格添加地形
   hexCell.AddTerrain(TerrainType.Plains);
   
   // 检查单元格地形
   bool isWater = hexCell.HasTerrain(TerrainType.Water);
   ```

2. 获取相邻单元格

   ```csharp
   // 获取东北方向的相邻格子
   HexCell neighbor = hexCell.GetNeighbor(HexDirection.NE);
   ```

3. 坐标转换

   ```csharp
   // 从世界坐标获取六边形坐标
   HexCoordinates coords = HexCoordinates.FromPosition(worldPosition);
   ```

## 常见问题

1. 网格不可见
   - 检查 Camera 是否正确朝向网格
   - 确认材质是否正确设置
   - 验证网格大小设置是否合理

2. 地形显示异常
   - 确保 TerrainSettings 配置正确
   - 检查 TerrainCellComponent 的引用
   - 验证颜色设置是否正确

3. 碰撞检测失效
   - 确认 PolygonCollider2D 已正确配置
   - 检查碰撞层级设置
   - 验证单元格大小是否合适

## 其他说明

1. 性能优化
   - 大型网格建议使用对象池
   - 考虑启用网格合批（Mesh Batching）
   - 适当调整更新频率

2. 扩展功能
   - 骰子系统通过 IDiceContainer 接口扩展
   - 文明系统通过预留的接口实现
   - 可自定义新的地形类型
   - 如果需要立体的建议自己改xz轴
