using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class PlayerAttackingState : PlayerBaseState
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

        public PlayerAttackingState()
        {
            _enemyDetectionFinished = false;
        }

        #endregion

        #region Public Methods

        public override void EnterState(PlayerStateManager player)
        {
            _enemyDetectionFinished = false;
            player.AnimatorManager.ChangeAnimationState(AttackingAnimation);
        }

        public override void ExitState(PlayerStateManager player)
        {

        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            if (player.AnimatorManager.DidAnimationFinish(AttackingAnimation))
            {
                var direction = player.IsFacingLeft ? Vector2.left : Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, AttackRange, player.EnemyLayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent<EnemyStateManager>(out var enemy))
                    {
                        enemy.Damage(AttackDamage);
                    }
                }

                _enemyDetectionFinished = true;
            }
        }

        public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerExit2D(PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerStay2D(PlayerStateManager player, Collider2D collision)
        {

        }

        public override void UpdateState(PlayerStateManager player)
        {
            if (player.AnimatorManager.DidAnimationFinish(AttackingAnimation) && _enemyDetectionFinished)
            {
                float horizontal = Input.GetAxis(PlayerStateManager.HorizontalAxis);

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
