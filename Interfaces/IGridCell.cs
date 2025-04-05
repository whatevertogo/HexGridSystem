using UnityEngine;

namespace Grid
{
    /// <summary>
    /// 网格单元格的基础接口
    /// </summary>
    public interface IGridCell
    {
        Vector3 GetPosition();
        HexCoordinates Coordinates { get; set; }
    }
}