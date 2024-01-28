using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveState
{
    protected WaveSpawner waveSpawner;
    protected WaveStateMachine waveStateMachine;
    public float Duration { get; protected set; }
    public float Timer { get; protected set; }


    public WaveState(WaveSpawner _waveSpawner, WaveStateMachine _waveStateMachine)
    {
        waveSpawner = _waveSpawner;
        waveStateMachine = _waveStateMachine;
    }

    public virtual void EnterState(float _duration)
    {
        Duration = _duration;
    }

    public virtual void ExitState()
    {
        
    }

    public virtual void FrameUpdate()
    {
        
    }
}
