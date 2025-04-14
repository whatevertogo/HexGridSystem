using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    /// <summary>
    /// 生成和管理六边形网格的类、
    /// 更具六边形单元格数据动态生成网格的几何形状和颜色并渲染到场景中
    /// </summary>
    public class HexMesh : MonoBehaviour
    {
        [Header("外观设置")]
        [SerializeField] private Color lineColor = Color.black;
        [SerializeField] [Range(0, 0.1f)] private float lineWidth = 0.025f;
        [SerializeField] private bool useTexture = false;
        [SerializeField] private Texture2D gridTexture;

        private Mesh hexMesh;
        private List<Vector3> vertices;
        private List<int> triangles;
        private List<Color> colors;
        private List<Vector2> uvs;
        private Material material;

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
            if (meshRenderer.sharedMaterial == null)
            {
                material = new Material(Shader.Find("Custom/HexGridShader"));
                UpdateMaterialProperties();
                meshRenderer.sharedMaterial = material;
            }
        }

        private void UpdateMaterialProperties()
        {
            if (material != null)
            {
                material.SetColor("_LineColor", lineColor);
                material.SetFloat("_LineWidth", lineWidth);
                material.SetFloat("_UseTexture", useTexture ? 1 : 0);
                if (useTexture && gridTexture != null)
                {
                    material.SetTexture("_MainTex", gridTexture);
                }
            }
        }

        /// <summary>
        /// 根据传入的六边形单元格集合生成网格
        /// </summary>
        /// <param name="cells"></param>
        public void Triangulate(IEnumerable<HexCell> cells)
        {
            hexMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();
            uvs.Clear();

            foreach (var cell in cells)
            {
                Triangulate(cell);
            }

            hexMesh.vertices = vertices.ToArray();
            hexMesh.triangles = triangles.ToArray();
            hexMesh.colors = colors.ToArray();
            hexMesh.uv = uvs.ToArray();
            hexMesh.RecalculateNormals();
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

                // 计算颜色
                Color centerColor = cell.color;
                Color edgeColor = neighbor != null ? (neighbor.color + centerColor) * 0.5f : centerColor;

                // 为每个顶点添加对应的颜色
                AddTriangleColor(centerColor, edgeColor, edgeColor);
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
            HexCoordinates neighborCoords = cell.Coordinates.GetNeighbor(direction);
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
            UpdateMaterialProperties();
        }
#endif
    }
}