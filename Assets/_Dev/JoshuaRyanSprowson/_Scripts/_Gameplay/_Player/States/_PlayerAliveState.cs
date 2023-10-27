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
            DodgeAttackState = new _PlayerDodgeAttackState();

            RechargeDodgeAttackStaminaCoroutine = null;
        }

        #endregion

        #region Properties

        public _PlayerIdleState IdleState { get; private set; }

        public _PlayerMovementState MovementState { get; private set; }

        public _PlayerAttackingState AttackingState { get; private set; }

        public _PlayerDodgeAttackState DodgeAttackState { get; private set; }

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

        public override void UpdateState(_PlayerStateManager player)
        {

        }

        #endregion
    }
}
