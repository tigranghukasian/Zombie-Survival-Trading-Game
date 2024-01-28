using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool HasInteracted { get; set; }
    public void Interact();
    event Action<IInteractable> OnInteract;
    event Action<IInteractable> OnUnInteract;
}
