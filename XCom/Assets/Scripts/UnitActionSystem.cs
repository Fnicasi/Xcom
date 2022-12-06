using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To make sure this script runs before the others, inside Edit-> Project Settings -> Script Execution Order -> Set before Default Time. 
public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;

    [SerializeField] private LayerMask unitLayerMask;

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) //If left click, check for unit selection
        {
            TryHandleUnitSelection();
        }
        if(Input.GetMouseButtonDown(1)) //Move currently selected unit to the selected position
        {
            if (selectedUnit != null)
            {
                selectedUnit.Move(Mouse.GetPosition());
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
                selectedUnit = unit;
                return true;
            }
        }
        selectedUnit= null;
        return false; // No collision with a unit
    }
}
