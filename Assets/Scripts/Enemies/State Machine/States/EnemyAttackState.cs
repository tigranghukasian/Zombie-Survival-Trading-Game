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
            if (enemy.PlayerInSight != null)
            {
                enemy.PlayerInSight.TakeDamage(enemy.AttackDamage, enemy);
            }
            
        }
        if (triggerType == Enemy.AnimationTriggerType.EnemyAttackFinished)
        {
            enemyStateMachine.ChangeState(enemy.ChaseState);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        var playerInSight = enemy.PlayerInSight;
        if (playerInSight != null)
        {
            enemy.transform.LookAt(new Vector3(playerInSight.transform.position.x, enemy.transform.position.y,
                playerInSight.transform.position.z));
        }
       
    }
    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.SetDestination(enemy.transform.position);
        enemy.Animator.SetBool("isRunning", false);
        enemy.Animator.SetTrigger("attack");
    }
}
