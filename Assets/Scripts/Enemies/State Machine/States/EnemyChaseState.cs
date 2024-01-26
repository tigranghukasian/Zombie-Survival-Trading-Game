using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.SetDestination(enemy.transform.position);
        enemy.Animator.SetBool("isRunning", true);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemy.PlayerInSight != null)
        {
            enemy.Agent.SetDestination(enemy.PlayerInSight.transform.position);
        }
        if (enemy.IsInAttackRange)
        {
            enemyStateMachine.ChangeState(enemy.AttackState);
        }
    }
}
