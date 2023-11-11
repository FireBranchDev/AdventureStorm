using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyDeathState : EnemyBaseState
    {
        #region Constant Fields

        private const string DyingAnimation = "Dying";

        #endregion

        #region Fields

        private Coroutine _deathCoroutine;

        #endregion

        #region Constructors

        public EnemyDeathState()
        {
            _deathCoroutine = null;

            DeathRewardState = new EnemyDeathRewardState();
        }

        #endregion

        #region Properties

        public EnemyDeathRewardState DeathRewardState { get; private set; }

        #endregion

        #region Public Methods

        public override void EnterState(EnemyStateManager enemy)
        {
            if (_deathCoroutine == null)
            {
                _deathCoroutine = enemy.StartCoroutine(DeathCoroutine(enemy));
            }
        }

        public override void ExitState(EnemyStateManager enemy)
        {
            if (_deathCoroutine != null)
            {
                enemy.StopCoroutine(_deathCoroutine);
                _deathCoroutine = null;
            }
        }

        public override void FixedUpdateState(EnemyStateManager enemy)
        {

        }

        public override void UpdateState(EnemyStateManager enemy)
        {

        }

        #endregion

        #region Private Methods

        private IEnumerator DeathCoroutine(EnemyStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(DyingAnimation);

            while (!enemy.AnimatorManager.DidAnimationFinish(DyingAnimation))
            {
                yield return null;
            }

            enemy.SwitchState(DeathRewardState);
        }

        #endregion
    }
}
