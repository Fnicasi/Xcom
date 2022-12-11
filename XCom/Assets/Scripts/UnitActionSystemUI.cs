using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged; //Subscribe to the event that the selected unit changes
        CreateUnitActionButtons();
    }
    void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject); //Clean all buttons before updating with the new ones
        }
        //we want to make buttons for the actions that the unit has
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        
        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform); //Instantiate Button prefab in the container
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>(); //Grab the ActionButtonUi element
            actionButtonUI.SetBaseAction(baseAction); //SetBaseAction of the action 
             
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs args)
    {//If selected unit changes, call the create buttons again, for units with different actions
        CreateUnitActionButtons();
    }
}
