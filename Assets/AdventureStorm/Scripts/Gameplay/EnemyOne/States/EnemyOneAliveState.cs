namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneAliveState : BaseState
    {
        #region Constructors

        public EnemyOneAliveState()
        {
            CombatState = new EnemyOneCombatState();
            IdleState = new EnemyOneIdleState();
            MovementState = new EnemyOneMovementState();
        }

        #endregion

        #region Properties

        public EnemyOneCombatState CombatState { get; private set; }
        public EnemyOneIdleState IdleState { get; private set; }
        public EnemyOneMovementState MovementState { get; private set; }

        #endregion

        #region Public Methods

        public override void EnterState(StateManager stateManager)
        {
            stateManager.SwitchState(IdleState);
        }

        public override void ExitState(StateManager stateManager)
        {

        }

        public override void FixedUpdateState(StateManager stateManager)
        {

        }

        public override void UpdateState(StateManager stateManager)
        {

        }

        #endregion
    }
}
