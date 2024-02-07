using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    private float destroyAfter = 8f;
    private float destroyTimer;
    public EnemyDeadState(Enemy _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.GetComponent<Collider>().enabled = false;
        enemy.RagdollEnabler.EnableRagdoll();
        enemy.Agent.enabled = false;
        enemy.EnemySoundPlayer.PlayRandomTypeSoundOneShot(EnemySoundPlayer.SoundType.Death);
        enemy.EnemySoundPlayer.StopPlayingContinuousSounds();
    }
    

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        destroyTimer += Time.deltaTime;
        if (destroyTimer > destroyAfter)
        {
            enemy.Destroy();
            destroyTimer = 0;
        }
    }
}
