using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyAttackingState : _EnemyBaseState
    {
        #region Constant Fields

        private const string AttackingAnimation = "Attacking";

        private const float DistanceForMovementState = 2.5f;

        private const float AttackDelay = 0.35f;

        #endregion

        #region Fields

        private Coroutine _attackCoroutine;

        #endregion

        #region Public Methods

        public override void EnterState(_EnemyStateManager enemy)
        {
            _attackCoroutine = enemy.StartCoroutine(AttackCoroutine(enemy));
        }

        public override void ExitState(_EnemyStateManager enemy)
        {
            if (_attackCoroutine != null)
            {
                enemy.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }

        public override void FixedUpdateState(_EnemyStateManager enemy)
        {
            var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;

            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, DistanceForMovementState, enemy.PlayerLayerMask);

            if (hit.collider == null)
            {
                enemy.SwitchState(enemy.AliveState.MovementState);
            }
        }

        public override void UpdateState(_EnemyStateManager enemy)
        {

        }

        #endregion

        #region Private Methods

        private IEnumerator AttackCoroutine(_EnemyStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(AttackingAnimation);
            yield return new WaitForSeconds(AttackDelay);
            for (; ; )
            {
                if (enemy.AnimatorManager.DidAnimationFinish(AttackingAnimation))
                {
                    enemy.AnimatorManager.ReplayAnimation();
                }

                yield return new WaitForSeconds(AttackDelay);
            }
        }

        #endregion
    }
}
