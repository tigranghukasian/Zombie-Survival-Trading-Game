using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFader))]
public class Healthbar : MonoBehaviour
{

    private Action<float, float> healthChangedAction;
    private Action<IDamageable> damageableDestroyedAction;
    private Action removeFromDictionaryAction;
    private Slider slider;
    public Transform FollowTransform { get; set; }
    private Camera mainCamera;

    //private bool alwaysActive;
    
    private float inactivityTimer = 0f;
    private float inactivityThreshold = 3f;
    private IDamageable damageable;
    private UIFader uiFader;
    private bool inactive = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        slider = GetComponent<Slider>();
        uiFader = GetComponent<UIFader>();
    }
    
    public void Setup(IDamageable _damageable, Action _removeFromDictionaryAction)
    {
        damageable = _damageable;

        removeFromDictionaryAction = _removeFromDictionaryAction;
        healthChangedAction = (_health, _fullHealth) => UpdateHealth(_health, _fullHealth);
        damageable.OnHealthChanged += healthChangedAction;

        damageableDestroyedAction = (_) => DestroyHealthbar();
        damageable.OnDestroyed += damageableDestroyedAction;
    }

    public void UpdateHealth(float health, float fullHealth)
    {
        slider.value = health / fullHealth;
        ResetInactivityTimer();
    }
    private void ResetInactivityTimer()
    {
        inactivityTimer = 0f;
        inactive = false;
        uiFader.FadeIn(0, null);
    }
    public void DestroyHealthbar()
    {
        if (damageable != null)
        {
            damageable.OnHealthChanged -= healthChangedAction;
            damageable.OnDestroyed -= damageableDestroyedAction;
        }
        removeFromDictionaryAction?.Invoke();
        Destroy(gameObject);
    }

    private void OnFadeOutCompleted()
    {
        if (inactive)
        {
            DestroyHealthbar();
        }
    }
    private void Update()
    {
        transform.position = mainCamera.WorldToScreenPoint(FollowTransform.position);
        
        inactivityTimer += Time.deltaTime;
        if (inactivityTimer >= inactivityThreshold && !inactive)
        {
            inactive = true;
            uiFader.FadeOut(1,OnFadeOutCompleted);
        }
    }
}
