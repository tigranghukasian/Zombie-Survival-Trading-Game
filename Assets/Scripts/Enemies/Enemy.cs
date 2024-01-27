using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : Damageable, IDamager
{
    #region Animation Triggers

    public enum AnimationTriggerType
    {
        EnemyAttackFinished,
        EnemyAttacked,
        EnemyBirthFinished,
        EnemyHeadbuttFinished,
        EnemyHeadbutt
    }
    
    #endregion
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyBirthState BirthState { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyDeadState DeadState { get; set; }
    public EnemyBlockedState BlockedState { get; set; }

    [field: SerializeField] public float AttackDamage { get; set; } = 15f;
    [field: SerializeField] public float HeadButtDamage { get; set; } = 15f;
    [field: SerializeField] public float IdleMovementRange { get; set; } = 5f;
    [field: SerializeField] public float IdleMovementSpeed { get; set; } = 1f;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerMask;
    [field: SerializeField] public RagdollEnabler RagdollEnabler { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public EnemySoundPlayer EnemySoundPlayer { get; set; }

    private Collider[] hitColliders = new Collider[10];

    
    public bool IsInSightRange { get; private set; }
    public bool IsInAttackRange { get; private set; }
    public Player PlayerInSight { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = new EnemyStateMachine();

        BirthState = new EnemyBirthState(this, StateMachine);
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        DeadState = new EnemyDeadState(this, StateMachine);
        BlockedState = new EnemyBlockedState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(BirthState);
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
    
    public override void TakeDamage(float amount, IDamager damager)
    {
        base.TakeDamage(amount, damager);
        EnemySoundPlayer.PlayRandomTypeSoundOneShot(EnemySoundPlayer.SoundType.Damaged);
    }
     public override void Kill()
     {
         base.Kill();
         StateMachine.ChangeState(DeadState);
     }

     public void Destroy()
     {
         Destroy(gameObject);
     }

     public void AnimationTriggerEvent(AnimationTriggerType triggerType)
     {
         StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
     }

     void OnDrawGizmos()
     {
         if (Application.isPlaying)
         {
             // Set the text position slightly above the GameObject
             Vector3 textPosition = transform.position + Vector3.up * 2;

             // Convert the world position to a screen position
             Vector3 screenPosition = Camera.current.WorldToScreenPoint(textPosition);

             // Use GUI to draw the label
             // Note: GUI calls need to be inside OnGUI method, so we use Handles
             UnityEditor.Handles.BeginGUI();

             var stateName = StateMachine.CurrentEnemyState.ToString();

             // Pass stateName to GUIContent to calculate the correct size
             var textSize = GUI.skin.label.CalcSize(new GUIContent(stateName));

             var rect = new Rect(screenPosition.x - textSize.x / 2, 
                 Screen.height - screenPosition.y - textSize.y / 2, 
                 textSize.x, textSize.y);
             GUI.Label(rect, stateName);

             UnityEditor.Handles.EndGUI();
         }
     }

}
