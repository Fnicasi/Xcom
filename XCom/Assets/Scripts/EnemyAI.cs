using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn; //First turn belongs to the player
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))//Try to perform action, then set state as taking turn (so it tries again to perform an action)
                    {
                        state = State.Busy;
                    } else
                    { //No units remaining for the AI, pass turn

                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) //If it's the enemies turn...
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }
    //Takes an action, when completes it, does the action passed
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }
    //Currently, if there's a unit of the player within range, it will shoot at that unit
    //If not, it will move to the target grid position with more available targets and then shoot
    //If can't move or shot will spin
    //If can't afford to do an action will do nothing
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (!enemyUnit.CanSpendActionPoints(baseAction))
            {
                // Enemy cannot afford this action
                continue;
            }

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();//See what is the best course of action for this action
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }

        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPoints(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }

}
