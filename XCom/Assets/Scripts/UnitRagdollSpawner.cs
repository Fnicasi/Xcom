using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem= GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead; //Subscribe to event 
    }

    private void HealthSystem_OnDead(object sender, EventArgs args)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, this.transform.position, this.transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.SetUp(originalRootBone); //We pass to the ragdoll, the root bone of the original unit
    }
}
