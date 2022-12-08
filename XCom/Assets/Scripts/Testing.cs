using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Unit unit; 
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            MoveAction moveAction = unit.GetMoveAction();
            moveAction.GetValidActionGridPositionList();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
