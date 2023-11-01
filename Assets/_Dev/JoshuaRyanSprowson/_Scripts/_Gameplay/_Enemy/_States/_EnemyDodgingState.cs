using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyDodgingState : _EnemyBaseState
    {
        #region Constant Fields

        private const string StartJumpAnimation = "Start Jump";

        #endregion

        #region Fields

        private Coroutine _dodgeCoroutine;

        #endregion

        #region Public Methods

        public override void EnterState(_EnemyStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(StartJumpAnimation);
            _dodgeCoroutine = enemy.StartCoroutine(DodgeCoroutine(enemy));
        }

        public override void ExitState(_EnemyStateManager enemy)
        {
            if (_dodgeCoroutine != null)
            {
                enemy.StopCoroutine(_dodgeCoroutine);
                _dodgeCoroutine = null;
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

        private IEnumerator DodgeCoroutine(_EnemyStateManager enemy)
        {
            yield return new WaitForSeconds(0.25f);
            enemy.SwitchState(enemy.AliveState.CombatState);
        }
        
        #endregion
    }
}
