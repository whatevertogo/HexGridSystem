using System.Collections.Generic;
using UnityEngine;
using Core.Interfaces;

namespace Core.Grid
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    /// <summary>
    /// 创造六边形网格的类
    /// </summary>
    public class HexMesh : MonoBehaviour
    {
        private Mesh hexMesh;
        private List<Vector3> vertices;
        private List<int> triangles;

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
            hexMesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
            triangles = new List<int>();
        }

        public void Triangulate(IEnumerable<GridCell> cells)
        {
            hexMesh.Clear();
            vertices.Clear();
            triangles.Clear();

            foreach (var cell in cells)
            {
                Triangulate(cell);
            }

            hexMesh.vertices = vertices.ToArray();
            hexMesh.triangles = triangles.ToArray();
            hexMesh.RecalculateNormals();
        }

        private void Triangulate(GridCell cell)
        {
            Vector3 center = cell.transform.localPosition;
            
            // 创建六边形的六个三角形
            for (int i = 0; i < 6; i++)
            {
                AddTriangle(
                    center,
                    center + HexMetrics.corners[i],
                    center + HexMetrics.corners[i + 1]
                );
            }
        }

        private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }
    }
}