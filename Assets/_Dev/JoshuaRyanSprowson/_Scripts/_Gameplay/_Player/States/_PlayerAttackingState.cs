using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerAttackingState : _PlayerBaseState
    {
        #region Constant Fields

        private const string AttackingAnimation = "Attacking";

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            player.AnimatorManager.ChangeAnimationState(AttackingAnimation);
        }

        public override void ExitState(_PlayerStateManager player)
        {
            
        }

        public override void FixedUpdateState(_PlayerStateManager player)
        {
            
        }

        public override void UpdateState(_PlayerStateManager player)
        {
            if (player.AnimatorManager.DidAnimationFinish(AttackingAnimation))
            {
                float horizontal = Input.GetAxis(_PlayerStateManager.HorizontalAxis);

                if (horizontal != 0f)
                {
                    player.SwitchState(player.MovementState);
                }
                else
                {
                    player.SwitchState(player.IdleState);
                }
            }
        }

        #endregion
    }
}
