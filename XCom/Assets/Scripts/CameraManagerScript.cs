using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerScript : MonoBehaviour
{
    //To manage the camera movements, we could do a singleton like UnitActionSystem, or events for every shoot action, or what we will do, add an event in the BaseAction class
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera(); //To make sure the camera taken is not the Action (over the shouler on shoot) when starting (ActionCamera has higher priority)


    }
    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera() 
    { 
        actionCameraGameObject.SetActive(false); 
    }

    public void BaseAction_OnAnyActionStarted(object sender, EventArgs args)
    {
        // We could do an if(sender is ShootAction) or to have options for other actions....
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetComponent<Unit>();
                Unit targetUnit = shootAction.GetTargetUnit();
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f; //Get the shoulder height
                float shoulderOffsetAmount = 0.5f; //Add a shoulder offset


                Vector3 shootDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized; //Get the direction of the target

                Vector3 shoulderOffset = Quaternion.Euler(0f, 90f, 0f) * shootDirection * shoulderOffsetAmount; //shootDirection to rotate the vector and add should offset

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDirection * -1);

                actionCameraGameObject.transform.position = actionCameraPosition; //Set the action camera to the shoulder of the shooter
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight); //Look at the target unit (at the correct height)
                ShowActionCamera();
                break;

        }
        
    } public void BaseAction_OnAnyActionCompleted(object sender, EventArgs args)
    {
        // We could do an if(sender is ShootAction) or to have options for other actions....
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;

        }
        
    }
}
