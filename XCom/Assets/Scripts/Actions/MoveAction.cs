using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{

    private Vector3 targetPosition; //Where the unit has to go
    private float stoppingDistance; //The distance at which the movement stops and we consider the unit has arrived to their destination
    private float moveSpeed; //At what speed it moves
    private float rotateSpeed;

    [SerializeField] private int maxMoveDistance;
   
    protected override void Awake()
    {
        base.Awake(); //So the Awake from BaseAction is ran
         
        targetPosition = transform.position;

    }

    private void Start()
    {
        stoppingDistance = 0.01f;
        moveSpeed = 5f;
        rotateSpeed= 10f;
        maxMoveDistance = 4;
    }
    private void Update()
    {
        if (!isActive) //If not performing an action (being active), return
        {
            return; 
        }
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) //If we reached the target, don't move
        {
            unitAnimator.SetBool("isWalking", true); //We set the bool to true so the Animator transitions to walking
            Vector3 moveDirection = (targetPosition - transform.position).normalized; //We just want the direction, not the magnitude
            transform.position += moveDirection * Time.deltaTime * moveSpeed; //To make it frame-independent, we use Time.deltaTime
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed); //We set the facing of the unit to the moveDirection 
        }
        else
        {
            unitAnimator.SetBool("isWalking", false);
            ActionComplete();
        }
    }

    //Method to move the unit to the target
    public override void TakeAction(GridPosition targetGridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition); //So "this" instance gets the value set

    }

    //Returns a list with all the grid positions where the unit can move (remember this script is attached to the unit)
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validPositionList = new List<GridPosition>(); //Empty list of valid grid positions to move
        GridPosition unitGridPosition = unit.GetGridPosition(); //The position of the unit on the grid
        
        //We cicle through all the possible Gridposition depending on the move distance
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){
                    continue; //if the GridPosition is not inside the grid bounds, skip to the next iteration
                }

                if(testGridPosition == unitGridPosition)
                {
                    continue; //if the GridPosition is the same than the one the unit is standing, skip it
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue; //If the Gridposition is occupied by another unit, skip it
                }

                validPositionList.Add(testGridPosition);
                //Debug.Log(testGridPosition);
            }
        }
        return validPositionList;
    }


    public override string GetActionName() //Return the name of the function (string specified by us)
    {
        return "Move";
    }
}
