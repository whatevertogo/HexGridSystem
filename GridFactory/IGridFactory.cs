using UnityEngine;
using Core.Interfaces;

public interface IGridFactory
{
    public HexCell CreateGridCell(Vector3 position, ITerrainCell terrain, IDiceContainer dice, ICivilizationTerritory territory);

}