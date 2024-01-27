using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private GameObject resourceAddedUIPrefab;
    [SerializeField] private Transform resourceAddedUIParent;

    private Camera mainCamera;
    private Dictionary<IDamageable, Healthbar> healthbars = new Dictionary<IDamageable, Healthbar>();

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public void AddResourceAddedUI(Item item, int amount)
    {
        ResourceAddedUI raui = Instantiate(resourceAddedUIPrefab, resourceAddedUIParent).GetComponent<ResourceAddedUI>();
        raui.transform.position = resourceAddedUIParent.transform.position;
        raui.Init(item.sprite, amount);
    }

    public void ShowHealthbar(IDamageable damageable)
    {
        if (!healthbars.ContainsKey(damageable))
        {
            var healthBarpos = mainCamera.WorldToScreenPoint(damageable.HealthbarTransform.position);
            Healthbar healthbar = Instantiate(healthBarPrefab,healthBarpos,Quaternion.identity, transform).GetComponent<Healthbar>();
            healthbar.FollowTransform = damageable.HealthbarTransform;
            healthbar.Setup(damageable,  () => RemoveHealthBarFromDictionary(damageable));
            healthbars.Add(damageable, healthbar);
        }
    }


    private void RemoveHealthBarFromDictionary(IDamageable damageable)
    {
        if (healthbars.ContainsKey(damageable))
        {
            healthbars.Remove(damageable);
        }
    }
}
