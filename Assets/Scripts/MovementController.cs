using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform aimTransform;
    
    private Rigidbody rb;
    private Vector3 movementVector;

    private Vector3 forceToApply;
    private float offset;
    
    private float maxVelocityChange = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0 ,Input.GetAxisRaw("Vertical"));
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        Vector3 directionToMousePos = Vector3.forward;
        if (groundPlane.Raycast(ray, out float point)) {
            Vector3 rayPoint = ray.GetPoint(point);
            directionToMousePos = rayPoint - transform.position;
            float angleToRotate = Mathf.Atan2(directionToMousePos.z, directionToMousePos.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, -angleToRotate + offset, 0));
            aimTransform.transform.position = rayPoint;
        }

       

        float angle = Vector3.SignedAngle(Vector3.forward, directionToMousePos, Vector3.up);

        // Rotate input vector by this angle
        Vector3 rotatedInput = RotateVector(movementVector, angle);

        // Use the rotated vector for animation, converting to 2D
        Vector2 animationVector = new Vector2(rotatedInput.x, rotatedInput.z);
        

        // Use the transformed vector for animation
        animator.SetFloat("HorizontalInput", Mathf.Clamp(animationVector.x, -1, 1));
        animator.SetFloat("VerticalInput", Mathf.Clamp(animationVector.y, -1, 1));
        animator.SetBool("Running", movementVector != Vector3.zero);
       // animator.SetFloat("HorizontalInput", rotatedInputVector.x);
       // animator.SetFloat("VerticalInput", rotatedInputVector.z);

    }
    
    Vector3 RotateVector(Vector3 vector, float angle) {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);
        return new Vector3(vector.x * cos - vector.z * sin, 0, vector.x * sin + vector.z * cos);
    }

    private void FixedUpdate()
    {

        Vector3 desiredVelocity = movementVector.normalized * speed;
        Vector3 velocityChange = desiredVelocity - rb.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0; // Prevent changes in the vertical direction

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
}
