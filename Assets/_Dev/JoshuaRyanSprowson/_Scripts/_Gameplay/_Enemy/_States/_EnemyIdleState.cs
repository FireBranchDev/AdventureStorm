using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyIdleState : _EnemyBaseState
    {
        private const string IdleAnimation = "Idle";
        private const string IdleBlinkingAnimation = "Idle Blinking";

        public override void EnterState(_EnemyStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(IdleAnimation);
            enemy.AnimatorManager.ChangeAnimationState(IdleBlinkingAnimation);
        }

        public override void OnTriggerEnter2D(_EnemyStateManager enemy, Collider2D collision)
        {
            Debug.Log($"Trigger has been entered. {collision.name}");
        }

        public override void UpdateState(_EnemyStateManager enemy)
        {
            
        }
    }
}
