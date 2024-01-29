using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ResourceObject : Damageable
{

    [SerializeField] private Item resourceItem;
    [SerializeField] private int amountToGive;

    private Player Player;


    public override void TakeDamage(float amount, IDamager damager)
    {
        base.TakeDamage(amount, damager);
        TriggerDamageEffect();
        Player = (Player)damager;
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
        Player.PlayerInventoryHolder.Inventory.AddItem(resourceItem, amountToGive);
        GameUIManager.Instance.AddResourceAddedUI(resourceItem, amountToGive);
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}
