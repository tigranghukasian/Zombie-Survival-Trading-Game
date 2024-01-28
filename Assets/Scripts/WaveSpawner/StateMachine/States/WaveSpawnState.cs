using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnState : WaveState
{
    private float spawnTimer;
    private float callSpawnInterval = 5f;
    private int enemyCount;
    public WaveSpawnState(WaveSpawner _waveSpawner, WaveStateMachine _waveStateMachine) : base(_waveSpawner, _waveStateMachine)
    {
    }
    public override void EnterState(float duration)
    {
        base.EnterState(duration);
        enemyCount = waveSpawner.CalcualteSpawnStateEnemiesCount(waveStateMachine.CycleCount);
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Timer += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        if (Timer >= Duration)
        {
            waveStateMachine.ChangeState(waveSpawner.PeacefulState, waveSpawner.CalculatePeacefulStateDuration);
            Timer = 0;
        }

        if (spawnTimer >= callSpawnInterval)
        {
            waveSpawner.SpawnEnemies(enemyCount);
            spawnTimer = 0;
        }

    }
}
