using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneDodgingState : BaseState
    {
        #region Constant Fields

        private const string StartJumpAnimation = "Start Jump";
        private const string IdleAnimation = "Idle";

        private const float DodgeDistance = 2f;
        private const float DodgeHeight = 0.75f;
        private const float InAirDuration = 0.3f;
        private const float DodgeCooldown = 0.65f;

        #endregion

        #region Fields

        private Coroutine _dodgeCoroutine;

        #endregion

        #region Public Methods

        public override void EnterState(StateManager stateManager)
        {
            if (stateManager.TryGetComponent<EnemyOneStateManager>(out var enemyOneStateManager))
            {
                _dodgeCoroutine = stateManager.StartCoroutine(DodgeCoroutine(enemyOneStateManager));
            }
        }

        public override void ExitState(StateManager stateManager)
        {
            if (_dodgeCoroutine != null)
            {
                stateManager.StopCoroutine(_dodgeCoroutine);
                _dodgeCoroutine = null;
            }
        }

        public override void FixedUpdateState(StateManager stateManager)
        {

        }

        public override void UpdateState(StateManager stateManager)
        {

        }

        #endregion

        #region Private Methods

        private IEnumerator DodgeCoroutine(EnemyOneStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(StartJumpAnimation);

            Vector3 position = Vector3.zero;

            position.y = DodgeHeight / 2f;
            enemy.transform.Translate(position);

            if (enemy.IsFacingLeft)
            {
                position.x = DodgeDistance;
            }
            else
            {
                position.x = -DodgeDistance;
            }

            yield return new WaitForSeconds(0.1f);
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
