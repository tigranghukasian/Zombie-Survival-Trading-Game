using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Camera mainCamera;
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
        if (groundPlane.Raycast(ray, out float point)) {
            Vector3 rayPoint = ray.GetPoint(point);
            Vector3 direction = rayPoint - transform.position;
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, -angle + offset, 0));
        }

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
