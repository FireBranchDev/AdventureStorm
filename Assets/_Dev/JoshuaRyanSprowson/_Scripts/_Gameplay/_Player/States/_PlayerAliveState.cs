using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerAliveState : _PlayerBaseState
    {
        #region Constructors

        public _PlayerAliveState()
        {
            IdleState = new _PlayerIdleState();
            MovementState = new _PlayerMovementState();
            AttackingState = new _PlayerAttackingState();
            DodgingState = new _PlayerDodgingState();

            RechargeDodgeAttackStaminaCoroutine = null;
        }

        #endregion

        #region Properties

        public _PlayerIdleState IdleState { get; private set; }

        public _PlayerMovementState MovementState { get; private set; }

        public _PlayerAttackingState AttackingState { get; private set; }

        public _PlayerDodgingState DodgingState { get; private set; }

        public Coroutine RechargeDodgeAttackStaminaCoroutine { get; set; }

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            player.SwitchState(IdleState);
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

        }

        #endregion
    }
}
