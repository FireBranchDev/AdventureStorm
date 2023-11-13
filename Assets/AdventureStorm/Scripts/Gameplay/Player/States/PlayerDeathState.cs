using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class PlayerDeathState : PlayerBaseState
    {
        #region Constant Fields

        private const string DyingAnimation = "Dying";

        #endregion

        #region Public Methods

        public override void EnterState(PlayerStateManager player)
        {
            player.AnimatorManager.ChangeAnimationState(DyingAnimation);
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
            if (player.AnimatorManager.DidAnimationFinish(DyingAnimation))
            {
                Object.Destroy(player.gameObject);
            }
        }

        #endregion
    }
}