namespace AdventureStorm
{
    public abstract class _PlayerBaseState
    {
        public abstract void EnterState(_PlayerStateManager player);

        public abstract void ExitState(_PlayerStateManager player);

        public abstract void FixedUpdateState(_PlayerStateManager player);

        public abstract void UpdateState(_PlayerStateManager player);
    }
}
