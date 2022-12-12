using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; } //Singleton property, this way it can be accesed by other scripts easily (get)

    public event EventHandler OnTurnChanged;

    private int turnNumber;
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
    }
    public void NextTurn()
    {
        turnNumber++;
        OnTurnChanged?.Invoke(this, EventArgs.Empty); //Call the OnTurnChange Event
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }
}
