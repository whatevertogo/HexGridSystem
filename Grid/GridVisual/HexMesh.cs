using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grid
{
    /// <summary>
    /// 生成和管理六边形网格的类
    /// 更具六边形单元格数据动态生成网格的几何形状和颜色并渲染到场景中
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexMesh : MonoBehaviour
    {
        [Header("外观设置")]
        [SerializeField]
        private Color lineColor = Color.black;

        [SerializeField]
        [Range(0, 0.1f)]
        private float lineWidth = 0.025f;

        [SerializeField]
        private bool useTexture = false;

        private Mesh hexMesh;
        private List<Vector3> vertices;
        private List<int> triangles;
        private List<Color> colors;
        private List<Vector2> uvs;
        private Material material;
        private Dictionary<HexCell, (int start, int count)> cellVertexRanges = new();

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
            hexMesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color>();
            uvs = new List<Vector2>();

            // 设置材质
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer.sharedMaterial is null)
            {
                material = new Material(Shader.Find("Custom/HexGridShader"));
                UpdateMaterialProperties();
                meshRenderer.sharedMaterial = material;
            }
        }

        private void UpdateMaterialProperties()
        {
            if (material is not null)
            {
                material.SetColor("_LineColor", lineColor);
                material.SetFloat("_LineWidth", lineWidth);
                material.SetFloat("_UseTexture", useTexture ? 1 : 0);
            }
        }

        /// <summary>
        /// 根据传入的六边形单元格集合生成网格
        /// </summary>
        /// <param name="cells"></param>
        public void Triangulate(IEnumerable<HexCell> cells)
        {
            bool fullUpdate = !cells.Any(c => cellVertexRanges.ContainsKey(c));

            if (fullUpdate)
            {
                hexMesh.Clear();
                vertices.Clear();
                triangles.Clear();
                colors.Clear();
                uvs.Clear();
                cellVertexRanges.Clear();
            }
            else
            {
                // 只移除需要更新的单元格的旧顶点
                foreach (var cell in cells.Where(c => cellVertexRanges.ContainsKey(c)))
                {
                    RemoveCellVertices(cell);
                }
            }

            foreach (var cell in cells)
            {
                int startIndex = vertices.Count;
                Triangulate(cell);
                cellVertexRanges[cell] = (startIndex, vertices.Count - startIndex);
            }

            hexMesh.vertices = vertices.ToArray();
            hexMesh.triangles = triangles.ToArray();
            hexMesh.colors = colors.ToArray();
            hexMesh.uv = uvs.ToArray();
            hexMesh.RecalculateNormals();
        }

        private void RemoveCellVertices(HexCell cell)
        {
            if (cellVertexRanges.TryGetValue(cell, out var range))
            {
                vertices.RemoveRange(range.start, range.count);

                // 更新后续单元格的顶点范围
                foreach (var kvp in System.Linq.Enumerable.ToList(cellVertexRanges))
                {
                    if (kvp.Key != cell && kvp.Value.start > range.start)
                    {
                        cellVertexRanges[kvp.Key] = (
                            kvp.Value.start - range.count,
                            kvp.Value.count
                        );
                    }
                }

                cellVertexRanges.Remove(cell);
            }
        }

        /// <summary>
        /// 为单个六边形单元格生成集合数据
        /// </summary>
        /// <param name="cell"></param>
        private void Triangulate(HexCell cell)
        {
            Vector3 center = cell.transform.localPosition;

            // 创建六边形的六个三角形，每个三角形共用中心点
            for (int i = 0; i < 6; i++)
            {
                // 获取当前方向的相邻单元格
                HexCell neighbor = GetNeighbors(cell, (HexDirection)i);

                // 当前三角形的三个点：中心点和两个顶点
                Vector3 v1 = center;
                Vector3 v2 = center + HexMetrics.corners[i];
                Vector3 v3 = center + HexMetrics.corners[i + 1];

                // 添加三角形顶点
                AddTriangle(v1, v2, v3);

                // 计算UV坐标以便shader能够正确渲染边线
                Vector2 uv1 = new Vector2(0.5f, 0.5f);
                Vector2 uv2 = new Vector2(
                    0.5f + HexMetrics.corners[i].x / (2f * HexMetrics.outerRadius),
                    0.5f + HexMetrics.corners[i].y / (2f * HexMetrics.outerRadius)
                );
                Vector2 uv3 = new Vector2(
                    0.5f + HexMetrics.corners[i + 1].x / (2f * HexMetrics.outerRadius),
                    0.5f + HexMetrics.corners[i + 1].y / (2f * HexMetrics.outerRadius)
                );
                AddUV(uv1, uv2, uv3);

                // 直接使用单元格的颜色，不进行混合
                Color cellColor = cell.color;
                AddTriangleColor(cellColor, cellColor, cellColor);
            }
        }

        /// <summary>
        /// 获取指定方向的相邻单元格
        /// </summary>
        /// <param name="cell">当前单元格</param>
        /// <param name="direction">方向</param>
        /// <returns>相邻的单元格，如果不存在则返回null</returns>
        private HexCell GetNeighbors(HexCell cell, HexDirection direction)
        {
            // 使用GridManager的GetCellAtCoordinates方法获取相邻单元格，避免重复逻辑
            HexCoordinates neighborCoords = cell.coordinates.GetNeighbor(direction);
            return GridManager.Instance.GetCellAtCoordinates(neighborCoords);
        }

        /// <summary>
        /// 为一个三角形的三个顶点设置颜色
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        private void AddTriangleColor(Color c1, Color c2, Color c3)
        {
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c3);
        }

        /// <summary>
        /// 添加另一个三角形顶点和索引
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
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

        /// <summary>
        /// 添加UV坐标
        /// </summary>
        /// <param name="uv1"></param>
        /// <param name="uv2"></param>
        /// <param name="uv3"></param>
        private void AddUV(Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // 确保material已初始化
            if (material == null)
            {
                var meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer != null && meshRenderer.sharedMaterial != null)
                {
                    material = meshRenderer.sharedMaterial;
                }
                else if (Application.isPlaying)
                {
                    material = new Material(Shader.Find("Custom/HexGridShader"));
                    if (meshRenderer != null)
                    {
                        meshRenderer.sharedMaterial = material;
                    }
                }
                else
                {
                    // 在编辑器模式下，如果没有材质，就不更新属性
                    return;
                }
            }

            UpdateMaterialProperties();
        }
#endif
    }
}
