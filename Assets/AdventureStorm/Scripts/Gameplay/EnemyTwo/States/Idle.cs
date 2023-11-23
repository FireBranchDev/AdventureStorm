using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyTwo.States
{
    public class Idle : BaseState
    {
        #region Fields

        private Coroutine _idle;

        #endregion

        #region Public Methods

        public override void EnterState(Gameplay.StateManager stateManager)
        {
            if (stateManager.TryGetComponent<StateManager>(out var enemyTwoStateManager))
            {
                _idle = stateManager.StartCoroutine(IdleCoroutine(enemyTwoStateManager));
            }
        }

        public override void ExitState(Gameplay.StateManager stateManager)
        {
            if (_idle != null)
            {
                stateManager.StopCoroutine(_idle);
                _idle = null;
            }
        }

        public override void FixedUpdateState(Gameplay.StateManager stateManager)
        {

        }

        public override void UpdateState(Gameplay.StateManager stateManager)
        {
            if (stateManager.TryGetComponent<StateManager>(out var enemyTwoStateManager))
            {
                if (stateManager.IsAlive)
                {
                    enemyTwoStateManager.FacePlayer(enemyTwoStateManager);
                    enemyTwoStateManager.FlipEnemy(enemyTwoStateManager);
                }

                if (enemyTwoStateManager.Player != null)
                {
                    float distanceToPlayer = enemyTwoStateManager.DistanceToPlayer();
                    if (distanceToPlayer < enemyTwoStateManager.DistanceForMovement
                        && distanceToPlayer > enemyTwoStateManager.AttackDistance)
                    {
                        enemyTwoStateManager.SwitchState(enemyTwoStateManager.States.Movement);
                    }
                    else if (distanceToPlayer < enemyTwoStateManager.AttackDistance)
                    {
                        enemyTwoStateManager.SwitchState(enemyTwoStateManager.States.Attack);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator IdleCoroutine(StateManager stateManager)
        {
            stateManager.AnimatorManager.ChangeAnimationState(Animation.Idle);
            while (true)
            {
                if (stateManager.AnimatorManager.DidAnimationFinish(Animation.Idle))
                {
                    stateManager.AnimatorManager.ReplayAnimation();
                }
                yield return null;
            }
        }

        #endregion
    }
}
