using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyDeathState : _EnemyBaseState
    {
        #region Constant Fields

        private const string DyingAnimation = "Dying";

        #endregion

        #region Public Methods

        public override void EnterState(_EnemyStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(DyingAnimation);
        }

        public override void ExitState(_EnemyStateManager enemy)
        {

        }

        public override void FixedUpdateState(_EnemyStateManager enemy)
        {

        }

        public override void UpdateState(_EnemyStateManager enemy)
        {
            if (enemy.AnimatorManager.DidAnimationFinish(DyingAnimation))
            {
                Object.Destroy(enemy.gameObject);
            }
        }

        #endregion
    }
}
