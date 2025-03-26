# HexGridSystem-

#### [EN](README.EN.md)

这个项目是一个六边形网格系统，可用于在Unity游戏引擎中创建六边形地图。它提供了一组C#脚本，包括HexCell、HexCoordinates、HexGrid、HexGridHighlight、HexMesh和HexMetrics等，可以帮助开发者快速构建六边形网格地图。
提示：这里是3d的六边形网格系统，包含了立方体坐标系的实现，支持动态生成六边形网格的网格模型，并提供了高亮和选择功能。
如果你需要变成2d的请自行修改，或者使用2d的六边形网格系统。

## 主要功能点

- **HexCell**: 定义六边形网格单元的基本属性和行为
- **HexCoordinates**: 实现立方体坐标系统，便于六边形网格的定位和计算
- **HexGrid**: 管理六边形网格的创建、布局和更新
- **HexGridHighlight**: 提供网格单元的高亮和选择功能
- **HexMesh**: 动态生成六边形网格的网格模型，支持定制外观
- **HexMetrics**: 定义六边形几何参数和常量，确保网格一致性

## 技术栈

- C#
- Unity游戏引擎

## 使用方法

1. 将HexSystem文件夹导入到Unity项目中
2. 在场景中创建一个空游戏对象，添加HexGrid组件
3. 配置网格大小、单元格预制体等参数
4. 运行场景即可查看和交互六边形网格

## 许可证

MIT许可证
