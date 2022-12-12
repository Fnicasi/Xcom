using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnVisualGameObject;
    private void Start()
    {
        endTurnButton.onClick.AddListener(() => TurnSystem.Instance.NextTurn()); //When clicking on the button, call the NextTurn function
        UpdateTurnNumberText();
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnNumberText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args) {
        UpdateTurnNumberText(); //When the event turn change happens, call this function to update the turn number text
        UpdateEnemyTurnVisual(); //When turn changes, update the enemy turn banner
        UpdateEndTurnButtonVisibility(); //If it's the enemies turn, hide the button End Turn, if it's the player's, show it 
    }
    private void UpdateTurnNumberText()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }
    private void UpdateEnemyTurnVisual()
    {//If it's not the player's turn, then set the banner of enemy turn as active
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
