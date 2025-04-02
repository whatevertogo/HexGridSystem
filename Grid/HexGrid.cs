using UnityEngine;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Grid
{
    public class HexGrid : MonoBehaviour
    {
        [Header("网格设置")]
        [SerializeField] private GridCellFactory gridCellFactory;
        [Header("网格设置")]
        [SerializeField] private int width = 6;
        [SerializeField] private int height = 6;
        [SerializeField] private float cellSpacing = 1.0f;

        private Dictionary<HexCoordinates, GridCell> cells = new();
        private HexMesh hexMesh;

        private void Awake()
        {
            hexMesh = GetComponentInChildren<HexMesh>();
            CreateGrid();
        }

        private void CreateGrid()
        {
            cells.Clear();
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    CreateCell(x, z);
                }
            }

            if (hexMesh != null)
            {
                hexMesh.Triangulate(cells.Values);
            }
        }

        private void CreateCell(int x, int z)
        {
            Vector3 position = CalculateCellPosition(x, z);
            HexCoordinates coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

            GridCell cell = gridCellFactory.CreateGridCell(position, coordinates);
            if (cell != null)
            {
                cell.transform.SetParent(transform, false);
                cells[coordinates] = cell;
            }
        }

        private Vector3 CalculateCellPosition(int x, int z)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f) * cellSpacing;
            position.y = z * (HexMetrics.outerRadius * 1.5f) * cellSpacing;
            position.z = 0f;
            return position;
        }

        public GridCell GetCellAtCoordinates(HexCoordinates coordinates)
        {
            cells.TryGetValue(coordinates, out var cell);
            return cell;
        }

        public IEnumerable<GridCell> GetAllCells()
        {
            return cells.Values;
        }

        public List<GridCell> GetNeighbors(GridCell cell)
        {
            var neighbors = new List<GridCell>();
            var coordinates = cell.Coordinates;

            for (int direction = 0; direction < 6; direction++)
            {
                var neighborCoords = coordinates.GetNeighbor((HexDirection)direction);
                if (cells.TryGetValue(neighborCoords, out var neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
            return neighbors;
        }

        public bool HasAnyCivilization()
        {
            foreach (var cell in cells.Values)
            {
                var territory = cell.GetComponent<ICivilizationTerritory>();
                if (territory?.GetOccupyingCivilization() != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}