using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;


    private void Start() 
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged; //This will update for all units, which is a bit wasteful
        healthSystem.OnDamaged += HealthSystem_OnDamaged; //Whenever the unit (and its healthSystem) is damaged, use this method
        UpdateActionPointsText();
        UpdateHealthBar();
    }
    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs args)
    {
        UpdateActionPointsText();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized(); //Set the fill amount to the value of the health
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs args)
    {
        UpdateHealthBar(); //Whenever the unit is damaged, update it
    }
}
