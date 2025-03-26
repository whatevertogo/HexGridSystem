using UnityEngine;

/// <summary>
/// HexCell类
/// </summary>
public class HexCell : MonoBehaviour
{
    /// <summary>
    /// 创造一个表示立方体坐标的结构体，并提供获得坐标的方法
    /// </summary>
    public HexCoordinates coordinates;

    // 添加更多游戏相关属性
    public enum TerrainType { Plains, Mountains, Forest, Water, Desert }
    public TerrainType terrain = TerrainType.Plains;

    // 视觉状态
    public bool isSelectedHighlighted;

    // 资源或建筑
    public string resourceType;
    public GameObject buildingInstance;

}
