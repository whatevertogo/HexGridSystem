using CDTU.Utils;
using Grid;
using UnityEngine;

/// <summary>
/// 创建配置管理器类
/// </summary>
public class ConfigManager : Singleton<ConfigManager>
{
    private TerrainSettings _terrainSettings;

    public TerrainSettings TerrainSettings
    {
        get
        {
            if (_terrainSettings == null)
            {
                //todo-改成实际路径或者你自己装上
                _terrainSettings = Resources.Load<TerrainSettings>("Path/To/TerrainSettings");
                if (_terrainSettings == null)
                {
                    Debug.LogError("Failed to load TerrainSettings from Resources.");
                }
            }
            return _terrainSettings;
        }
    }
}
