using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour //This is the main script that will manage the level grid
{
    public static LevelGrid Instance { get; private set; }
    [SerializeField] private Transform gridDebugObjectPrefab;


    public event EventHandler OnAnyMovedGridPosition;

    private GridSystem<GridObject> gridSystem; 
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null) //Just in case another LevelGrid was created incorrectly 
        {
            Debug.Log("There's more than one LevelGrid" + transform + " - " + Instance);
            Destroy(Instance);
            return;
        }
        Instance = this;
        //Due to the GridSystem generic configuration, it needs a delegate, we will use a lambda expression (define function here) as parameter
        //so it receives two parameters as defined in the Func of GridSystem a GridSystem<GridObject> and GridPosition and returns a new GridObject(gridPosition, g)
        gridSystem = new GridSystem<GridObject>(10, 10, 2f,     
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(gridPosition, g));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    //Gets the GridObject from the GridSystem and sets the unit
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList(); 
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) 
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition); //Take the GridObject in gridPosition and...
        gridObject.RemoveUnit(unit); //delete the unit from the list
    }

    //Get the grid position (through the grid system) of the world position
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition); 
    }
    //This => (lambda) notation is equal to the one in GetGridPosition aka return the value on the right
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    //Get the world position (through the grid system) of the grid position
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();


    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPos, GridPosition toGridPos)
    {
        RemoveUnitAtGridPosition(fromGridPos, unit); //Clear the grid position from the unit that left
        AddUnitAtGridPosition(toGridPos, unit); //Assign to toGridPos the unit

        OnAnyMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public bool HasAnyUnitOnGridPosition (GridPosition gridPosition)
    { //If there is any unit, return true
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }public Unit GetUnitAtGridPosition (GridPosition gridPosition)
    { //Get the unit at the grid position 
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}
