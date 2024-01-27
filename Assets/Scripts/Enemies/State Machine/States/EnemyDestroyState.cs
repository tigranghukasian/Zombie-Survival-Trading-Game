using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyState : EnemyState
{

    private bool animationPlaying;
    public EnemyDestroyState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.transform.LookAt(new Vector3(enemy.PlayerInSight.transform.position.x, enemy.transform.position.y,
            enemy.PlayerInSight.transform.position.z));
        if (enemy.Agent.enabled)
        {
            enemy.Agent.SetDestination(enemy.transform.position);
        }
       
    }

    public override void FrameUpdate()
    {

        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        if (enemy.Agent.enabled)
        {
            enemy.Agent.CalculatePath(enemy.PlayerInSight.transform.position, path);
            if (path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
            {
                enemyStateMachine.ChangeState(enemy.ChaseState);
                return;
            }
        }
        
        if (!animationPlaying)
        {
            enemy.Animator.SetBool("headButting", true);
            animationPlaying = true;
        }

        

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
            Debug.DrawRay(enemyPos, enemy.transform.forward * 1f, Color.red, 100f );
            if (Physics.Raycast(enemyPos, enemy.transform.forward * 1f, out hit))
            {
                
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    if (damageable is not ResourceObject)
                    {
                        damageable.TakeDamage(enemy.HeadButtDamage, enemy);
                    }
                    
                }
            }
        }
        if (triggerType == Enemy.AnimationTriggerType.EnemyHeadbuttFinished)
        {
            //animationPlaying = false;
        }
    }
}
