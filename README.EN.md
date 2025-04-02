# HexGridSystem

#### [中文](README.md)

A 3D hexagonal grid system for Unity, providing comprehensive functionality for creating, managing, and interacting with hexagonal maps.

## Overview

HexGridSystem is a modular hexagonal grid system designed for Unity games, offering a flexible and powerful mapping solution. Features include:

- Dynamic 3D hexagonal grid generation and management
- Efficient cube coordinate system
- Complete terrain system
- Grid cell highlighting and selection
- Flexible cell management and interaction system

## Core Features

### 1. Grid System
- **Dynamic Grid Generation**: Support for hexagonal maps of any size
- **Customizable Grid Parameters**: Adjustable hexagon size, spacing, and other parameters
- **Cube Coordinate System**: Efficient hexagonal coordinate conversion and calculation

### 2. Terrain System
- **Multiple Terrain Types**: Built-in support for water, mountains, plains, and more
- **Terrain Layering**: Support for terrain type combinations
- **Extensibility**: Easy addition of new terrain types

### 3. Interaction System
- **Selection Highlighting**: Mouse hover and selection effects
- **Event System**: Complete cell interaction events
- **Collision Detection**: Precise hexagonal collision boundaries

## Requirements

- Unity 2020.3 or higher
- .NET Framework 4.x

## Quick Start

### 1. Installation
1. Clone or download this repository
2. Copy the entire HexGridSystem folder into your Unity project's Assets directory

### 2. Basic Setup
1. Create Scene
   ```
   Create a new scene in Unity
   ```

2. Create Grid Manager
   ```
   Create an empty GameObject named "HexGrid"
   Add the HexGrid component to this object
   ```

3. Configure Parameters
   ```
   Set grid dimensions (width, height)
   Adjust cell spacing (cellSpacing)
   Assign GridCell Prefab
   ```

### 3. Code Examples

1. Creating Grid
```csharp
// Get or create grid manager
HexGrid hexGrid = FindObjectOfType<HexGrid>();

// Access cell at specific coordinates
HexCoordinates coords = HexCoordinates.FromOffsetCoordinates(x, y);
GridCell cell = hexGrid.GetCellAtCoordinates(coords);
```

2. Terrain Operations
```csharp
// Set terrain
cell.AddTerrain(TerrainType.Mountain);

// Check terrain
bool isMountain = cell.HasTerrain(TerrainType.Mountain);
```

3. Getting Adjacent Cells
```csharp
var neighbors = hexGrid.GetNeighbors(cell);
foreach (var neighbor in neighbors) {
    // Process neighboring cells
}
```

## Advanced Features

### 1. Custom Terrain Types
Extend terrain types in `TerrainType.cs`:
```csharp
public enum TerrainType
{
    None = 0,
    Water = 1 << 1,
    Mountain = 1 << 2,
    Plane = 1 << 3,
    // Add new terrain types
    Forest = 1 << 4,
    Desert = 1 << 5
}
```

### 2. Grid Parameter Adjustment
Modify grid parameters in `HexMetrics.cs`:
```csharp
public static class HexMetrics
{
    public const float outerRadius = 1f;
    public const float innerRadius = outerRadius * 0.866025404f;
}
```

## Optimization Tips

1. **Performance Optimization**
   - Consider object pooling for large maps
   - Implement dynamic loading within view range
   - Use mesh combining to reduce draw calls

2. **Memory Management**
   - Release unused grid resources promptly
   - Use shared materials to reduce memory usage

## Important Notes

- This is a 3D hexagonal grid system; code modifications needed for 2D
- Consider performance optimization for large-scale maps
- Ensure all necessary components and references are properly set up

## Technical Support

For issues, please check the example scenes or submit an Issue.

## License

MIT License - See [LICENSE](LICENSE) file for details

## Integration Guide

### Interacting with HexGrid System

There are several ways to integrate and interact with the HexGrid system in your own project:

### 1. Context Class Pattern

Create a context class to manage the interaction with HexGrid:

```csharp
public class HexGridContext
{
    private HexGrid hexGrid;
    private Dictionary<HexCoordinates, GameEntity> entityMap;

    public HexGridContext(HexGrid grid)
    {
        hexGrid = grid;
        entityMap = new Dictionary<HexCoordinates, GameEntity>();
    }

    // Place game entities on the grid
    public void PlaceEntity(GameEntity entity, HexCoordinates coordinates)
    {
        if (IsValidPosition(coordinates))
        {
            entityMap[coordinates] = entity;
            GridCell cell = hexGrid.GetCellAtCoordinates(coordinates);
            // Update cell state or visual feedback
        }
    }

    // Check if a move is valid
    public bool CanMoveTo(HexCoordinates from, HexCoordinates to)
    {
        GridCell fromCell = hexGrid.GetCellAtCoordinates(from);
        GridCell toCell = hexGrid.GetCellAtCoordinates(to);
        
        return IsValidPosition(to) && !entityMap.ContainsKey(to) 
               && !toCell.HasTerrain(TerrainType.Mountain);
    }

    // Get entities in range
    public List<GameEntity> GetEntitiesInRange(HexCoordinates center, int range)
    {
        var cells = hexGrid.GetCellsInRange(center, range);
        return cells.Where(c => entityMap.ContainsKey(c.Coordinates))
                   .Select(c => entityMap[c.Coordinates])
                   .ToList();
    }
}
```

### 2. Component-Based Integration

Create a MonoBehaviour component to handle grid interactions:

```csharp
public class HexGridInteractionManager : MonoBehaviour
{
    public HexGrid hexGrid;
    private GridCell selectedCell;
    private List<GridCell> highlightedCells;

    void Start()
    {
        highlightedCells = new List<GridCell>();
        // Subscribe to grid events
        hexGrid.OnCellSelected += HandleCellSelection;
    }

    // Handle cell selection
    private void HandleCellSelection(GridCell cell)
    {
        selectedCell = cell;
        // Clear previous highlights
        ClearHighlights();
        
        // Highlight movement range
        var cellsInRange = hexGrid.GetCellsInRange(cell.Coordinates, 3);
        foreach (var rangeCell in cellsInRange)
        {
            if (IsCellValidForMovement(rangeCell))
            {
                HighlightCell(rangeCell);
                highlightedCells.Add(rangeCell);
            }
        }
    }

    // Example of handling path finding
    public List<GridCell> FindPath(GridCell start, GridCell end)
    {
        // Implement A* or other pathfinding algorithm
        return hexGrid.FindPath(start.Coordinates, end.Coordinates);
    }
}
```

### 3. Event-Based System

Subscribe to grid events for reactive gameplay:

```csharp
public class GameplayManager : MonoBehaviour
{
    private HexGrid hexGrid;
    
    void Start()
    {
        hexGrid = FindObjectOfType<HexGrid>();
        
        // Subscribe to grid events
        hexGrid.OnCellSelected += OnCellSelected;
        hexGrid.OnCellHighlighted += OnCellHighlighted;
    }
    
    private void OnCellSelected(GridCell cell)
    {
        // Handle cell selection
        if (IsUnitSelected && cell.IsValidDestination)
        {
            MoveUnitTo(cell.Coordinates);
        }
    }
    
    private void OnCellHighlighted(GridCell cell)
    {
        // Update UI or show cell information
        UpdateCellInfoPanel(cell);
    }
}
```

### Best Practices

1. **Separation of Concerns**
   - Keep grid logic separate from game logic
   - Use interfaces to define interaction contracts
   - Implement observers for grid state changes

2. **State Management**
   - Cache frequently accessed data
   - Use events to notify state changes
   - Implement undo/redo system if needed

3. **Performance Considerations**
   - Batch grid updates
   - Use object pooling for visual effects
   - Cache pathfinding results

4. **Example Integration Pattern**

```csharp
public interface IGridInteraction
{
    void OnCellSelected(GridCell cell);
    void OnCellHighlighted(GridCell cell);
    bool IsValidMove(HexCoordinates from, HexCoordinates to);
}

public class TurnBasedGameManager : MonoBehaviour, IGridInteraction
{
    private HexGridContext gridContext;
    private GameState currentState;

    void Start()
    {
        var hexGrid = FindObjectOfType<HexGrid>();
        gridContext = new HexGridContext(hexGrid);
        
        // Initialize game state
        currentState = new GameState(gridContext);
    }

    public void OnCellSelected(GridCell cell)
    {
        // Handle based on current game state
        currentState.HandleCellSelection(cell);
    }

    public bool IsValidMove(HexCoordinates from, HexCoordinates to)
    {
        return gridContext.CanMoveTo(from, to);
    }
}
```

These integration patterns provide flexible ways to interact with the HexGrid system while maintaining clean code architecture and separation of concerns.
