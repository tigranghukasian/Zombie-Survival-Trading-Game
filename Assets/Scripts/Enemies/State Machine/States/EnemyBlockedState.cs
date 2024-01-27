using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockedState : EnemyState
{

    private bool animationPlaying;
    public EnemyBlockedState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void FrameUpdate()
    {
        
       
        if (enemy.PlayerInSight != null)
        {
            enemy.Agent.SetDestination(enemy.PlayerInSight.transform.position);
        }
        else
        {
            enemyStateMachine.ChangeState(enemy.IdleState);
        }

        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        enemy.Agent.CalculatePath(enemy.PlayerInSight.transform.position, path);
        if (path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            enemyStateMachine.ChangeState(enemy.ChaseState);
            return;
        }
        if (!animationPlaying)
        {
            enemy.Animator.SetBool("headButting", true);
            animationPlaying = true;
        }

        enemy.transform.LookAt(new Vector3(enemy.PlayerInSight.transform.position.x, enemy.transform.position.y,
            enemy.PlayerInSight.transform.position.z));

    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.Animator.SetBool("headButting", false);
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        if (triggerType == Enemy.AnimationTriggerType.EnemyHeadbutt)
        {
            RaycastHit hit;
            var enemyPos = enemy.transform.position + Vector3.up;
            var playerPos = enemy.PlayerInSight.transform.position;
            Debug.DrawRay(enemyPos, playerPos- enemyPos, Color.red, 100f );
            if (Physics.Raycast(enemyPos, playerPos- enemyPos, out hit))
            {
                Debug.Log(hit.collider.name);
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(enemy.HeadButtDamage, enemy);
                }
            }
        }
        if (triggerType == Enemy.AnimationTriggerType.EnemyHeadbuttFinished)
        {
            //animationPlaying = false;
        }
    }
}
