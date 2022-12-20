using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Since we want to use the GridSystem for the pathfinding, and to separate the game logic from the pathfinding logic, we will make it Generic
//C# does not allow to use new() with multiple parameters when defining it as constraint (where TGridObject : new() ), so we receive a delegate that creates our TGridObject
public class GridSystem<TGridObject>//Not extending monobehaviour, because we want to use constructor
{
    private int width;
    private int height;
    private float cellsize;

    private TGridObject[,] gridObjectsArray;
    //We used the constructor "gridObjectsArray[x,z] = new TGridObject(this, gridPosition);" now we will use the delegate Func, which returns something,
    //Matching the old constructor: this = GridSystem<TGridObject>, a GridPosition and returns a TGridObject, this func will be called createGridObject
    public GridSystem(int width, int height, float cellsize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;

        //We initialize the array of grid objects to the width and height of the grid defined by the grid system
        gridObjectsArray = new TGridObject[width, height];
        //Here we create the GridObjects
        for (int x= 0; x < width; x++)
        { 
            for (int z=0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectsArray[x,z] = createGridObject(this, gridPosition); //Delegate, will return a TGridObject
            }
        }
    }

    //Get world position of cell
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x,0,gridPosition.z) * cellsize;

    }

    //Get grid position of a world location
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellsize), 
            Mathf.RoundToInt(worldPosition.z / cellsize)
            );
    }


    public void CreateDebugObjects(Transform debugPrefab) //Test 
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z); //Set griPosition of the current Debug Object
                //Instantiate and set it as debugTransform, getting its gridPosition with GetWorldPosition
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity); 
                //Of said debugPrefab, get the GridDebugObject
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                //Set it as a GridObject
                gridDebugObject.SetGridObject(GetGridObject(gridPosition)); 
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition) //Return the GridObject at the indicated grid position
    {
        return gridObjectsArray[gridPosition.x, gridPosition.z];
    }
    //Will check if it is out of bounds
    public Boolean IsValidGridPosition(GridPosition gridPosition)
    { 
         return gridPosition.x >= 0 && 
                gridPosition.z >= 0 && 
                gridPosition.x< width && 
                gridPosition.z< height;
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}

