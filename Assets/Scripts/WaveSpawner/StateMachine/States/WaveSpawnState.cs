using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnState : WaveState
{
    private float spawnTimer;
    private float callSpawnInterval = 10f;
    private int enemyCount;
    public WaveSpawnState(WaveSpawner _waveSpawner, WaveStateMachine _waveStateMachine) : base(_waveSpawner, _waveStateMachine)
    {
    }
    public override void EnterState(float duration)
    {
        base.EnterState(duration);
        enemyCount = waveSpawner.CalcualteSpawnStateEnemiesCount(waveStateMachine.CycleCount);
        Debug.Log("ENTER WAVE SPAWN STATE" + Duration);
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Timer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= callSpawnInterval && (Duration - Timer) >= 4f)
        {
            waveSpawner.SpawnEnemies(enemyCount);
            spawnTimer = 0;
        }

        if (Timer >= Duration)
        {
            Timer = 0;
            waveStateMachine.ChangeState(waveSpawner.PeacefulState, waveSpawner.CalculatePeacefulStateDuration);
        }


    }
}
