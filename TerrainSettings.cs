using UnityEngine;
using System;
using System.Collections.Generic;

namespace Grid
{
    [CreateAssetMenu(fileName = "TerrainSettings", menuName = "Game/Terrain Settings")]
    public class TerrainSettings : ScriptableObject
    {
        [Serializable]
        public class TerrainData
        {
            public TerrainType type;
            public string displayName;
            public Color color = Color.white;
            [TextArea]
            public string description;
        }

        public TerrainData[] terrainTypes;

        private Dictionary<TerrainType, TerrainData> terrainDataMap;

        private void OnEnable()
        {
            InitializeTerrainDataMap();
        }

        private void InitializeTerrainDataMap()
        {
            terrainDataMap = new Dictionary<TerrainType, TerrainData>();
            if (terrainTypes != null)
            {
                foreach (var data in terrainTypes)
                {
                    terrainDataMap[data.type] = data;
                }
            }
        }

        public Color GetTerrainColor(TerrainType type)
        {
            if (terrainDataMap == null)
            {
                InitializeTerrainDataMap();
            }

            if (terrainDataMap.TryGetValue(type, out TerrainData data))
            {
                return data.color;
            }
            return Color.white; // 默认颜色
        }

        public string GetTerrainName(TerrainType type)
        {
            if (terrainDataMap == null)
            {
                InitializeTerrainDataMap();
            }

            if (terrainDataMap.TryGetValue(type, out TerrainData data))
            {
                return data.displayName;
            }
            return type.ToString();
        }

        public string GetTerrainDescription(TerrainType type)
        {
            if (terrainDataMap == null)
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