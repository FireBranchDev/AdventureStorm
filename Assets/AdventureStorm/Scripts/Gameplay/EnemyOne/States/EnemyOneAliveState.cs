using AdventureStorm.Gameplay.Enemy.States;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneAliveState : EnemyBaseState<EnemyOneStateManager>
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

        public override void EnterState(EnemyOneStateManager enemy)
        {
            enemy.SwitchState(IdleState);
        }

        public override void ExitState(EnemyOneStateManager enemy)
        {

        }

        public override void FixedUpdateState(EnemyOneStateManager enemy)
        {

        }

        public override void UpdateState(EnemyOneStateManager enemy)
        {

        }

        #endregion
    }
}
