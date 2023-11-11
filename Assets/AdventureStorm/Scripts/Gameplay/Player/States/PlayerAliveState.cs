using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class PlayerAliveState : PlayerBaseState
    {
        #region Constructors

        public PlayerAliveState()
        {
            IdleState = new PlayerIdleState();
            MovementState = new PlayerMovementState();
            AttackingState = new PlayerAttackingState();
            DodgingState = new PlayerDodgingState();

            RechargeDodgeAttackStaminaCoroutine = null;
        }

        #endregion

        #region Properties

        public PlayerIdleState IdleState { get; private set; }

        public PlayerMovementState MovementState { get; private set; }

        public PlayerAttackingState AttackingState { get; private set; }

        public PlayerDodgingState DodgingState { get; private set; }

        public Coroutine RechargeDodgeAttackStaminaCoroutine { get; set; }

        #endregion

        #region Public Methods

        public override void EnterState(PlayerStateManager player)
        {
            player.SwitchState(IdleState);
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

        }

        #endregion
    }
}
