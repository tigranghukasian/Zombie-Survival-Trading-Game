using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    
    private float exitTimer ;
    private float timeTillExit = 5f;
    private float distanceToCountExit;
    public EnemyChaseState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
        distanceToCountExit = _enemy.IdleMovementRange;
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.SetDestination(enemy.transform.position);
        enemy.Animator.SetBool("isRunning", true);
        enemy.EnemySoundPlayer.StartPlayingContinuousRandomSounds(EnemySoundPlayer.SoundType.Chase, 3f, 7f);
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
        
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        enemy.Agent.CalculatePath(enemy.PlayerInSight.transform.position, path);
        if (path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete) {
            Debug.Log("PATH BLOCKED");
            enemyStateMachine.ChangeState(enemy.BlockedState);
        }

        CheckForExit();
        if (Vector3.Distance(enemy.PlayerInSight.transform.position, enemy.transform.position) > distanceToCountExit)
        {
            exitTimer += Time.deltaTime;

            if (exitTimer > timeTillExit)
            {
                exitTimer = 0;
                enemyStateMachine.ChangeState(enemy.IdleState);
            }
        }
        else
        {
            exitTimer = 0;
        }

    }

    private void CheckForExit()
    {
        
    }
}
