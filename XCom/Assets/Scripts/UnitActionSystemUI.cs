using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonsUIList; //List of buttons

    private void Awake()
    {
        actionButtonsUIList = new List<ActionButtonUI>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged; //Subscribe to the event that the selected unit changes
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged; //Subscribe to the event that the selected action changes
    
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }
    void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject); //Clean all buttons before updating with the new ones
        }
        actionButtonsUIList.Clear(); //Clear the list of buttons

        //we want to make buttons for the actions that the unit has
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        
        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform); //Instantiate Button prefab in the container
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>(); //Grab the ActionButtonUi element
            actionButtonUI.SetBaseAction(baseAction); //SetBaseAction of the action 
            actionButtonsUIList.Add(actionButtonUI); //Add the button to the list of buttons
            Debug.Log(baseAction);
             
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs args)
    {//If selected unit changes, call the create buttons again, for units with different actions
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs args)
    {//If selected action changes, call the update actions
        UpdateSelectedVisual();
    }
    
    //To update which action is highlighted
    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonsUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}
