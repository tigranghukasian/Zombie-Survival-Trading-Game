using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 targetPos;
    private bool walkPointSet;
    private Vector3 walkPoint;
    public EnemyIdleState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
        
    }

    public override void EnterState()
    {
        enemy.Animator.Play("Idle", 0);
        enemy.Animator.SetBool("isRunning", false);
        enemy.Agent.enabled = false;
        enemy.EnemySoundPlayer.StartPlayingContinuousRandomSounds(EnemySoundPlayer.SoundType.Idle, 1f, 5f);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemy.IsInSightRange)
        {
            enemyStateMachine.ChangeState(enemy.ChaseState);
        }
    }
    //
    // private void SetEnemyWalkPoint()
    // {
    //     walkPoint = GetRandomWalkPoint();
    //     if (enemy.Agent.enabled)
    //     {
    //         enemy.Agent.SetDestination(walkPoint);
    //     }
    //   
    //     walkPointSet = true;
    // }

    private Vector3 GetRandomWalkPoint()
    {
        Vector3 randomPosition = (Vector3)Random.insideUnitCircle * enemy.IdleMovementRange;
        return new Vector3(randomPosition.x, enemy.transform.position.y, randomPosition.z);
    }
}
