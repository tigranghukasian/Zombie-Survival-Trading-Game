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
            RaycastHit hit;
            var enemyPos = enemy.transform.position + Vector3.up;
            var playerPos = enemy.PlayerInSight.transform.position;
            Debug.DrawRay(enemyPos, enemy.transform.forward * 1f, Color.red, 100f );
            if (Physics.Raycast(enemyPos, enemy.transform.forward * 1f, out hit))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    if (damageable is not ResourceObject)
                    {
                        damageable.TakeDamage(enemy.AttackDamage, enemy);
                    }
                }
                
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
        if (playerInSight != null && enemy.Agent.enabled)
        {
            if (enemy.Agent.enabled)
            {
                enemy.Agent.SetDestination(enemy.PlayerInSight.transform.position);
            }
            enemy.transform.LookAt(new Vector3(playerInSight.transform.position.x, enemy.transform.position.y,
                playerInSight.transform.position.z));
           
        }
       
    }
    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.enabled = true;
        enemy.Agent.speed = enemy.AttackChargeSpeed;
        enemy.Agent.SetDestination(enemy.PlayerInSight.transform.position);
        enemy.Animator.SetTrigger("attack");
        enemy.EnemySoundPlayer.PlayRandomTypeSoundOneShot(EnemySoundPlayer.SoundType.Attack);
    }
}
