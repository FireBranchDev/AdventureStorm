using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerIdleState : _PlayerBaseState
    {
        #region Constant Fields

        private const string IdleAnimation = "Idle";

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            player.AnimatorManager.ChangeAnimationState(IdleAnimation);

            player.AliveState.RechargeDodgeAttackStaminaCoroutine ??=
                player.StartCoroutine(player.AliveState.DodgingState.RechargeDodgeAttackStaminaCoroutine());
        }

        public override void ExitState(_PlayerStateManager player)
        {

        }

        public override void FixedUpdateState(_PlayerStateManager player)
        {

        }

        public override void OnTriggerEnter2D(_PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerExit2D(_PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerStay2D(_PlayerStateManager player, Collider2D collision)
        {

        }

        public override void UpdateState(_PlayerStateManager player)
        {
            if (player.AnimatorManager != null)
            {
                if (player.AnimatorManager.DidAnimationFinish(IdleAnimation))
                {
                    player.AnimatorManager.ReplayAnimation();
                }
            }

            float horizontal = Input.GetAxis(_PlayerStateManager.HorizontalAxis);

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
