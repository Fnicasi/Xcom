using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
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
        }
    }
    public void Spin()
    {
        isActive= true;
    }
}
