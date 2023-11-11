using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class PlayerIdleState : PlayerBaseState
    {
        #region Constant Fields

        private const string IdleAnimation = "Idle";

        #endregion

        #region Public Methods

        public override void EnterState(PlayerStateManager player)
        {
            player.AnimatorManager.ChangeAnimationState(IdleAnimation);

            player.AliveState.RechargeDodgeAttackStaminaCoroutine ??=
                player.StartCoroutine(player.AliveState.DodgingState.RechargeDodgeAttackStaminaCoroutine());
        }

        public override void ExitState(PlayerStateManager player)
        {

        }

        public override void FixedUpdateState(PlayerStateManager player)
        {

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
            if (player.AnimatorManager != null)
            {
                if (player.AnimatorManager.DidAnimationFinish(IdleAnimation))
                {
                    player.AnimatorManager.ReplayAnimation();
                }
            }

            float horizontal = Input.GetAxis(PlayerStateManager.HorizontalAxis);

            if (horizontal != 0f)
            {
                player.SwitchState(player.AliveState.MovementState);
            }

            if (Input.GetMouseButtonDown(0))
            {
                player.SwitchState(player.AliveState.AttackingState);
            }

            if (Input.GetKey(KeyCode.Space) && horizontal != 0f)
            {
                player.SwitchState(player.AliveState.DodgingState);
            }
        }

        #endregion
    }
}
