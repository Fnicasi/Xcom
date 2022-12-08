using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem //Not extending monobehaviour, because we want to use constructor
{
    private int width;
    private int height;
    private float cellsize;

    private GridObject[,] gridObjectsArray;
    public GridSystem(int width, int height, float cellsize) //Constructor
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;

        //We initialize the array of grid objects to the width and height of the grid defined by the grid system
        gridObjectsArray = new GridObject[width, height];
        //Here we create the GridObjects
        for (int x= 0; x < width; x++)
        { 
            for (int z=0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectsArray[x,z] = new GridObject(gridPosition, this);
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

    public GridObject GetGridObject(GridPosition gridPosition) //Return the GridObject at the indicated grid position
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
}

