using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerAttackingState : _PlayerBaseState
    {
        #region Constant Fields

        private const string AttackingAnimation = "Attacking";

        private const float AttackRange = 2.1f;

        private const float AttackDamage = 2.5f;

        #endregion

        #region Fields

        private bool _enemyDetectionFinished;

        #endregion

        #region Constructors

        public _PlayerAttackingState()
        {
            _enemyDetectionFinished = false;
        }

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            _enemyDetectionFinished = false;
            player.AnimatorManager.ChangeAnimationState(AttackingAnimation);
        }

        public override void ExitState(_PlayerStateManager player)
        {

        }

        public override void FixedUpdateState(_PlayerStateManager player)
        {
            if (player.AnimatorManager.DidAnimationFinish(AttackingAnimation))
            {
                var direction = player.IsFacingLeft ? Vector2.left : Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, AttackRange, player.EnemyLayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent<_EnemyStateManager>(out var enemy))
                    {
                        enemy.Damage(AttackDamage);

                        if (!enemy.IsAlive)
                        {
                            enemy.SwitchState(enemy.DeathState);
                        }
                    }
                }

                _enemyDetectionFinished = true;
            }
        }

        public override void UpdateState(_PlayerStateManager player)
        {
            if (player.AnimatorManager.DidAnimationFinish(AttackingAnimation) && _enemyDetectionFinished)
            {
                float horizontal = Input.GetAxis(_PlayerStateManager.HorizontalAxis);

                if (horizontal != 0f)
                {
                    player.SwitchState(player.AliveState.MovementState);
                }
                else
                {
                    player.SwitchState(player.AliveState.IdleState);
                }
            }
        }

        #endregion
    }
}
