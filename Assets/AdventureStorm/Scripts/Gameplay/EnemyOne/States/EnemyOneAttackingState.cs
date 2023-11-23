using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneAttackingState : BaseState
    {
        #region Constant Fields

        private const string AttackingAnimation = "Attacking";

        private const float AttackRange = 2.5f;

        private const float AttackDelay = 0.35f;

        private const float AttackDamage = 1.25f;

        #endregion

        #region Fields

        private Coroutine _attackCoroutine;

        private GameObject _player;

        private PlayerStateManager _playerStateManager;

        #endregion

        #region Constructors

        public EnemyOneAttackingState()
        {
            _attackCoroutine = null;
        }

        #endregion

        #region Public Methods

        public override void EnterState(StateManager stateManager)
        {
            if (stateManager.TryGetComponent<EnemyOneStateManager>(out var enemyOneStateManager))
            {
                if (_attackCoroutine == null)
                {
                    _attackCoroutine = stateManager.StartCoroutine(AttackCoroutine(enemyOneStateManager));
                }
            }
        }

        public override void ExitState(StateManager stateManager)
        {
            if (_attackCoroutine != null)
            {
                stateManager.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }

        public override void FixedUpdateState(StateManager stateManager)
        {
            var direction = stateManager.IsFacingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(stateManager.transform.position, direction, AttackRange, stateManager.PlayerLayerMask);

            if (_player == null)
            {
                if (hit.collider != null)
                {
                    _player = hit.collider.gameObject;
                }
            }
        }

        public override void UpdateState(StateManager stateManager)
        {
            if (_player != null && _playerStateManager == null)
            {
                if (_player.TryGetComponent<PlayerStateManager>(out var playerStateManager))
                {
                    _playerStateManager = playerStateManager;
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator AttackCoroutine(EnemyOneStateManager stateManager)
        {
            if (stateManager.AnimatorManager.DidAnimationFinish(AttackingAnimation))
            {
                stateManager.AnimatorManager.ReplayAnimation();
            }
            else
            {
                stateManager.AnimatorManager.ChangeAnimationState(AttackingAnimation);
            }

            while (!stateManager.AnimatorManager.DidAnimationFinish(AttackingAnimation))
            {
                yield return null;
            }

            if (_player != null)
            {
                float distance = Mathf.Abs(stateManager.transform.position.x - _player.transform.position.x);

                if (distance <= AttackRange)
                {
                    if (_playerStateManager != null)
                    {
                        _playerStateManager.Damage(AttackDamage);

                        if (!_playerStateManager.IsAlive)
                        {
                            _playerStateManager.SwitchState(_playerStateManager.DeathState);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(AttackDelay);

            stateManager.SwitchState(stateManager.AliveState.CombatState);
        }

        #endregion
    }
}
