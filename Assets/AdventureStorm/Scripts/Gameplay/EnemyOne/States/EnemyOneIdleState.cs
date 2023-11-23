using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneIdleState : BaseState
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

        public override void EnterState(StateManager stateManager)
        {
            stateManager.AnimatorManager.ChangeAnimationState(IdleAnimation);
        }

        public override void ExitState(StateManager stateManager)
        {

        }

        public override void FixedUpdateState(StateManager stateManager)
        {
            if (_player == null)
            {
                var direction = stateManager.IsFacingLeft ? Vector2.left : Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(stateManager.transform.position, direction, DistanceForMovementState, stateManager.PlayerLayerMask);

                if (hit.collider != null)
                {
                    _player = hit.collider.gameObject;
                }
            }
        }

        public override void UpdateState(StateManager stateManager)
        {
            if (stateManager.TryGetComponent<EnemyOneStateManager>(out var enemyOneStateManager))
            {
                if (stateManager.AnimatorManager.DidAnimationFinish(IdleAnimation))
                {
                    stateManager.AnimatorManager.ReplayAnimation();
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
                        stateManager.SwitchState(enemyOneStateManager.AliveState.MovementState);
                    }
                }
            }
        }

        #endregion
    }
}
