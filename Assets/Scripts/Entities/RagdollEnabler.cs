using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform ragdollRoot;
    
    private List<JointData> storedJointData = new List<JointData>();

    private Rigidbody[] rigidbodies;
    private CharacterJoint[] joints;
    private Collider[] colliders;

    private void Awake()
    {
        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        //joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();
        StoreAndDestroyJoints();
        EnableAnimator();
    }
    
    private void StoreAndDestroyJoints()
    {
        joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        foreach (var joint in joints)
        {
            storedJointData.Add(new JointData(joint));
            Destroy(joint);
        }
    }

    public void EnableRagdoll()
    {
        RecreateJoints();
        animator.enabled = false;
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].velocity = Vector3.zero;
            rigidbodies[i].detectCollisions = true;
            rigidbodies[i].useGravity = true;
        }
        // for (int i = 0; i < joints.Length; i++)
        // {
        //     //joints[i].enableCollision = true;
        // }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
    }

    public void EnableAnimator()
    {
        
        animator.enabled = true;
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].detectCollisions = false;
            rigidbodies[i].useGravity = false;
        }
        // for (int i = 0; i < joints.Length; i++)
        // {
        //     joints[i].enableCollision = false;
        // }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
    
    private void RecreateJoints()
    {
        foreach (var jointData in storedJointData)
        {
            CharacterJoint newJoint = jointData.JointGameObject.AddComponent<CharacterJoint>();
            newJoint.anchor = jointData.Anchor;
            newJoint.axis = jointData.Axis;
            newJoint.connectedAnchor = jointData.ConnectedAnchor;
            newJoint.connectedBody = jointData.ConnectedBody;
            newJoint.enableCollision = true;
            // Set other properties as needed
        }
    }
}

[System.Serializable]
public class JointData
{
    public Vector3 Anchor;
    public Vector3 Axis;
    public Vector3 ConnectedAnchor;
    public Rigidbody ConnectedBody;
    public GameObject JointGameObject;
    // Add other properties as needed

    public JointData(CharacterJoint joint)
    {
        Anchor = joint.anchor;
        Axis = joint.axis;
        ConnectedAnchor = joint.connectedAnchor;
        ConnectedBody = joint.connectedBody;
        JointGameObject = joint.gameObject;
        // Initialize other properties as needed
    }
}
