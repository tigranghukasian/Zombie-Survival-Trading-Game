using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEventChecker : MonoBehaviour
{
    public Action OnHit { get; set; }
    
    public void Hit()
    {
        OnHit?.Invoke();
    }
}
