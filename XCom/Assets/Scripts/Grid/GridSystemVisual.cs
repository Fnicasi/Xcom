using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [Serializable] //So it shows in the inspector
    public struct GridVisualTypeMaterial //This struct, together with a list, will allow us to store dinamically the colors 
    {
        public GridVisualType gridVisualType;
        public Material material; 
    }
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        RedSoft
    }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    //Like we did with GridObject, we will create an array of GridVisualSingles (the transparent quads of movement)
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    private void Awake()
    {
        if (Instance != null) //Just in case another GridSystemVisual was created incorrectly 
        {
            Debug.Log("There's more than one UnitActionSystem" + transform + " - " + Instance);
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()]; //Create array with the dimensions defined in the level
        for(int x=0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z=0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition= new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                //This array contains all the components of GridVisualSingle
                gridSystemVisualSingleArray[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged; //Listen to event that controls when an action is changed
        LevelGrid.Instance.OnAnyMovedGridPosition += LevelGrid_OnAnyMovedGridPosition; //Listen to the event that controls when a unit changed its grid position

        UpdateGridVisual();
    }


    //Will hide ALL the meshes
    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].HideMesh();
            }
        }
    }
    //This function shows the range of the shooter 
    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue; //if the GridPosition is not inside the grid bounds, skip to the next iteration
                }
                //With this, the distance will be like a diamond instead of a square
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    //Will enable the mesh of the list passed
    public void ShowGridPositionList(List<GridPosition> gridPositionsList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionsList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].ShowMesh(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    void UpdateGridVisual()
    {
        HideAllGridPositions();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxRange(), GridVisualType.RedSoft);
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
               
                break;
        }
        if (selectedUnit != null)
        {
            ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType); //Show the grid position in the valid position(s) and in the selected color
        }

    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs args)
    {
        UpdateGridVisual(); //Whenever the selection action chanfges, update the visuals of the grid
    }

    private void LevelGrid_OnAnyMovedGridPosition(object sender, EventArgs args)
    {
        UpdateGridVisual(); //When a unit moves, upgrade the visuals of the grid
    }

    //Get the material type (color) checking in the list if the material is the same
    private Material GetGridVisualTypeMaterial (GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType: " + gridVisualType);//We should never reach this
        return null;

    }
}
