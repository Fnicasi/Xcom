using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 moveDirection;
    private float moveSpeed;
    private float distanceBeforeMoving;
    private float distanceAfterMoving;

    [SerializeField] private Transform bulletHitVFXPrefab; 
    [SerializeField] private TrailRenderer trailRenderer;
    public void SetUp(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        moveDirection = (targetPosition - transform.position).normalized;
        moveSpeed = 200f;

    }
    private void Update()
    {
        //To avoid overshooting the target (the bullet trails goes further than the target)
        distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        //This way we can check if the bullet is about to pass the target, to be destroyed
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition; //Advance no further than the target
            trailRenderer.transform.parent = null; //Unparent the trail so when the projectile is destroyed, the trail lingers a little before dissapearing too (configured in editor as autodestruct)
            Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity); //Instantiate the impact vfx
            Destroy(gameObject);
        }
    }
}
