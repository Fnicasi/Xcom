using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    private float timer;


    private void Start()
    {
        timer = 2f;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    //A countdown to give the turn back to the player 
    private void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer<= 0f)
        {//If timer runs out, change turn to the player
            TurnSystem.Instance.NextTurn();
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {
        timer = 2f;
    }
}
