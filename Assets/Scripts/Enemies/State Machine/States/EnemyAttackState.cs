using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
        
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        if (triggerType == Enemy.AnimationTriggerType.EnemyAttacked && enemy.IsInAttackRange)
        {
            Debug.Log("DAMAGE PLAYER");
            //enemy.PlayerInSight
        }
    }
    public override void EnterState()
    {
        base.EnterState();
        enemy.PlayAttackAnimation();
    }
}
