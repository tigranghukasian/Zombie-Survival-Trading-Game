using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ResourceObject : Damageable
{

    [SerializeField] private Item resourceItem;
    [SerializeField] private int amountToGive;

    private Player Damager;


    public override void TakeDamage(float amount, Player _damager)
    {
        base.TakeDamage(amount, _damager);
        Damager = _damager;
        TriggerDamageEffect();
    }
    
    private void TriggerDamageEffect()
    {
        if (IsDead)
        {
            return;
        }
        float duration = 0.3f;
        float bobbingAmount = 0.2f;

        // Animate the object to move up and then back down
        transform.DOJump(transform.position, bobbingAmount, 1, duration);
    }
    

    public override void Kill()
    {
        base.Kill();
        Damager.InventoryHolder.Inventory.AddItem(resourceItem, amountToGive);
        GameUIManager.Instance.AddResourceAddedUI(resourceItem, amountToGive);
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}
