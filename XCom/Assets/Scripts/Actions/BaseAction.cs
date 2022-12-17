using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//abstract so it's never created
public abstract class BaseAction : MonoBehaviour
{
    //Events when action is started / completed
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    //protected so private except for the classes that extend from this one (MoveAction, SpinAction,...)
    protected Unit unit;
    protected bool isActive; //Check if the unit is active(performing an action), if true, allow actions on update

    protected Action onActionComplete;


    //Virtual so it can be overrided
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();

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

    //We want the events to be called after something has already happened, so we put the Invoke at the end of the function
    protected void ActionStart(Action onActionComplete){
        isActive = true; //The unit is performing action, thus allow action to be executed in Update
        this.onActionComplete = onActionComplete; //Assign to the variable onActionComplete the delegate we passed as value

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);  

    }
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete(); //Call the delegate

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }
    public Unit GetUnit()
    {
        return unit; 
    }
}
