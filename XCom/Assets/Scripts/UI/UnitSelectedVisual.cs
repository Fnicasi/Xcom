using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Green circle around the selected character
public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer= GetComponent<MeshRenderer>(); //Get the quad reference so we can disable/enable it
        unit= GetComponentInParent<Unit>();  //Get a reference of the unit (in the parent)
    }

    private void Start() //Whenever the OnSelectedUnitChanged Event is called, the UnitActionSystem_OnSelectedUnitChanged will be executed
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged; //Here is the subscription mentioned above
        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
       UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false; 
        }
    }
    //When the unit is destroyed, this causes the MeshRenderer to encounter a bug because it tries to deselect the unit by disabling the meshRenderer on the UpdateVisual function
    //but since the gameObject is destroyed... there's no meshRender, so we will use this built in function
    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged; //We unsubscribe from the event that calls the Update Visual Method

    }

}
