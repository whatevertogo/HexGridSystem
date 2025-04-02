using UnityEngine;
using Core.Interfaces;
using Core.Grid;

namespace Core
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PolygonCollider2D))]
    public class GridCell : MonoBehaviour, IGridCell
    {

        public HexCoordinates Coordinates { get; set; }

        // 组件引用
        private ITerrainCell terrainComponent;
        private IDiceContainer diceContainer;
        private ICivilizationTerritory territoryComponent;
        public Vector3 GetPosition() => transform.position;

        private PolygonCollider2D polygonCollider;

        private void Awake()
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            InitializeHexCollider();
            InitializeComponents();
        }

        private void InitializeHexCollider()
        {
            if (polygonCollider != null)
            {
                // 创建六边形的顶点数组（6个点）
                Vector2[] points = new Vector2[6];
                for (int i = 0; i < 6; i++)
                {
                    // 直接使用 x 和 y 坐标，忽略 z
                    Vector3 corner = HexMetrics.corners[i];
                    points[i] = new Vector2(corner.x, corner.y);
                }
                
                polygonCollider.points = points;
                polygonCollider.isTrigger = false;
            }
        }

        // 移除 Initialize 方法，改为配置方法
        public void Configure(HexCoordinates coordinates)
        {
            Coordinates = coordinates;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            if (terrainComponent == null)
                terrainComponent = GetComponent<ITerrainCell>();
            if (diceContainer == null)
                diceContainer = GetComponent<IDiceContainer>();
            if (territoryComponent == null)
                territoryComponent = GetComponent<ICivilizationTerritory>();

            if (terrainComponent == null || diceContainer == null || territoryComponent == null)
            {
                Debug.LogError($"[GridCell] Missing required components on {gameObject.name}");
            }
        }
        // 地形相关方法委托
        public void AddTerrain(TerrainType terrain)
        {
            terrainComponent?.AddTerrainType(terrain);
        }

        public bool HasTerrain(TerrainType terrain)
        {
            return terrainComponent?.HasTerrainType(terrain) ?? false;
        }

        // 骰子相关方法委托
        public bool TryPlaceDice(Dice dice)
        {
            if (diceContainer?.CanPlaceDice(dice) ?? false)
            {
                diceContainer.PlaceDice(dice);
                return true;
            }
            return false;
        }

        public Dice RemoveDice()
        {
            return diceContainer?.RemoveDice();
        }

        // 文明相关方法委托
        public bool TryOccupy(ICivilization civilization)
        {
            if (territoryComponent?.CanBeOccupied(civilization) ?? false)
            {
                territoryComponent.SetOccupyingCivilization(civilization);
                return true;
            }
            return false;
        }

        public ICivilization GetOccupyingCivilization()
        {
            return territoryComponent?.GetOccupyingCivilization();
        }

        public override string ToString()
        {
            return $"GridCell {Coordinates}";
        }
    }
}