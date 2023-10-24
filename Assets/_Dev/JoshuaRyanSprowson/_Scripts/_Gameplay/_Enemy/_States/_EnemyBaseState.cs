namespace AdventureStorm
{
    public abstract class _EnemyBaseState
    {
        public abstract void EnterState(_EnemyStateManager enemy);

        public abstract void ExitState(_EnemyStateManager enemy);

        public abstract void FixedUpdateState(_EnemyStateManager enemy);

        public abstract void UpdateState(_EnemyStateManager enemy);
    }
}
