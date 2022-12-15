using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

//To make sure this script runs before the others, inside Edit-> Project Settings -> Script Execution Order -> Set before Default Time. 
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; } //Singleton property, this way it can be accesed by other scripts easily (get)

    //This event will notify all subscribers when a new unit is selected
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;


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

        if (isBusy) //If performing an action (Busy), don't allow actions
        {
            return;
        }
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            return; //If it's not the player's turn, don't allow actions
        }
        if(EventSystem.current.IsPointerOverGameObject()) //If the cursor is over any UI element...
        {
            return; //Return, ergo don't click
        }
        TryHandleUnitSelection(); //Left click selection of units handling
        
        HandleSelectedAction(); //Action handling 
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //We will perform a raycast from the camera to the mouse position and store it as ray
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) //We check collision with a unit
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) //By using TryGetComponent, it returns a true/false, and if true stores it in out --> unit
                {
                    if(unit == selectedUnit) {
                        return false; //if the unit is already selected, don't select it again, (to be able to spin easily)
                    }
                    if(unit.IsEnemy()) //if the unit selected is an enemy, don't allow it
                    {
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
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
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    //There's 2 ways of handling actions, each action has its own function due to the different required args or...
    //We create a general "takeAction" function 
    private void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (selectedUnit == null) //If there's not any selected unit, exit
            {
                return;
            }
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(Mouse.GetPosition()); //get the position of the mouse on the grid (when clicking)
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) //Check if it's a valid grid position, if not, exit
            {
                return;
            }
            if (!selectedUnit.TrySpendActionPoints(selectedAction)) //If it does not have enough action points, exit, if it does, it will detract the cost
            {
                return;
            }
            //Otherwise, the unit takes the action, so it's busy
            SetBusy(); //Set the unit as busy
            selectedAction.TakeAction(mouseGridPosition, ClearBusy); //Perform the action and then the ClearBusy when finished    

            OnActionStarted?.Invoke(this, EventArgs.Empty);//Whenever an action is taken, call the event of action started
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);

    }
    private void ClearBusy()
    {
        isBusy= false;
        OnBusyChanged?.Invoke(this, isBusy);
    }
}
