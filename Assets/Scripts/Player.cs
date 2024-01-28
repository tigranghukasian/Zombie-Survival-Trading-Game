using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour, IDamageable, IDamager
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventoryHolder equipmentHolder;

    private int selectedEquipmentSlot = 0;
    [SerializeField] private Transform toolParentTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerAnimatorEventChecker playerAnimatorEventChecker;
    [SerializeField] private ToolDetection toolDetection;
    [SerializeField] private Rig pistolRig;
    private bool playingAnimation;
    private Equipable equippedItem;
    
    [field:SerializeField] public float MaxHealth { get; set; }
    [field:SerializeField] public float Health { get; set; }
    public event Action<IDamageable> OnDestroyed;
    public event Action<float, float> OnHealthChanged;
    public bool IsDead { get; set; }
    public Transform HealthbarTransform { get; set; }
    
    public InventoryHolder InventoryHolder => inventoryHolder;

    private void Awake()
    {
        Application.targetFrameRate = -1;
        playerAnimatorEventChecker.OnHit += OnAnimationHit;
        pistolRig.weight = 0;
    }

    private void Start()
    {
        for (int i = 0; i < equipmentHolder.Inventory.GetSlotsLength(); i++)
        {
            equipmentHolder.Inventory.GetSlot(i).inventorySlotUpdatedCallback += SlotUpdated;
        }
        SelectEquipmentSlot(0);
        
    }
    
    public void TakeDamage(float amount, IDamager damager)
    {
        Health -= amount;
        OnHealthChanged?.Invoke(Health, MaxHealth);
        if (Health <= 0)
        {
            Health = 0;
            Kill();
        }
    }

 
    public void Kill()
    {
        //TODO: LOSE WHEN PLAYER DIES
    }

    private void SlotUpdated(InventorySlot slot)
    {
        if(equipmentHolder.Inventory.GetSlot(selectedEquipmentSlot) == slot)
        {
            DestroyCurrentEquippedItem();
            EquipSlot(slot);
        }
    }

    private void OnAnimationHit()
    {
        playingAnimation = false;
        if (equippedItem is ToolEquipable)
        {
            ToolEquipable toolEquipable = (ToolEquipable)equippedItem;
            toolEquipable.Fire();
        }

        
    }

    private void Update()
    {
        CheckKeysForSelectingEquipment();
        
        if (!GameUIManager.Instance.IsMouseOverUI() && !InventorySelectionManager.IsSlotSelected)
        {
            
            if (Input.GetMouseButton(0) && equippedItem != null)
            {
                equippedItem.Use();
                if (equippedItem is ToolEquipable && !playingAnimation)
                {
                    playingAnimation = true;
                    animator.SetTrigger("Swing");
                }
            }

            if (Input.GetMouseButtonDown(0) && equippedItem != null)
            {
                if (equippedItem is PistolEquipable)
                {
                    equippedItem.Fire();
                }
            }
        }
        

        // if (Input.GetMouseButtonDown(0) && eqippedItem != null)
        // {
        //     
        // }
    }


    private void CheckKeysForSelectingEquipment()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectEquipmentSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectEquipmentSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectEquipmentSlot(2);
        }
    }

    private void SelectEquipmentSlot(int slotIndex)
    {
        selectedEquipmentSlot = slotIndex;
        var slot = equipmentHolder.Inventory.GetSlot(selectedEquipmentSlot);
        equipmentHolder.InventoryUI.AddSelectedCrownToSlot(slot);
        DestroyCurrentEquippedItem();
        EquipSlot(slot);
    }

    private Item SelectedItem()
    {
        var slot = equipmentHolder.Inventory.GetSlot(selectedEquipmentSlot);
        return equipmentHolder.Inventory.ItemDatabase.GetItem(slot.Id);
    }

    private void DestroyCurrentEquippedItem()
    {
        if (equippedItem != null)
        {
            Destroy(equippedItem.gameObject);
        }
    }

    private void EquipSlot(InventorySlot slot)
    {
        if (slot.Id == -1)
        {
            return;
        }

        ItemEquipable itemEquipable = (ItemEquipable)equipmentHolder.Inventory.ItemDatabase.GetItem(slot.Id);

        var playerDisplay = itemEquipable.playerDisplay;
        if (playerDisplay != null)
        {
            var playerItem = Instantiate(playerDisplay, transform);
            equippedItem = playerItem.GetComponent<Equipable>();
            equippedItem.OnEquip();
            equippedItem.Owner = this;
            animator.SetBool("holdingPistol", false);
            pistolRig.weight = 0;
            if (equippedItem is ToolEquipable)
            {
                ToolEquipable toolEquipable = (ToolEquipable)equippedItem;
                toolEquipable.ToolDetection = toolDetection;
                toolEquipable.transform.SetParent(toolParentTransform, false);
            }
            if (equippedItem is PistolEquipable)
            {
                PistolEquipable pistolEquippable = (PistolEquipable)equippedItem;
                pistolEquippable.PlayerTransform = transform;
                equippedItem.transform.SetParent(toolParentTransform, false);
                animator.SetBool("holdingPistol", true);
                pistolRig.weight = 1;
            }
        }
    }

    public void OnTriggerEnter(Collider _other)
    {
        if (_other.TryGetComponent(out DroppedItem droppedItem))
        {
            Debug.Log(droppedItem.Item);
            inventoryHolder.Inventory.AddItem(droppedItem.Item, droppedItem.Amount);
            Destroy(droppedItem.gameObject);
        }
    }
}
