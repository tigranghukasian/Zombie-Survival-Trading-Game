using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthbarManager : Singleton<HealthbarManager>
{
    [SerializeField] private GameObject healthBarPrefab;

    private Camera mainCamera;
    private Dictionary<IDamageable, Healthbar> healthbars = new Dictionary<IDamageable, Healthbar>();

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
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
