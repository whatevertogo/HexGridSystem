using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(fileName = "TerrainSettings", menuName = "Game/Terrain Settings")]
    public class TerrainSettings : ScriptableObject
    {
        [Serializable]
        public class TerrainData
        {
            public TerrainType type;
            public Color color = Color.white;
            public string displayName;

            [TextArea(3, 5)]
            public string description;
        }

        [SerializeField]
        private List<TerrainData> terrainDataList = new List<TerrainData>();
        private Dictionary<TerrainType, TerrainData> terrainDataMap;

        private void OnEnable()
        {
            InitializeTerrainDataMap();
        }

        private void InitializeTerrainDataMap()
        {
            terrainDataMap = new Dictionary<TerrainType, TerrainData>();
            foreach (var data in terrainDataList)
            {
                if (!terrainDataMap.ContainsKey(data.type))
                {
                    terrainDataMap[data.type] = data;
                }
            }
        }

        public Color GetTerrainColor(TerrainType type)
        {
            if (terrainDataMap is null)
            {
                InitializeTerrainDataMap();
            }

            if (terrainDataMap.TryGetValue(type, out TerrainData data))
            {
                return data.color;
            }

            // 如果找不到对应的地形数据，返回默认颜色
            return Color.white;
        }

        public string GetTerrainName(TerrainType type)
        {
            if (terrainDataMap is null)
            {
                InitializeTerrainDataMap();
            }

            if (terrainDataMap.TryGetValue(type, out TerrainData data))
            {
                return !string.IsNullOrEmpty(data.displayName) ? data.displayName : type.ToString();
            }

            return type.ToString();
        }

        public string GetTerrainDescription(TerrainType type)
        {
            if (terrainDataMap is null)
            {
                InitializeTerrainDataMap();
            }

            if (terrainDataMap.TryGetValue(type, out TerrainData data))
            {
                return data.description;
            }

            return string.Empty;
        }
    }
}
