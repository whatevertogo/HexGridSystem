using CDTU.Utils;
using UnityEngine;

namespace Grid
{
    /// <summary>
    /// 此组件负责管理地形类型和相关的视觉效果
    /// 通过 HexCell 组件与地形类型进行交互
    /// 该组件可以添加、移除和检查地形类型，并更新地形的视觉效果
    /// </summary>
    public class TerrainCellComponent : MonoBehaviour
    {
        [SerializeField]
        private TerrainSettings terrainSettings;
        private TerrainType terrainType;
        private HexCell hexCell;

        #region 地形改变
        // public event EventHandler<OnTerrainChangedEventArgs> OnTerrainChanged;

        // public class OnTerrainChangedEventArgs : EventArgs
        // {
        //     public HexCell Cell { get; }
        //     public TerrainType NewTerrain { get; }
        //     public TerrainType OldTerrain { get; }

        //     public OnTerrainChangedEventArgs(HexCell cell, TerrainType newTerrain, TerrainType oldTerrain)
        //     {
        //         Cell = cell;
        //         NewTerrain = newTerrain;
        //         OldTerrain = oldTerrain;
        //     }
        // }
        #endregion


        private void Awake()
        {
            hexCell = GetComponent<HexCell>();
            if (terrainSettings is null)
            {
                //todo-如果没有要从resource加载，但是resource还没有实现，这个方式也不太好
                ULogger.LogError("TerrainSettings is null, loading from Resources.");
                var settings = ConfigManager.Instance.TerrainSettings;
            }
        }

        public void SetTerrain(TerrainType type)
        {
            terrainType = type;
            UpdateTerrainVisuals();
        }

        public TerrainType GetTerrain()
        {
            return terrainType;
        }

        private void UpdateTerrainVisuals()
        {
            if (terrainSettings != null && hexCell != null)
            {
                Color terrainColor = terrainSettings.GetTerrainColor(terrainType);
                hexCell.SetColor(terrainColor);
            }
        }

        public void AddTerrainType(HexCell hexCell, TerrainType terrain)
        {
            // 添加地形类型的逻辑
            if (hexCell != null && terrain != TerrainType.None)
            {
                SetTerrain(terrain);
            }
        }

        public void RemoveTerrainType(HexCell hexCell, TerrainType terrain)
        {
            // 移除地形类型的逻辑
            if (hexCell is not null && terrain == terrainType)
            {
                SetTerrain(TerrainType.None);
            }
        }

        public bool HasTerrainType(HexCell hexCell, TerrainType terrain)
        {
            // 检查是否具有特定地形类型的逻辑
            return hexCell is not null && terrain == terrainType;
        }

        public string GetTerrainName()
        {
            return terrainSettings is not null
                ? terrainSettings.GetTerrainName(terrainType)
                : terrainType.ToString();
        }

        public string GetTerrainDescription()
        {
            return terrainSettings is not null
                ? terrainSettings.GetTerrainDescription(terrainType)
                : string.Empty;
        }
    }
}
