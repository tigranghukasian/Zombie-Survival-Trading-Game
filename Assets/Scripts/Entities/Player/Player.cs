using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour, IDamageable, IDamager
{
    [SerializeField] private InventoryHolder playerInventoryHolder;
    [SerializeField] private InventoryHolder equipmentHolder;

    private int selectedEquipmentSlot = 0;
    [SerializeField] private Transform toolParentTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerAnimatorEventChecker playerAnimatorEventChecker;
    [SerializeField] private ToolDetection toolDetection;
    [SerializeField] private Rig pistolRig;
    [SerializeField] private PlayerInteractionDetector interactionDetector;
    private bool playingAnimation;
    private Equipable equippedItem;
    
    [field:SerializeField] public float MaxHealth { get; set; }
    [field:SerializeField] public float Health { get; set; }
    public event Action<IDamageable> OnDestroyed;
    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnMoneyIncreased;
    public event Action<int> OnMoneyDecreased;
    public event Action<int> OnMoneyChanged;
    public bool IsDead { get; set; }
    public Transform HealthbarTransform { get; set; }
    
    public Interactable CurrentInteractable { get; set; }
    
    public InventoryHolder PlayerInventoryHolder => playerInventoryHolder;
    
    public int Money { get; set; }

    private void Awake()
    {
        Application.targetFrameRate = -1;
        playerAnimatorEventChecker.OnHit += OnAnimationHit;
        pistolRig.weight = 0;
        interactionDetector.OnInteractableDetected += OnInteractableDetected;
        interactionDetector.OnInteractableUndetected += OnInteractableUnDetected;
        Money = 200;
    }

    private void Start()
    {
        for (int i = 0; i < equipmentHolder.Inventory.GetSlotsLength(); i++)
        {
            equipmentHolder.Inventory.GetSlot(i).inventorySlotUpdatedCallback += SlotUpdated;
        }
        SelectEquipmentSlot(0);

    }

    public bool HasMoney(int amount)
    {
        return Money >= amount;
    }

    public void DecreaseMoney(int amount)
    {
        Money -= amount;
        OnMoneyChanged?.Invoke(Money);
        OnMoneyIncreased?.Invoke(amount);
    }

    public void IncreaseMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
        OnMoneyIncreased?.Invoke(amount);
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

        HandleInteractLogic();
        
    }

    private void OnInteractableDetected(Interactable _interactable)
    {
        CurrentInteractable = _interactable;
        GameUIManager.Instance.EnableInteractableIcon(CurrentInteractable.InteractableIconTransform.position);
    }

    private void OnInteractableUnDetected(Interactable _interactable)
    {
        if (_interactable.HasInteracted)
        {
            _interactable.UnInteract();
        }
        CurrentInteractable = null;
        GameUIManager.Instance.DisableInteractableIcon();
    }

    private void HandleInteractLogic()
    {
        
        if (CurrentInteractable != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!CurrentInteractable.HasInteracted)
                {
                    CurrentInteractable.Interact();
                    
                }
                else
                {
                    CurrentInteractable.UnInteract();
                }
            }
        }
        else
        {
            GameUIManager.Instance.DisableInteractableIcon();
        }
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
                pistolEquippable.Inventory = playerInventoryHolder.Inventory;
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
            playerInventoryHolder.Inventory.AddItem(droppedItem.Item, droppedItem.Amount);
            Destroy(droppedItem.gameObject);
        }
    }
}
