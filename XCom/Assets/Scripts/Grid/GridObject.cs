using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject //Each cell will contain a GridObject, which will contain all the relevant info like occupied by whom, interactables, etc
{
    private GridPosition gridPosition;
    private GridSystem<GridObject> gridSystem;
    private List<Unit> unitList;

    public GridObject(GridPosition gridPosition, GridSystem<GridObject> gridSystem)
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList) { //If we have multiple units (more than one occupying same space, like walking through) then it will display both names correctly
            unitString += unit + "\n"; 
        }
        return gridPosition.ToString() + "\n" + unitString;

    }

    //With this, we can "define" that the GridObject is a Unit, this way we know what's occupying this space in the grid
    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0; //Return true if there's any unit in the list of units occupying this grid position
    }

    public Unit GetUnit()
    {
        if(HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }
}
