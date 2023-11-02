using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyCombatState : _EnemyBaseState
    {
        #region Constant Fields

        private const float CombatDistance = 2.5f;

        private const float DodgeChance = 0.35f;

        #endregion

        #region Fields

        private _PlayerStateManager _playerStateManager;

        #endregion

        #region Constructors

        public _EnemyCombatState()
        {
            AttackingState = new _EnemyAttackingState();
            DodgingState = new _EnemyDodgingState();

            _playerStateManager = null;
        }

        #endregion

        #region Properties

        public _EnemyAttackingState AttackingState { get; private set; }

        public _EnemyDodgingState DodgingState { get; private set; }

        #endregion

        #region Public Methods

        public override void EnterState(_EnemyStateManager enemy)
        {

        }

        public override void ExitState(_EnemyStateManager enemy)
        {

        }

        public override void FixedUpdateState(_EnemyStateManager enemy)
        {
            if (_playerStateManager == null)
            {
                var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, CombatDistance, enemy.PlayerLayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent<_PlayerStateManager>(out var playerStateManager))
                    {
                        _playerStateManager = playerStateManager;
                    }
                }
            }
        }

        public override void UpdateState(_EnemyStateManager enemy)
        {
            if (_playerStateManager != null)
            {
                if (_playerStateManager.IsAlive)
                {
                    float distance = Mathf.Abs(enemy.transform.position.x - _playerStateManager.transform.position.x);
                    if (distance <= CombatDistance)
                    {
                        float random = Random.Range(1f, 100f);
                        if (random / 100f <= DodgeChance)
                        {
                            enemy.SwitchState(enemy.AliveState.CombatState.DodgingState);
                        }
                        else
                        {
                            enemy.SwitchState(enemy.AliveState.CombatState.AttackingState);
                        }
                    }
                    else
                    {
                        enemy.SwitchState(enemy.AliveState.MovementState);
                    }
                }
                else
                {
                    enemy.SwitchState(enemy.AliveState.IdleState);
                }
            }
        }

        #endregion
    }
}