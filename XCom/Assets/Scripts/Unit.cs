using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Animator unitAnimator;
    private GridPosition gridPosition; //To keep track of which gridPosition it's occupying
    [SerializeField] private MoveAction moveAction; //The move action is stored in the MoveAction class
    [SerializeField] private SpinAction spinAction;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        unitAnimator = GetComponentInChildren<Animator>();
        spinAction= GetComponent<SpinAction>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position); //The unit finds its gridPosition at the beggining
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); //and adds it to the grid  
    }
    private void Update()
    {
       
        if (unitAnimator.GetBool("isWalking")) //If the unit is walking, update its grid position while moving
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
  
}
