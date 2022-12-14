using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    //Since it's static, it will be the same for all instances of the same class, so if any instance calls it, it will be called
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;


    [SerializeField] private bool isEnemy;

    [SerializeField]public Animator unitAnimator;
    private GridPosition gridPosition; //To keep track of which gridPosition it's occupying
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;
    private int actionPoints;

    private void Awake()
    {
        healthSystem= GetComponent<HealthSystem>();
        //unitAnimator = GetComponentInChildren<Animator>();
        baseActionArray = GetComponents<BaseAction>(); //This will get all components that extend from BaseAction
        actionPoints = ACTION_POINTS_MAX;
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position); //The unit finds its gridPosition at the beggining
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); //and adds it to the grid  

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealtSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty); //Call this event when the unit is created
    }
    private void Update()
    {
       
        if (unitAnimator.GetBool("IsWalking")) //If the unit is walking, update its grid position while moving
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position); //grid position that checks if its different (this one is in real time) than the current one (in code)
            if (newGridPosition != gridPosition) //If its new Grid Position is different, then it moved and we need to update the grid occupancy
            {
                //Unit changed grid position
                GridPosition oldGridPosition = gridPosition;
                gridPosition = newGridPosition; //Set it as current position

                LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition); //set the unit as having occupied newGridPosition and clear gridPosition of unit
            }
        }
    }

    //Generic method to get all the possible actions, we extend from BaseAction so we force the generic to be of the type baseAction (shoot, move,...)
    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T) //If this baseAction is of type T... (for example we did GetAction<ShootAction>, if one of the actions in the array is of that type...)
            {
                return (T)baseAction; //...return the baseAction casted as type T (one of the actions that BaseAction extends, shoot, move,...)
            }
        }
        return null;
    }


    public GridPosition GetGridPosition()
    {
        return gridPosition;
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

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args) //Called when the event of turn changed is called
    {//If it's an enemy and it's the enemies turn or if it's a controllable unit and it's the player's turn...
        if (isEnemy && !TurnSystem.Instance.IsPlayerTurn() || !isEnemy && TurnSystem.Instance.IsPlayerTurn())
        {//Refresh the action points an invoke the event that is used update the display of action points available
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy() //Identify as enemy (AI) or controller by player
    {
        return isEnemy;
    }
    public Vector3 GetWorldPosition() //Return the Vector3 position (x,y,z) of this unit
    {
        return transform.position;  
    }
    public void Damage(int damageAmount) //Damage amount delivered
    {
        healthSystem.Damage(damageAmount);
    }
    private void HealtSystem_OnDead(object sender, EventArgs args) //When this unit dies...
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this); //Remove from the gridPosition (that this unit occupies), this unit
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }



}
