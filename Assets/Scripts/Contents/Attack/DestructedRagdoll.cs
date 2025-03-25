using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedRagdoll : MonoBehaviour, IDestructible
{
    //public Ragdoll ragdollPrefab;

    //public float force = 5f;
    //public float lift = 0.1f;
    //public float duration = 3f;

    public void OnDestruction(GameObject attacker)
    {
        //Vector3 direction = transform.position - attacker.transform.position;
        //direction.y += lift;
        //direction.Normalize();

        //Ragdoll ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        //ragdoll.AddForce(direction * force);

        //Destroy(ragdoll.gameObject, duration);
    }
}
