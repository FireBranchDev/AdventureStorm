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

            player.RechargeDodgeAttackStaminaCoroutine ??=
                player.StartCoroutine(player.DodgeAttackState.RechargeDodgeAttackStaminaCoroutine());
        }

        public override void ExitState(_PlayerStateManager player)
        {
            
        }

        public override void FixedUpdateState(_PlayerStateManager player)
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
                player.SwitchState(player.MovementState);
            }

            if (Input.GetMouseButtonDown(0))
            {
                player.SwitchState(player.AttackingState);
            }

            if (Input.GetKey(KeyCode.Space) && horizontal != 0f)
            {
                player.SwitchState(player.DodgeAttackState);
            }
        }

        #endregion
    }
}
