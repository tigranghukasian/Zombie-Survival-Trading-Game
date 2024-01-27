using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    
    private float exitTimer ;
    private float timeTillExit = 5f;
    private float distanceToCountExit;
    private int DestructibleObjectCheckRate = 10;
    private float CheckDistance = 1.5f;
    private NavMeshPath OriginalPath;

    private float destructibleCheckTimer = 0f;
    private bool isCheckingForDestructibles = false;
    private Vector3[] corners = new Vector3[2];

    public EnemyChaseState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
        distanceToCountExit = _enemy.IdleMovementRange;
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.enabled = true;
        enemy.Agent.SetDestination(enemy.transform.position);
        enemy.Animator.SetBool("isRunning", true);
        enemy.EnemySoundPlayer.StartPlayingContinuousRandomSounds(EnemySoundPlayer.SoundType.Chase, 3f, 7f);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemy.PlayerInSight != null && enemy.Agent.enabled)
        {
            enemy.Agent.SetDestination(enemy.PlayerInSight.transform.position);
        }
        if (enemy.IsInAttackRange)
        {
            enemyStateMachine.ChangeState(enemy.AttackState);
        }
        
        // UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        // enemy.Agent.CalculatePath(enemy.PlayerInSight.transform.position, path);
        // if (path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete) {
        //     Debug.Log("PATH BLOCKED");
        //     enemyStateMachine.ChangeState(enemy.DestroyState);
        // }

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
        
        if (enemy.Agent.enabled)
        {
            destructibleCheckTimer += Time.deltaTime;
            
            if (destructibleCheckTimer >= 1f / DestructibleObjectCheckRate)
            {
                destructibleCheckTimer = 0f;
                CheckForDestructibleObjects(); 
            }
        }

    }
    
    private void CheckForDestructibleObjects()
    {
        int length = enemy.Agent.path.GetCornersNonAlloc(corners);
        Debug.DrawRay(corners[0], (corners[1] - corners[0]).normalized, Color.red);
        if (length > 1 &&
            Physics.Raycast(
                corners[0],
                (corners[1] - corners[0]).normalized,
                out RaycastHit hit,
                CheckDistance,
                enemy.DestructibleLayer) &&
            hit.collider.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("CHANGE STATE TO DESTROY");
            damageable.OnDestroyed += (_d) => HandleDestroy();
            enemy.Agent.enabled = false;
            enemyStateMachine.ChangeState(enemy.DestroyState);
            isCheckingForDestructibles = false;
        }
    }
    
    private void HandleDestroy()
    {
        enemyStateMachine.ChangeState(enemy.ChaseState);
    }

    private void CheckForExit()
    {
        
    }
}
