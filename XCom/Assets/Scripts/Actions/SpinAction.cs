using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{
    //Delegates, just as any other function, can return different types and receive arguments
    //public delegate void SpinCompleteDelegate();
    //private SpinCompleteDelegate onSpinComplete;
    //private Action onSpinComplete; //This line equals the delegeta above, this is an existing delegate in Unity.System, we will use OnActionComplete from BaseAction


    private float totalSpinAmount;
    private float addSpinAmount;

    
    private void Update()
    {
        if (!isActive)//If not performing an action (being active), return
        {
            return;
        }

        addSpinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, addSpinAmount, 0); //Rotate on the Y

        totalSpinAmount += addSpinAmount;
        if(totalSpinAmount >= 360f)
        {
            isActive= false;
            OnActionComplete(); //Call the delegate
        }
    }

    //Spin has the delegate SpinCompleteDelegate as argument and, when it finishes it will call the method passed as argument (ClearBusy in this case)
    public void Spin(Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        totalSpinAmount = 0;
        isActive = true;
    }
    public override string GetActionName()
    {
        return "Spin";
    }
}
