using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerDeathState : _PlayerBaseState
    {
        #region Constant Fields

        private const string DyingAnimation = "Dying";

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            player.AnimatorManager.ChangeAnimationState(DyingAnimation);
        }

        public override void ExitState(_PlayerStateManager player)
        {

        }

        public override void FixedUpdateState(_PlayerStateManager player)
        {

        }

        public override void UpdateState(_PlayerStateManager player)
        {
            if (player.AnimatorManager.DidAnimationFinish(DyingAnimation))
            {
                Object.Destroy(player.gameObject);
            }
        }

        #endregion
    }
}
