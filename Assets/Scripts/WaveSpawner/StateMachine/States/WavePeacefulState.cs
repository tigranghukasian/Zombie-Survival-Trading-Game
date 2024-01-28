using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePeacefulState : WaveState
{
    public WavePeacefulState(WaveSpawner _waveSpawner, WaveStateMachine _waveStateMachine) : base(_waveSpawner, _waveStateMachine)
    {
    }

    public override void EnterState(float duration)
    {
        base.EnterState(duration);
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Timer += Time.deltaTime;
        if (Timer >= Duration)
        {
            waveStateMachine.ChangeState(waveSpawner.SpawnState, waveSpawner.CalculateSpawnStateDuration);
            Timer = 0;
        }
    }
}
