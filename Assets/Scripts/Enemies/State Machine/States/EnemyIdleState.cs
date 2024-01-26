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
        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemy.IsInSightRange)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
        if (!walkPointSet)
        {
            SetEnemyWalkPoint();
        }
        Vector3 distanceToRandomPoint = enemy.transform.position - walkPoint;
        if (distanceToRandomPoint.sqrMagnitude < 1)
        {
            walkPointSet = false;
        }
    }

    private void SetEnemyWalkPoint()
    {
        walkPoint = GetRandomWalkPoint();
        enemy.Agent.SetDestination(walkPoint);
        walkPointSet = true;
    }

    private Vector3 GetRandomWalkPoint()
    {
        Vector3 randomPosition = (Vector3)Random.insideUnitCircle * enemy.IdleMovementRange;
        return new Vector3(randomPosition.x, enemy.transform.position.y, randomPosition.z);
    }
}
