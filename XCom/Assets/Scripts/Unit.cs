using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    //Since it's static, it will be the same for all instances of the same class, so if any instance calls it, it will be called
    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;

    [SerializeField]public Animator unitAnimator;
    private GridPosition gridPosition; //To keep track of which gridPosition it's occupying
    [SerializeField] private MoveAction moveAction; //The move action is stored in the MoveAction class
    [SerializeField] private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        //unitAnimator = GetComponentInChildren<Animator>();
        spinAction= GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>(); //This will get all components that extend from BaseAction
        actionPoints = ACTION_POINTS_MAX;
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position); //The unit finds its gridPosition at the beggining
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); //and adds it to the grid  

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }
    private void Update()
    {
       
        if (unitAnimator.GetBool("IsWalking")) //If the unit is walking, update its grid position while moving
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position); //grid position that checks if its different (this one is in real time) than the current one (in code)
            if (newGridPosition != gridPosition) //If its new Grid Position is different, then it moved and we need to update the grid occupancy
            {
                LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition); //set the unit as having occupied newGridPosition and clear gridPosition of unit
                gridPosition = newGridPosition; //Set it as current position
            }
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPoints(BaseAction baseAction)
    {
        if(CanSpendActionPoints(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }
        return false;
    }
    public bool CanSpendActionPoints(BaseAction baseAction)
    {
        if(actionPoints >= baseAction.GetActionPointCost())
        {
            return true;
        }
        return false;
    }
    private void SpendActionPoints(int amountSpend)
    {
        actionPoints -= amountSpend;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {//If it's an enemy and it's the enemies turn or if it's a controllable unit and it's the player's turn...
        if (isEnemy && !TurnSystem.Instance.IsPlayerTurn() || !isEnemy && TurnSystem.Instance.IsPlayerTurn())
        {//Refresh the action points an invoke the event that is used update the display of action points available
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }
    public Vector3 GetWorldPosition()
    {
        return transform.position;  
    }
    public void Damage()
    {
        Debug.Log(transform + " !ouch!");
    }
}
