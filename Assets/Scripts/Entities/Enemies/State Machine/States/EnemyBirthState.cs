using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBirthState : EnemyState
{
    public EnemyBirthState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        enemy.GetComponent<Collider>().enabled = true;
        enemy.RagdollEnabler.EnableAnimator();
        enemy.Agent.enabled = true;
        enemy.Agent.ResetPath();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        if (triggerType == Enemy.AnimationTriggerType.EnemyBirthFinished)
        {
            enemyStateMachine.ChangeState(enemy.IdleState);
        }
    }
}
