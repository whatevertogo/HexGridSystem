using Core;
using UnityEngine;
using Core.Interfaces;

public interface IGridFactory
{
    public GridCell CreateGridCell(Vector3 position, ITerrainCell terrain, IDiceContainer dice, ICivilizationTerritory territory);

}