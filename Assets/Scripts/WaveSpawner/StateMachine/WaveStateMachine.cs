using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStateMachine 
{
    public WaveState CurrentWaveState { get; set; }
    public int CycleCount { get; set; }
    
    public delegate float DurationCalculationStrategy(int cycleCount);

    private float defaultDuration = 10f;
    
    private event Action<WaveState> onStateChangedCallback;

    public WaveStateMachine(Action<WaveState> _onStateChangedCallback)
    {
        onStateChangedCallback = _onStateChangedCallback;
    }
    public void Initialize(WaveState startingState, float startingStateDuration)
    {
        CurrentWaveState = startingState;
        CurrentWaveState.EnterState(startingStateDuration);
        onStateChangedCallback?.Invoke(CurrentWaveState);
    }

    public void ChangeState(WaveState newState, DurationCalculationStrategy calculationStrategy)
    {
        WaveState previousState = CurrentWaveState;
        CurrentWaveState.ExitState();
        IncrementCycleCountIfNeeded(previousState, newState);

        onStateChangedCallback?.Invoke(newState);

        CurrentWaveState = newState;
        float newDuration = calculationStrategy?.Invoke(CycleCount) ?? defaultDuration;
        CurrentWaveState.EnterState(newDuration);

    }
    
    private void IncrementCycleCountIfNeeded(WaveState previousState, WaveState newState)
    {
        if (previousState is WaveSpawnState && newState is WavePeacefulState)
        {
            CycleCount++;
        }
    }
}
