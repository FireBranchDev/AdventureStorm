using AdventureStorm.Gameplay.Enemy.States;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneIdleState : EnemyBaseState<EnemyOneStateManager>
    {
        #region Constant Fields

        private const string IdleAnimation = "Idle";

        private const float DistanceForMovementState = 5f;

        #endregion

        #region Fields

        private GameObject _player;

        private PlayerStateManager _playerStateManager;

        #endregion

        #region Public Methods

        public override void EnterState(EnemyOneStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(IdleAnimation);
        }

        public override void ExitState(EnemyOneStateManager enemy)
        {

        }

        public override void FixedUpdateState(EnemyOneStateManager enemy)
        {
            if (_player == null)
            {
                var direction = enemy.IsFacingLeft ? Vector2.left : Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, DistanceForMovementState, enemy.PlayerLayerMask);

                if (hit.collider != null)
                {
                    _player = hit.collider.gameObject;
                }
            }
        }

        public override void UpdateState(EnemyOneStateManager enemy)
        {
            if (enemy.AnimatorManager.DidAnimationFinish(IdleAnimation))
            {
                enemy.AnimatorManager.ReplayAnimation();
            }

            if (_player != null && _playerStateManager == null)
            {
                if (_player.TryGetComponent<PlayerStateManager>(out var playerStateManager))
                {
                    _playerStateManager = playerStateManager;
                }
            }

            if (_playerStateManager != null)
            {
                if (_playerStateManager.IsAlive)
                {
                    enemy.SwitchState(enemy.AliveState.MovementState);
                }
            }
        }

        #endregion
    }
}
