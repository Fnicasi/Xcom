using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//To make sure this script runs before the others, inside Edit-> Project Settings -> Script Execution Order -> Set before Default Time. 
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; } //Singleton property, this way it can be accesed by other scripts easily (get)

    [SerializeField] private Unit selectedUnit;

    [SerializeField] private LayerMask unitLayerMask;
    //This event will notify all subscribers when a new unit is selected
    public event EventHandler OnSelectedUnitChanged;

    private void Awake()
    {
        if(Instance!=null) //Just in case another UnitActionSystem was created incorrectly 
        {
            Debug.Log("There's more than one UnitActionSystem" + transform + " - " + Instance);
            Destroy(Instance);
            return;  
        }
        Instance= this;
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) //If left click, check for unit selection
        {
            TryHandleUnitSelection();
        }
        if(Input.GetMouseButtonDown(1)) //Move currently selected unit to the selected position
        {
            if (selectedUnit != null) //If there's any selected unit...
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(Mouse.GetPosition()); //get the position of the mouse on the grid (when clicking)
                if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)) //if it's a valid position on the grid...
                {
                    selectedUnit.GetMoveAction().Move(mouseGridPosition); //move to the clicked position
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedUnit != null)
            {
                selectedUnit.GetSpinAction().Spin();
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //We will perform a raycast from the camera to the mouse position and store it as ray
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) //We check collision with a unit
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) //By using TryGetComponent, it returns a true/false, and if true stores it in out --> unit
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        selectedUnit= null;
        return false; // No collision with a unit
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty); //THe code commented below does the same than this compact line
        /**if (OnSelectedUnitChanged != null) 
        {
            OnSelectedUnitChanged(this, EventArgs.Empty);
        }**/
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
