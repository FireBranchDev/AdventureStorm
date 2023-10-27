using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyMovementState : _EnemyBaseState
    {
        #region Constant Fields

        private const string WalkingAnimation = "Walking";

        private const float MovementSpeed = 3f;

        private const float DistanceForIdleState = 5f;

        private const float DistanceForAttackState = 2.5f;

        private const float SecondsRequiredForIdleState = 0.33f;

        #endregion

        #region Fields

        private Coroutine _idleCountDownTimer;

        private bool _isIdle;

        #endregion

        #region Public Methods

        public override void EnterState(_EnemyStateManager enemy)
        {
            _idleCountDownTimer = null;
            _isIdle = false;

            enemy.AnimatorManager.ChangeAnimationState(WalkingAnimation);
        }

        public override void ExitState(_EnemyStateManager enemy)
        {
            if (_idleCountDownTimer != null)
            {
                enemy.StopCoroutine(_idleCountDownTimer);
                _idleCountDownTimer = null;
            }
        }

        public override void FixedUpdateState(_EnemyStateManager enemy)
        {
            Attack(enemy);
            Idle(enemy);
            Movement(enemy);
        }

        public override void UpdateState(_EnemyStateManager enemy)
        {
            if (_isIdle)
            {
                enemy.SwitchState(enemy.AliveState.IdleState);
            }

            if (enemy.AnimatorManager.DidAnimationFinish(WalkingAnimation))
            {
                enemy.AnimatorManager.ReplayAnimation();
            }
        }

        #endregion

        #region Private Methods

        private void Attack(_EnemyStateManager enemy)
        {
            var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, DistanceForAttackState, enemy.PlayerLayerMask);
            Debug.DrawRay(enemy.transform.position, direction * DistanceForAttackState, Color.cyan);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<_PlayerStateManager>(out var player))
                {
                    if (player.IsAlive)
                    {
                        enemy.SwitchState(enemy.AliveState.AttackingState);
                    }
                }
            }
        }

        private void Idle(_EnemyStateManager enemy)
        {
            var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, DistanceForIdleState, enemy.PlayerLayerMask);
            Debug.DrawRay(enemy.transform.position, direction * DistanceForIdleState, Color.yellow);

            if (hit.collider != null)
            {
                if (_idleCountDownTimer != null)
                {
                    enemy.StopCoroutine(_idleCountDownTimer);
                    _idleCountDownTimer = null;
                }
            }
            else
            {
                _idleCountDownTimer ??= enemy.StartCoroutine(IdleCountDownTimer());
            }
        }

        private IEnumerator IdleCountDownTimer()
        {
            _isIdle = false;

            yield return new WaitForSeconds(SecondsRequiredForIdleState);

            _isIdle = true;
        }

        private void Movement(_EnemyStateManager enemy)
        {
            Vector2 velocity = new(0, 0);

            if (enemy.AliveState.IsFacingLeft)
            {
                velocity.x = -MovementSpeed;
            }
            else
            {
                velocity.x = MovementSpeed;
            }

            enemy.RB2D.MovePosition(enemy.RB2D.position + velocity * Time.fixedDeltaTime);
        }

        #endregion
    }
}
