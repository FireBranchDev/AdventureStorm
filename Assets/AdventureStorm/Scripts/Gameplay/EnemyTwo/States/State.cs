namespace AdventureStorm.Gameplay.EnemyTwo.States
{
    public class State
    {
        #region Fields

        public readonly Idle Idle = new();
        public readonly Movement Movement = new();
        public readonly Attack Attack = new();
        public readonly Flee Flee = new();
        public readonly Death Death = new();

        #endregion
    }
}
