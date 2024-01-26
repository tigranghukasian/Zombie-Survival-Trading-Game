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
        Debug.Log("ENTER BIRTH STATE");
        //enemy.EnemySoundPlayer.PlayRandomTypeSoundOneShot(EnemySoundPlayer.SoundType.Birth);
    }

    public override void ExitState()
    {
        base.ExitState();
        // enemy.Animator.transform.localPosition =
        //     new Vector3(enemy.Animator.transform.localPosition.x, 0, enemy.Animator.transform.localPosition.z);
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        if (triggerType == Enemy.AnimationTriggerType.EnemyBirthFinished)
        {
            enemyStateMachine.ChangeState(enemy.IdleState);
        }
    }
}
