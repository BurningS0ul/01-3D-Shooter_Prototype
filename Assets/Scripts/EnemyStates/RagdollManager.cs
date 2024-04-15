using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    Rigidbody[] rbs;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidBody in rbs)
        {
            rigidBody.isKinematic = true;
        }
        anim.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rbs)
        {
            rigidBody.isKinematic = false;
        }
        anim.enabled = false;
    }
}
