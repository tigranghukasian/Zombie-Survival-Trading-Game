using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventChecker : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        enemy.AnimationTriggerEvent(triggerType);
    }
}
