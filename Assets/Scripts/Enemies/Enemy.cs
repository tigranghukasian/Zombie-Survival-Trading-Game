using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : Damageable
{
    #region Animation Triggers

    public enum AnimationTriggerType
    {
        EnemyAttackFinished,
        EnemyAttacked
    }
    
    #endregion
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    [field: SerializeField] public float IdleMovementRange { get; set; } = 5f;
    [field: SerializeField] public float IdleMovementSpeed { get; set; } = 1f;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerMask;
    [field: SerializeField] public Animator Animator { get; set; }

    private Collider[] hitColliders = new Collider[10];

    
    public bool IsInSightRange { get; private set; }
    public bool IsInAttackRange { get; private set; }
    public Player PlayerInSight { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CheckSightAndAttackRanges();
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void CheckSightAndAttackRanges()
    {
        int colliderNumber = Physics.OverlapSphereNonAlloc(transform.position, sightRange, hitColliders, playerMask);
        IsInSightRange = CheckForPlayer(hitColliders, colliderNumber);

        if (IsInSightRange)
        {
            colliderNumber = Physics.OverlapSphereNonAlloc(transform.position, attackRange, hitColliders, playerMask);
            IsInAttackRange = CheckForPlayer(hitColliders, colliderNumber);
        }
    }

    public void PlayAttackAnimation()
    {
        
    }
    private bool CheckForPlayer(Collider[] colliders, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (colliders[i] != null && colliders[i].TryGetComponent(out Player player))
            {
                PlayerInSight = player;
                return true;
            }
        }
        return false;
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    public override void TakeDamage(float amount, Player player)
     {
         base.TakeDamage(amount, player);
     }
     public override void Kill()
     {
         base.Kill();
         Destroy(gameObject);
     }

     public void AnimationTriggerEvent(AnimationTriggerType triggerType)
     {
         StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
     }


}
