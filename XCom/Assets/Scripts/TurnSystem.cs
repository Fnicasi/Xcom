using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; } //Singleton property, this way it can be accesed by other scripts easily (get)

    public event EventHandler OnTurnChanged;

    private int turnNumber;
    private bool isPlayerTurn;
    private void Awake()
    {
        if (Instance != null) //Just in case another TurnSystem was created incorrectly 
        {
            Debug.Log("There's more than one UnitActionSystem" + transform + " - " + Instance);
            Destroy(Instance);
            return;
        }
        Instance = this;

        turnNumber = 1;
        isPlayerTurn= true;
    }
    public void NextTurn()
    {
        turnNumber++; 
        isPlayerTurn = !isPlayerTurn; //Change from player to enemy's turn and viceversa
        OnTurnChanged?.Invoke(this, EventArgs.Empty); //Call the OnTurnChange Event
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
