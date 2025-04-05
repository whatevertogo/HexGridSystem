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
