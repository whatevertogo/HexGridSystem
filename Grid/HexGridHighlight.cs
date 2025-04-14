// using UnityEngine;
// using System;

// namespace Grid
// {
//     public class HexGridHighlight : MonoBehaviour
//     {
//         [Header("引用设置")]
//         public Camera mainCamera;
//         public HexGrid hexGrid;

//         [Header("高亮设置")]
//         public Color hoverColor = new Color(1f, 1f, 0f, 0.3f);
//         public Color selectedColor = new Color(0f, 1f, 0f, 0.3f);

//         private HexCell currentHoverCell;
//         private HexCell selectedCell;

//         public event EventHandler<GridCellEventArgs> OnCellHovered;
//         public event EventHandler<GridCellEventArgs> OnCellSelected;
//         public event EventHandler<GridCellEventArgs> OnCellDeselected;

//         public class GridCellEventArgs : EventArgs
//         {
//             public HexCell Cell { get; }
//             public GridCellEventArgs(HexCell cell) => Cell = cell;
//         }

//         private void Start()
//         {
//             mainCamera ??= Camera.main;
//             hexGrid ??= FindFirstObjectByType<HexGrid>();
//         }

//         private void Update()
//         {
//             HandleHighlight();
//             HandleSelection();
//         }

//         private void HandleHighlight()
//         {
//             Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//             if (Physics.Raycast(ray, out RaycastHit hit))
//             {
//                 if (hit.collider.TryGetComponent<HexCell>(out var hoveredCell) && hoveredCell != currentHoverCell)
//                 {
//                     ResetHoverVisual();
//                     if (hoveredCell != selectedCell)
//                     {
//                         SetCellHighlight(hoveredCell, hoverColor);
//                     }
//                     currentHoverCell = hoveredCell;
//                     OnCellHovered?.Invoke(this, new GridCellEventArgs(hoveredCell));
//                 }
//             }
//             else
//             {
//                 ResetHoverVisual();
//             }
//         }

//         private void HandleSelection()
//         {
//             if (Input.GetMouseButtonDown(0) && currentHoverCell != null)
//             {
//                 if (currentHoverCell == selectedCell)
//                 {
//                     ResetSelectVisual();
//                     OnCellDeselected?.Invoke(this, new GridCellEventArgs(selectedCell));
//                     selectedCell = null;
//                 }
//                 else
//                 {
//                     ResetSelectVisual();
//                     selectedCell = currentHoverCell;
//                     SetCellHighlight(selectedCell, selectedColor);
//                     OnCellSelected?.Invoke(this, new GridCellEventArgs(selectedCell));
//                 }
//             }
//         }

//         private void SetCellHighlight(HexCell cell, Color color)
//         {
//             var renderer = cell.GetComponent<Renderer>();
//             if (renderer != null)
//             {
//                 // var highlightMaterial = materialPool.GetHighlightMaterial(color);
//                 // renderer.material = highlightMaterial;
//             }
//         }

//         private void ResetHoverVisual()
//         {
//             if (currentHoverCell != null && currentHoverCell != selectedCell)
//             {
//                 ResetCellMaterial(currentHoverCell);
//             }
//             currentHoverCell = null;
//         }

//         private void ResetSelectVisual()
//         {
//             if (selectedCell != null)
//             {
//                 ResetCellMaterial(selectedCell);
//             }
//         }

//         private void ResetCellMaterial(HexCell cell)
//         {
//             var renderer = cell.GetComponent<Renderer>();
//             if (renderer != null)
//             {
//                 var defaultMaterial = hexGrid.GetComponent<MeshRenderer>()?.sharedMaterial;
//                 if (defaultMaterial != null)
//                 {
//                     renderer.material = defaultMaterial;
//                 }
//             }
//         }

//         private void OnDisable()
//         {
//             ResetHoverVisual();
//             ResetSelectVisual();
//         }
//     }
// }
