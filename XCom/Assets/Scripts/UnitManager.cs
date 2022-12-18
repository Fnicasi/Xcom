using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; } //Singleton property, this way it can be accesed by other scripts easily (get)


    private List<Unit> unitList;
    private List<Unit> friendlyList; //Player Units
    private List<Unit> enemyList; //AI units

    private void Awake()
    {
        if (Instance != null) //Just in case another UnitManager was created incorrectly 
        {
            Debug.Log("There's more than one UnitManager" + transform + " - " + Instance);
            Destroy(Instance);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        friendlyList= new List<Unit>(); 
        enemyList= new List<Unit>();
    }

    //Set priority in editor so the start of unitManager runs before the unit's start, or else....
    private void Start() 
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned; //...the event won't be listened and thus won't add that particular unit to the list
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs args)
    {
        Unit unit = sender as Unit; //Could also be done with casting (Unit)sender, but this way returns a null if the cast fails
        unitList.Add(unit); //Add the unit to the list of units, then check if enemy or friendly and add to the corresponding list
        if (unit.IsEnemy())
        {
            enemyList.Add(unit);
            Debug.Log(unit.name);
        }
        else
        {
            friendlyList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs args)
    {
        Unit unit = sender as Unit; //Could also be done with casting (Unit)sender, but this way returns a null if the cast fails
        unitList.Remove(unit); //Remove from the unit to the list of units, then check if enemy or friendly and remove to the corresponding list
        if (unit.IsEnemy())
        {
            enemyList.Remove(unit);
        }
        else
        {
            friendlyList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetEnemyList()
    {
        return enemyList;
    }
    public List<Unit> GetFriendlyList()
    {
        return friendlyList;
    }
}
