using AdventureStorm.Gameplay.EnemyOne.States;
using System.Collections;

namespace AdventureStorm.Gameplay.EnemyOne
{
    public class EnemyOneStateManager : EnemyStateManager<EnemyOneStateManager>
    {
        #region Constructors

        public EnemyOneStateManager()
        {
            StateManager = this;
        }

        #endregion

        #region Properties

        public EnemyOneAliveState AliveState { get; private set; }

        public EnemyOneDeathState DeathState { get; private set; }

        #endregion

        #region LifeCycle

        protected override void Awake()
        {
            base.Awake();

            AliveState = new EnemyOneAliveState();
            DeathState = new EnemyOneDeathState();

            StartCoroutine(DeathCoroutine());
        }

        protected override void Start()
        {
            base.Start();

            SwitchState(AliveState);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void Update()
        {
            base.Update();
        }

        #endregion

        #region Private Methods

        private IEnumerator DeathCoroutine()
        {
            while (IsAlive)
            {
                yield return null;
            }

            SwitchState(DeathState);
        }

        #endregion
    }
}
