using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using NavMeshSurface = Unity.AI.Navigation.NavMeshSurface;

public class AreaBaker : Singleton<AreaBaker>
{
    [SerializeField] private NavMeshSurface surface;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private float updateRate = 0.1f;
    [SerializeField] private float movementThreshold = 3;

    [SerializeField] private Vector3 navmeshSize = new Vector3(20, 20, 20);

    private Vector3 worldAnchor;
    private NavMeshData navmeshData;
    private List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
    private List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();

    private Bounds navmeshBounds;

    public Action<Bounds> OnNavmeshUpdate { get; set; }


    private void Start()
    {
        navmeshData = new NavMeshData();
        NavMesh.AddNavMeshData(navmeshData);
        BuildNavmesh(false);
        StartCoroutine(CheckPlayerMovement());
    }

    private IEnumerator CheckPlayerMovement()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (true)
        {
            if (Vector3.Distance(worldAnchor, playerTransform.position) > movementThreshold)
            {
                navmeshBounds = new Bounds(playerTransform.position, navmeshSize);
                BuildNavmesh(true);
                worldAnchor = playerTransform.position;
                OnNavmeshUpdate?.Invoke(navmeshBounds);
            }

            yield return wait;
        }
    }

    private void BuildNavmesh(bool async)
    {
        Bounds navmeshBounds = new Bounds(playerTransform.position, navmeshSize);
        List<NavMeshModifierVolume> modifiers;
        // modifiers = new List<Unity.AI.Navigation.NavMeshModifierVolume>(surface.GetComponentsInChildren<NavMeshModifierVolume>());
        //
        // for (int i = 0; i < modifiers.Count; i++)
        // {
        //     markups.Add(new NavMeshBuildMarkup
        //     {
        //         root = modifiers[i].transform,
        //         overrideArea = true,
        //         area = modifiers[i].area,
        //         ignoreFromBuild = false
        //     });
        // }

        if (surface.collectObjects == Unity.AI.Navigation.CollectObjects.Children)
        {
            NavMeshBuilder.CollectSources(surface.transform, surface.layerMask, surface.useGeometry,
                surface.defaultArea, markups, sources);
        }
        else
        {
            NavMeshBuilder.CollectSources(navmeshBounds, surface.layerMask, surface.useGeometry,
                surface.defaultArea, markups, sources);
        }

        // sources.RemoveAll(source =>
        //     source.component != null && source.component.gameObject.GetComponent<NavMeshAgent>() != null);


        if (async)
        {
            NavMeshBuilder.UpdateNavMeshDataAsync(navmeshData, surface.GetBuildSettings(), sources,
                navmeshBounds);
        }
        else
        {
            NavMeshBuilder.UpdateNavMeshData(navmeshData, surface.GetBuildSettings(), sources,
                navmeshBounds);
        }
    }
}
