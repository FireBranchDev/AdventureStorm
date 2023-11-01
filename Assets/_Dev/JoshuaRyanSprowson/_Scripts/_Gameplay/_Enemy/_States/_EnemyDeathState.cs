using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyDeathState : _EnemyBaseState
    {
        #region Constant Fields

        private const string DyingAnimation = "Dying";

        #endregion

        #region Fields

        private Coroutine _deathCoroutine;

        #endregion

        #region Constructors

        public _EnemyDeathState()
        {
            _deathCoroutine = null;

            DeathRewardState = new _EnemyDeathRewardState();
        }

        #endregion

        #region Properties

        public _EnemyDeathRewardState DeathRewardState { get; private set; }

        #endregion

        #region Public Methods

        public override void EnterState(_EnemyStateManager enemy)
        {
            if (_deathCoroutine == null)
            {
                _deathCoroutine = enemy.StartCoroutine(DeathCoroutine(enemy));
            }
        }

        public override void ExitState(_EnemyStateManager enemy)
        {
            if (_deathCoroutine != null)
            {
                enemy.StopCoroutine(_deathCoroutine);
                _deathCoroutine = null;
            }
        }

        public override void FixedUpdateState(_EnemyStateManager enemy)
        {

        }

        public override void UpdateState(_EnemyStateManager enemy)
        {
           
        }

        #endregion

        #region Private Methods

        private IEnumerator DeathCoroutine(_EnemyStateManager enemy)
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
