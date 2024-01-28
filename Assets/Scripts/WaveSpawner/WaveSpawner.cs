using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float firstPeacefulPeriodDuration = 20f;
    [SerializeField] private float initialPeaceDuration = 10f;
    [SerializeField] private float initialSpawnDuration = 10f;
    [SerializeField] private float peaceDurationIncrement = 1.5f;
    [SerializeField] private float spawnDurationIncrement = 2f;
    [SerializeField] private int initialEnemySpawnCount = 10;
    [SerializeField] private int enemySpawnIncrement = 2;
    private float minRadius = 5f;
    private float maxRadius = 30f;

    private WaveStateMachine waveStateMachine;
    public WavePeacefulState PeacefulState { get; private set; }
    public WaveSpawnState SpawnState { get; private set; }
    
    public delegate void StateChangedHandler(WaveState newState);
    public event StateChangedHandler OnStateChanged;

    private bool hasActivated;

    private void Awake()
    {
        waveStateMachine = new WaveStateMachine(OnStateChangedCallback);

        PeacefulState = new WavePeacefulState(this, waveStateMachine);
        SpawnState = new WaveSpawnState(this, waveStateMachine);
        IntroUI.OnIntroFinished += Activate;
    }

    private void Activate()
    {
        hasActivated = true;
        waveStateMachine.Initialize(PeacefulState, firstPeacefulPeriodDuration);
    }

    private void Update()
    {
        if (!hasActivated)
        {
            return;
        }
        waveStateMachine.CurrentWaveState.FrameUpdate();
    }

    public int GetCycle()
    {
        return waveStateMachine.CycleCount;
    }
    private void OnStateChangedCallback(WaveState newState)
    {
        OnStateChanged?.Invoke(newState);
    }
    
    public float CalculateSpawnStateDuration(int cycle)
    {

        return initialSpawnDuration+ (spawnDurationIncrement * cycle);
    }

    public float CalculatePeacefulStateDuration(int cycle)
    {

        return initialPeaceDuration + (peaceDurationIncrement * cycle);
    }

    public int CalcualteSpawnStateEnemiesCount(int cycle)
    {

        return initialEnemySpawnCount + (enemySpawnIncrement * cycle);
    }

    public void SpawnEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPoint = GetSpawnPoint();
            if(spawnPoint != Vector3.zero) 
            {
                Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            }
        }
    }

    Vector3 GetSpawnPoint()
    {
        for (int i = 0; i < 30; i++) 
        {
            Vector3 randomPoint = RandomPointWithinSphere();
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position; 
            }
        }
        return Vector3.zero; 
    }

    Vector3 RandomPointWithinSphere()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        float randomDistance = Random.Range(minRadius, maxRadius);
    
        return playerTransform.position + randomDirection.normalized * randomDistance;
    }

}
