using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform ragdollRoot;

    private Rigidbody[] rigidbodies;
    private CharacterJoint[] joints;
    private Collider[] colliders;

    private void Awake()
    {
        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();
        EnableAnimator();
    }
    
    public void EnableRagdoll()
    {
        animator.enabled = false;
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].velocity = Vector3.zero;
            rigidbodies[i].detectCollisions = true;
            rigidbodies[i].useGravity = true;
        }
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].enableCollision = true;
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
    }

    private void EnableAnimator()
    {
        animator.enabled = true;
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].detectCollisions = false;
            rigidbodies[i].useGravity = false;
        }
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].enableCollision = false;
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
}
