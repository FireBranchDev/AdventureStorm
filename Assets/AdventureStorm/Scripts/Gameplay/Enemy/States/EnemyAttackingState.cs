using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyAttackingState : EnemyBaseState
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

        public EnemyAttackingState()
        {
            _attackCoroutine = null;
        }

        #endregion

        #region Public Methods

        public override void EnterState(EnemyStateManager enemy)
        {
            if (_attackCoroutine == null)
            {
                _attackCoroutine = enemy.StartCoroutine(AttackCoroutine(enemy));
            }
        }

        public override void ExitState(EnemyStateManager enemy)
        {
            if (_attackCoroutine != null)
            {
                enemy.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }

        public override void FixedUpdateState(EnemyStateManager enemy)
        {
            var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, AttackRange, enemy.PlayerLayerMask);

            if (_player == null)
            {
                if (hit.collider != null)
                {
                    _player = hit.collider.gameObject;
                }
            }
        }

        public override void UpdateState(EnemyStateManager enemy)
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

        private IEnumerator AttackCoroutine(EnemyStateManager enemy)
        {
            if (enemy.AnimatorManager.DidAnimationFinish(AttackingAnimation))
            {
                enemy.AnimatorManager.ReplayAnimation();
            }
            else
            {
                enemy.AnimatorManager.ChangeAnimationState(AttackingAnimation);
            }

            while (!enemy.AnimatorManager.DidAnimationFinish(AttackingAnimation))
            {
                yield return null;
            }

            if (_player != null)
            {
                float distance = Mathf.Abs(enemy.transform.position.x - _player.transform.position.x);

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

            enemy.SwitchState(enemy.AliveState.CombatState);
        }

        #endregion
    }
}
