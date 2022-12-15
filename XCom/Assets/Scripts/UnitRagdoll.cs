using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void SetUp(Transform originalRootBone) //We receive the root bone of the original unit
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            { 
                cloneChild.position= child.position;
                cloneChild.rotation= child.rotation;

                MatchAllChildTransforms(child, cloneChild); //Call the function recursively because the childs have childs in the bone hierarchy
                                                            //since we have the if(cloneChild != null), when there's no child, it will stop

                ApplyExplosionToRagdoll(ragdollRootBone, 5f, transform.position, 10f);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
