using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
       // animator = GetComponentInChildren<Animator>();
        //In case a unit does not have the movement action (a turret), so try to get the component
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs arg)
    {
        animator.SetBool("IsWalking",true);
    } 
    private void MoveAction_OnStopMoving(object sender, EventArgs arg)
    {
        Debug.Log("Called");
        animator.SetBool("IsWalking",false);
    }
    private void ShootAction_OnShoot(object sender, EventArgs arg)
    {
        animator.SetTrigger("Shoot");
    }

}
