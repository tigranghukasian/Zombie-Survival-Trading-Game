using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructurePlacementManager : Singleton<StructurePlacementManager>
{
    public enum GhostType
    {
        CanPlace,
        CannotPlace
    }
    private Camera mainCamera;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Material ghostMat;
    [SerializeField] private List<GhostColorType> ghostColorTypes = new List<GhostColorType>();
    private GameObject currentActiveStructurePrefab;
    private StructureGhost structureGhost;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void SetCurrentActiveStructurePrefab(GameObject structurePrefab)
    {
        currentActiveStructurePrefab = structurePrefab;
        if (structureGhost != null)
        {
            Destroy(structureGhost.gameObject);
        }
        if (structurePrefab != null)
        {
            var mousePointInWorld = GetMousePointInWorld();
            var ghostObject = Instantiate(currentActiveStructurePrefab, mousePointInWorld, Quaternion.identity);
            RemoveAllComponents(ghostObject);
            SetMaterialsToTransparent(ghostObject);
            SetColliderToTrigger(ghostObject);
            AddRigidbody(ghostObject);
            ghostObject.layer = LayerMask.NameToLayer("StructureGhost");
            structureGhost = ghostObject.AddComponent<StructureGhost>();
        }
        
    }

    private bool CanSpawnBuilding()
    {
        return structureGhost.CollidersInRange.Count == 0;
    }

    private void AddRigidbody(GameObject ghostObject)
    {
        var rb = ghostObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }
    private void SetColliderToTrigger(GameObject ghostObject)
    {
        var colliders = ghostObject.GetComponents<Collider>();
        if (colliders.Length == 0)
        {
            Debug.LogError("No Collider on this building type");
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].isTrigger = true;
        }
    }

    private void RemoveAllComponents(GameObject ghostObject)
    {
        foreach (var script in ghostObject.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }
    }

    private void SetGhostType(GhostType type)
    {
        var color = ghostColorTypes.FirstOrDefault(x => x.GhostType == type).Color;
        ghostMat.color = color;
    }
    private void SetMaterialsToTransparent(GameObject ghostObject)
    {
        Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();
        
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] newMaterials = new Material[renderers[i].materials.Length];
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                newMaterials[j] = ghostMat; 
            }
            renderers[i].materials = newMaterials;
        }
    }
    

    private void Update()
    {
        if (!GameUIManager.Instance.IsMouseOverUI())
        {
            if (Input.GetMouseButtonDown(0) && currentActiveStructurePrefab != null && CanSpawnBuilding()) {
                var mousePointInWorld = GetMousePointInWorld();
                var structure = Instantiate(currentActiveStructurePrefab, mousePointInWorld, structureGhost.transform.rotation);
                
                InventorySelectionManager.RemoveItemFromSelectedSlotData(1);
            }

            if (currentActiveStructurePrefab != null)
            {
                structureGhost.transform.position = GetMousePointInWorld();
                if (CanSpawnBuilding())
                {
                    SetGhostType(GhostType.CanPlace);
                }
                else
                {
                    SetGhostType(GhostType.CannotPlace);
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    structureGhost.transform.Rotate(0, 45, 0); // Assuming Y-axis rotation
                }
            }
        }
    }

    private Vector3 GetMousePointInWorld()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
[System.Serializable]
public class GhostColorType
{
    [field: SerializeField] public StructurePlacementManager.GhostType GhostType { get; set; }
    [field: SerializeField] public Color Color { get; set; }
}
