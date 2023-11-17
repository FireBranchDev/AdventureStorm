using AdventureStorm.Gameplay.Enemy.States;
using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneDodgingState : EnemyBaseState<EnemyOneStateManager>
    {
        #region Constant Fields

        private const string StartJumpAnimation = "Start Jump";
        private const string IdleAnimation = "Idle";

        private const float DodgeDistance = 1.35f;
        private const float DodgeHeight = 0.75f;
        private const float InAirDuration = 0.2f;
        private const float DodgeCooldown = 0.5f;

        #endregion

        #region Fields

        private Coroutine _dodgeCoroutine;

        #endregion

        #region Public Methods

        public override void EnterState(EnemyOneStateManager enemy)
        {
            _dodgeCoroutine = enemy.StartCoroutine(DodgeCoroutine(enemy));
        }

        public override void ExitState(EnemyOneStateManager enemy)
        {
            if (_dodgeCoroutine != null)
            {
                enemy.StopCoroutine(_dodgeCoroutine);
                _dodgeCoroutine = null;
            }
        }

        public override void FixedUpdateState(EnemyOneStateManager enemy)
        {

        }

        public override void UpdateState(EnemyOneStateManager enemy)
        {

        }

        #endregion

        #region Private Methods

        private IEnumerator DodgeCoroutine(EnemyOneStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(StartJumpAnimation);

            Vector3 position = Vector3.zero;

            if (enemy.IsFacingLeft)
            {
                position.x = DodgeDistance / 2f;
            }
            else
            {
                position.x = -DodgeDistance / 2f;
            }

            position.y = DodgeHeight / 2f;
            enemy.transform.Translate(position);
            yield return new WaitForSeconds(0.05f);
            enemy.transform.Translate(position);

            yield return new WaitForSeconds(InAirDuration);

            position.x = 0;
            position.y = -DodgeHeight;

            enemy.transform.Translate(position);

            enemy.AnimatorManager.ChangeAnimationState(IdleAnimation);

            yield return new WaitForSeconds(DodgeCooldown);

            enemy.SwitchState(enemy.AliveState.CombatState);
        }

        #endregion
    }
}
