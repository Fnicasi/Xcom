using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

}
