namespace AdventureStorm.Gameplay
{
    public abstract class BaseState
    {
        public abstract void EnterState(StateManager stateManager);

        public abstract void ExitState(StateManager stateManager);

        public abstract void FixedUpdateState(StateManager stateManager);

        public abstract void UpdateState(StateManager stateManager);
    }
}
