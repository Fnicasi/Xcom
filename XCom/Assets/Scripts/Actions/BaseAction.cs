using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//abstract so it's never created
public abstract class BaseAction : MonoBehaviour
{
    //protected so private except for the classes that extend from this one (MoveAction, SpinAction,...)
    protected Unit unit;
    protected bool isActive; //Check if the unit is active(performing an action), if true, allow actions on update
    protected Animator unitAnimator;

    protected Action OnActionComplete;


    //Virtual so it can be overrided
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        unitAnimator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
