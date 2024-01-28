using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    public event Action<IInteractable> OnInteract;
    public event Action<IInteractable> OnUnInteract;
    [field: SerializeField] public Transform InteractableIconTransform { get; set; }
    public bool HasInteracted { get; set; }

    public virtual void Interact()
    {
        HasInteracted = true;
        OnInteract?.Invoke(this);
    }

    public virtual void UnInteract()
    {
        HasInteracted = false;
        OnUnInteract?.Invoke(this);
    }
    

  
}
