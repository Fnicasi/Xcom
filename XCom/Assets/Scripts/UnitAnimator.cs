using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
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
    //EventArgs is not Empty anymore, check ShootAction class to see why, args contains target unit and shooting unit
    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs args)
    {
        animator.SetTrigger("Shoot");

        Transform bulletTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletTransform.GetComponent<BulletProjectile>();
        
        Vector3 targetUnitShootAtPosition = args.targetUnit.GetWorldPosition();
        //With this, the height of the objective will be the same as the height of the weapon (this might change for enemies with different heights
        targetUnitShootAtPosition.y = shootPointTransform.position.y;  
        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }

}
