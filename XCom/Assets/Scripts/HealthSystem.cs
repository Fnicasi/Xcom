using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    [SerializeField] private int health;
    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() //Used in the UnitWorldUI to control the fill amount of the health display
    {
        return (float)health / healthMax; //If we cast one int as float, we will obtain a float in the result
    }
}
