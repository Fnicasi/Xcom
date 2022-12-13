using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//abstract so it's never created
public abstract class BaseAction : MonoBehaviour
{
    //protected so private except for the classes that extend from this one (MoveAction, SpinAction,...)
    protected Unit unit;
    protected bool isActive; //Check if the unit is active(performing an action), if true, allow actions on update
    protected Animator unitAnimator;

    protected Action onActionComplete;


    //Virtual so it can be overrided
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        unitAnimator = GetComponentInChildren<Animator>();

    }

    public abstract string GetActionName(); //We force the classes that extend to implement this function

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    //Check if the grid position is valid
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList(); //Take the list of valid movements
        return validGridPositionList.Contains(gridPosition); //Check if the gridPosition passed to move, is in the list of valid movements
    }
    public abstract List<GridPosition> GetValidActionGridPositionList();
    
    public virtual int GetActionPointCost() //All actions will take 1 action point, but it can be overrided if necessary
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete){
        isActive = true; //The unit is performing action, thus allow action to be executed in Update
        this.onActionComplete = onActionComplete;
    }
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete(); //Call the delegate
    }
}
