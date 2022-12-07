using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Animator unitAnimator;
    private Vector3 targetPosition; //Where the unit has to go
    private GridPosition gridPosition; //To keep track of which gridPosition it's occupying
    [SerializeField]
    private float moveSpeed; //At what speed it moves
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]

    private float stoppingDistance; //The distance at which the movement stops and we consider the unit has arrived to their destination

    private void Awake()
    {
        targetPosition = transform.position; 
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position); //The unit finds its gridPosition at the beggining
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); //and adds it to the grid  
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) //If we reached the target, don't move
        {
            unitAnimator.SetBool("isWalking", true); //We set the bool to true so the Animator transitions to walking
            Vector3 moveDirection = (targetPosition - transform.position).normalized; //We just want the direction, not the magnitude
            transform.position += moveDirection * Time.deltaTime * moveSpeed; //To make it frame-independent, we use Time.deltaTime
            transform.forward= Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed); //We set the facing of the unit to the moveDirection 
            
        }
        else
        {
            unitAnimator.SetBool("isWalking", false);
        }

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

    //Method to move the unit to the target
    public void Move(Vector3 targetPosition)
    {
        Debug.Log("Moving");
        this.targetPosition = targetPosition; //So "this" instance gets the value set
    }
}
