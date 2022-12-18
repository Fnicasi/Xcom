using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{
    //Delegates, just as any other function, can return different types and receive arguments
    //public delegate void SpinCompleteDelegate();
    //private SpinCompleteDelegate onSpinComplete;
    //private Action onSpinComplete; //This line equals the delegeta above, this is an existing delegate in Unity.System, we will use OnActionComplete from BaseAction


    private float totalSpinAmount;
    private float addSpinAmount;


    private void Update()
    {
        if (!isActive)//If not performing an action (being active), return
        {
            return;
        }

        addSpinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, addSpinAmount, 0); //Rotate on the Y

        totalSpinAmount += addSpinAmount;
        if (totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }
    //Implement the generic TakeAction function
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        totalSpinAmount = 0;
        ActionStart(onActionComplete); //We call this function, that calls the event, and do it at the end to make sure everything is done before calling the event

    }
    public override string GetActionName()
    {
        return "Spin";
    }

    //For spin, just return the position of the unit on the grid
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition(); //The position of the unit on the grid

        return new List<GridPosition> { unitGridPosition };
    }

    public override int GetActionPointCost()
    {
        return 1;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

}
