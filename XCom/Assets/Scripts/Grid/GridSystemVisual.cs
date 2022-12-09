 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

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
    }

    private void Update()
    {
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

    //Will enable the mesh of the list passed
    public void ShowGridPositionList(List<GridPosition> gridPositionsList)
    {
        foreach (GridPosition gridPosition in gridPositionsList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].ShowMesh();
        }
    }

    void UpdateGridVisual()
    {
        HideAllGridPositions();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit != null)
        {
            ShowGridPositionList(selectedUnit.GetMoveAction().GetValidActionGridPositionList());
        }

    }
}
