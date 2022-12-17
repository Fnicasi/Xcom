using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    private int maxShootDistance;
    private float stateTimer;
    private Unit targetUnit;
    private State state;
    private bool canShootBullet;
    private float rotateSpeed;
    private Vector3 aimDirection;
    private int damageAmount; //This value can be modified by different weapons (TO DO)

    public event EventHandler<OnShootEventArgs> OnShoot;
    //For the bulletProjectile, we need some way of knowing the target position, so we need to know the target unit
    //to achieve this we will extend the EventArgs class, this means that we can't use EventArgs.Empty when Invoking
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    };


    private void Start()
    {
        maxShootDistance = 7;
        rotateSpeed = 10f;
        damageAmount = 40;

    }

    private void Update()
    {
        if (!isActive)//If not performing an action (being active), return
        {
            return;
        }
        stateTimer -=Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break; 
            case State.Shooting:
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet= false;
                }
                break; 
            case State.Cooloff:

                break;
        }
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    public void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
               
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 2f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validPositionList = new List<GridPosition>(); //Empty list of valid grid positions to move
        GridPosition unitGridPosition = unit.GetGridPosition(); //The position of the unit on the grid

        //We cicle through all the possible Gridposition depending on the move distance
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue; //if the GridPosition is not inside the grid bounds, skip to the next iteration
                }

                //With this, the distance will be like a diamond instead of a square
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootDistance) 
                { 
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue; //If the Gridposition is not occupied by another unit, skip it
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy()) //If they are in the same team, continue
                {
                    continue;
                }

                
                
                
                validPositionList.Add(testGridPosition);
                //Debug.Log(testGridPosition);
            }
        }
        return validPositionList;
    }
    private void Shoot()
    {
        targetUnit.Damage(damageAmount);
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit= targetUnit,
            shootingUnit= unit //Got in the BaseAction class
        });
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {//When we want to take action by shooting, 

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition); //Find the target being shot at
        aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized; //Get the direction to the target

        canShootBullet = true;

        ActionStart(onActionComplete); //We call this function, that calls the event, and do it at the end to make sure everything is done before calling the event

    }

    public Unit GetTargetUnit()
    {
        return targetUnit;  
    }
}
