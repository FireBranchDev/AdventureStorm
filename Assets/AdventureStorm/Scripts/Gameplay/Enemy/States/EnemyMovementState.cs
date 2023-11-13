using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyMovementState : EnemyBaseState
    {
        #region Constant Fields

        private const string WalkingAnimation = "Walking";

        private const float MovementSpeed = 3f;

        private const float DistanceForIdleState = 5f;

        private const float DistanceForCombatState = 2.5f;

        private const float SecondsRequiredForIdleState = 0.33f;

        #endregion

        #region Fields

        private Coroutine _idleCountDownTimer;

        private bool _isIdle;

        #endregion

        #region Public Methods

        public override void EnterState(EnemyStateManager enemy)
        {
            _idleCountDownTimer = null;
            _isIdle = false;

            enemy.AnimatorManager.ChangeAnimationState(WalkingAnimation);
        }

        public override void ExitState(EnemyStateManager enemy)
        {
            if (_idleCountDownTimer != null)
            {
                enemy.StopCoroutine(_idleCountDownTimer);
                _idleCountDownTimer = null;
            }
        }

        public override void FixedUpdateState(EnemyStateManager enemy)
        {
            Combat(enemy);
            Idle(enemy);
            Movement(enemy);
        }

        public override void UpdateState(EnemyStateManager enemy)
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

        private void Combat(EnemyStateManager enemy)
        {
            var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, DistanceForCombatState, enemy.PlayerLayerMask);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<PlayerStateManager>(out var player))
                {
                    if (player.IsAlive)
                    {
                        enemy.SwitchState(enemy.AliveState.CombatState);
                    }
                }
            }
        }

        private void Idle(EnemyStateManager enemy)
        {
            var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, DistanceForIdleState, enemy.PlayerLayerMask);

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

        private void Movement(EnemyStateManager enemy)
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