using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyIdleState : _EnemyBaseState
    {
        #region Constant Fields

        private const string IdleAnimation = "Idle";

        private const float DistanceForMovementState = 5f;

        #endregion

        #region Public Methods

        public override void EnterState(_EnemyStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(IdleAnimation);
        }

        public override void ExitState(_EnemyStateManager enemy)
        {

        }

        public override void FixedUpdateState(_EnemyStateManager enemy)
        {
            var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, DistanceForMovementState, enemy.PlayerLayerMask);
            Debug.DrawRay(enemy.transform.position, direction * DistanceForMovementState, Color.blue);

            if (hit.collider != null)
            {
                enemy.SwitchState(enemy.AliveState.MovementState);
            }
        }

        public override void UpdateState(_EnemyStateManager enemy)
        {
            if (enemy.AnimatorManager.DidAnimationFinish(IdleAnimation))
            {
                enemy.AnimatorManager.ReplayAnimation();
            }
        }

        #endregion
    }
}
