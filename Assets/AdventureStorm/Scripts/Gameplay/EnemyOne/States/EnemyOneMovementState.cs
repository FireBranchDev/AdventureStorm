using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneMovementState : BaseState
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

        public override void EnterState(StateManager stateManager)
        {
            _idleCountDownTimer = null;
            _isIdle = false;

            stateManager.AnimatorManager.ChangeAnimationState(WalkingAnimation);
        }

        public override void ExitState(StateManager stateManager)
        {
            if (_idleCountDownTimer != null)
            {
                stateManager.StopCoroutine(_idleCountDownTimer);
                _idleCountDownTimer = null;
            }
        }

        public override void FixedUpdateState(StateManager stateManager)
        {
            if (stateManager.TryGetComponent<EnemyOneStateManager>(out var enemyOneStateManager))
            {
                Combat(enemyOneStateManager);
                Idle(enemyOneStateManager);
                Movement(enemyOneStateManager);
            }
        }

        public override void UpdateState(StateManager stateManager)
        {
            if (stateManager.TryGetComponent<EnemyOneStateManager>(out var enemyOneStateManager))
            {
                if (_isIdle)
                {
                    stateManager.SwitchState(enemyOneStateManager.AliveState.IdleState);
                }

                if (stateManager.AnimatorManager.DidAnimationFinish(WalkingAnimation))
                {
                    stateManager.AnimatorManager.ReplayAnimation();
                }
            }
        }

        #endregion

        #region Private Methods

        private void Combat(EnemyOneStateManager enemy)
        {
            var direction = enemy.IsFacingLeft ? Vector2.left : Vector2.right;
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

        private void Idle(EnemyOneStateManager enemy)
        {
            var direction = enemy.IsFacingLeft ? Vector2.left : Vector2.right;
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

        private void Movement(EnemyOneStateManager enemy)
        {
            Vector2 velocity = new(0, 0);

            if (enemy.IsFacingLeft)
            {
                velocity.x = -MovementSpeed;
            }
            else
            {
                velocity.x = MovementSpeed;
            }

            enemy.Rb2D.MovePosition(enemy.Rb2D.position + velocity * Time.fixedDeltaTime);
        }

        #endregion
    }
}
