namespace AdventureStorm.Gameplay.Enemy.States
{
    public abstract class EnemyBaseState<T>
    {
        public abstract void EnterState(T enemy);

        public abstract void ExitState(T enemy);

        public abstract void FixedUpdateState(T enemy);

        public abstract void UpdateState(T enemy);
    }
}
