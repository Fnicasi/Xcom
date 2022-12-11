using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//To make sure this script runs before the others, inside Edit-> Project Settings -> Script Execution Order -> Set before Default Time. 
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; } //Singleton property, this way it can be accesed by other scripts easily (get)

    //This event will notify all subscribers when a new unit is selected
    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private bool isBusy;

    private BaseAction selectedAction;


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

    private void Start()
    {
        SetSelectedUnit(selectedUnit); //
    }

    private void Update()
    {

        if (isBusy) //If performing an action (Busy), 
        {
            return;
        }
        TryHandleUnitSelection(); //Left click selection of units handling
        
        HandleSelectedAction(); //Action handling 
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //We will perform a raycast from the camera to the mouse position and store it as ray
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) //We check collision with a unit
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) //By using TryGetComponent, it returns a true/false, and if true stores it in out --> unit
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false; // No collision with a unit
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetMoveAction());


        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty); //The code commented below does the same than this compact line
        /*if (OnSelectedUnitChanged != null) 
        {
            OnSelectedUnitChanged(this, EventArgs.Empty);
        }*/
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    //There's 2 ways of handling actions, each action has its own function due to the different required args or...
    //We create a general "takeAction" function 
    private void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (selectedUnit != null) //If there's any selected unit...
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(Mouse.GetPosition()); //get the position of the mouse on the grid (when clicking)

                switch (selectedAction) //In the first case of different functions, we can solve it by doing a switch case
                {

                    case MoveAction moveAction:
                        if (moveAction.IsValidActionGridPosition(mouseGridPosition)) //if it's a valid position on the grid...
                        {
                            SetBusy();
                            moveAction.Move(mouseGridPosition, ClearBusy); //move to the clicked position
                        }
                        break;
                    case SpinAction spin:
                        SetBusy();
                        //To clearBusy and allow to perform actions once the method has finished, we use Delegates, sending the method we want called after the method Spin() has finished
                        spin.Spin(ClearBusy);
                        break;
                }
            }
        }
    }

    private void SetBusy()
    {
        isBusy= true;
    }
    private void ClearBusy()
    {
        isBusy= false; 
    }
}
