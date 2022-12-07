using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour //This is the main script that will manage the level grid
{
    public static LevelGrid Instance { get; private set; }
    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem gridSystem; 
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
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    //Gets the GridObject from the GridSystem and sets the unit
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList(); 
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) 
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition); //Take the GridObject in gridPosition and...
        gridObject.RemoveUnit(unit); //delete the unit from the list
    }
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition); 
    }
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPos, GridPosition toGridPos)
    {
        RemoveUnitAtGridPosition(fromGridPos, unit); //Clear the grid position from the unit that left
        AddUnitAtGridPosition(toGridPos, unit); //Assign to toGridPos the unit
    }
}
