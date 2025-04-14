using UnityEngine;

namespace Grid.Interfaces
{
    /// <summary>
    /// 网格单元格接口，定义了网格单元格的基本功能
    /// </summary>
    public interface IGridCell
    {
        /// <summary>
        /// 获取单元格的世界坐标位置
        /// </summary>
        /// <returns>单元格的世界坐标位置</returns>
        Vector3 GetPosition();
    }
}
