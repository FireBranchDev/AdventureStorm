using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyDodgingState : EnemyBaseState
    {
        #region Constant Fields

        private const string StartJumpAnimation = "Start Jump";

        #endregion

        #region Fields

        private Coroutine _dodgeCoroutine;

        #endregion

        #region Public Methods

        public override void EnterState(EnemyStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(StartJumpAnimation);
            _dodgeCoroutine = enemy.StartCoroutine(DodgeCoroutine(enemy));
        }

        public override void ExitState(EnemyStateManager enemy)
        {
            if (_dodgeCoroutine != null)
            {
                enemy.StopCoroutine(_dodgeCoroutine);
                _dodgeCoroutine = null;
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

        private IEnumerator DodgeCoroutine(EnemyStateManager enemy)
        {
            yield return new WaitForSeconds(0.25f);
            enemy.SwitchState(enemy.AliveState.CombatState);
        }

        #endregion
    }
}
