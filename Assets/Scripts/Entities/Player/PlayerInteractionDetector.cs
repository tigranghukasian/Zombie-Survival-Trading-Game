using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    
    [SerializeField] private float interactionCheckRadius = 3f;
    private Collider[] colliders = new Collider[20];
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable currentInteractable;
    private Interactable previousInteractable;
    public Action<Interactable> OnInteractableDetected;
    public Action<Interactable> OnInteractableUndetected;
    private void Update()
    {
        interactables.Clear();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i] = null;
        }
        Physics.OverlapSphereNonAlloc(transform.position, interactionCheckRadius, colliders);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == null)
            {
                break;
            }
            if (colliders[i].TryGetComponent(out Interactable interactable))
            {
                interactables.Add(interactable);
            }
        }
        if (interactables.Count == 0 )
        {
            if (previousInteractable != null)
            {
                OnInteractableUndetected?.Invoke(previousInteractable);
            }

            currentInteractable = null;
            previousInteractable = null;
        }
        else if (interactables.Count == 1)
        {
            currentInteractable = interactables[0];
        }
        else if (interactables.Count >= 1)
        {
            float smallestDistance = Vector3.Distance(interactables[0].transform.position, transform.position);
            Interactable closestInteractable = interactables[0];
            for (int i = 0; i < interactables.Count; i++)
            {
                var dist = Vector3.Distance(interactables[i].transform.position, transform.position);
                if (dist < smallestDistance)
                {
                    smallestDistance = dist;
                    closestInteractable = interactables[i];
                }
            }

            currentInteractable = closestInteractable;
        }

        if (currentInteractable != null && currentInteractable != previousInteractable)
        {
            OnInteractableDetected?.Invoke(currentInteractable);
            previousInteractable = currentInteractable;
        }

    }
}
