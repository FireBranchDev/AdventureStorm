using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyTwo.States
{
    public class Movement : BaseState
    {
        #region Instance Fields

        private Coroutine _movingCoroutine;
        private Coroutine _idleCoroutine;

        #endregion

        #region Fields

        private readonly float _timeForIdle = 1.1f;

        #endregion

        #region Public Methods

        public override void EnterState(Gameplay.StateManager stateManager)
        {
            if (stateManager.TryGetComponent<StateManager>(out var enemyTwoStateManager))
            {
                _movingCoroutine = stateManager.StartCoroutine(MovingCoroutine(enemyTwoStateManager));
                _idleCoroutine = stateManager.StartCoroutine(CheckIdleCoroutine(enemyTwoStateManager));
            }
        }

        public override void ExitState(Gameplay.StateManager stateManager)
        {
            if (_movingCoroutine != null)
            {
                stateManager.StopCoroutine(_movingCoroutine);
                _movingCoroutine = null;
            }

            if (_idleCoroutine != null)
            {
                stateManager.StopCoroutine(_idleCoroutine);
                _idleCoroutine = null;
            }
        }

        public override void FixedUpdateState(Gameplay.StateManager stateManager)
        {
            if (stateManager.TryGetComponent<StateManager>(out var enemyTwoStateManager))
            {
                Vector3 velocity = Vector3.zero;
                float speed = enemyTwoStateManager.MovementSpeed;
                velocity.x = stateManager.IsFacingLeft ? -speed : speed;
                velocity *= Time.deltaTime;
                stateManager.transform.Translate(velocity);
            }
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
                    if (enemyTwoStateManager.DistanceToPlayer() < enemyTwoStateManager.AttackDistance)
                    {
                        stateManager.SwitchState(enemyTwoStateManager.States.Attack);
                    }
                }
            }
        }

        #endregion

        private IEnumerator MovingCoroutine(StateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(Animation.Moving);
            while (true)
            {
                if (enemy.AnimatorManager.DidAnimationFinish(Animation.Moving))
                {
                    enemy.AnimatorManager.ReplayAnimation();
                }
                yield return null;
            }
        }

        private IEnumerator CheckIdleCoroutine(StateManager enemy)
        {
            while (true)
            {
                if (enemy.Player != null)
                {
                    if (enemy.DistanceToPlayer() > enemy.DistanceForMovement)
                    {
                        yield return new WaitForSeconds(_timeForIdle);
                        if (enemy.DistanceToPlayer() > enemy.DistanceForMovement)
                        {
                            enemy.SwitchState(enemy.States.Idle);
                        }
                    }
                }
                yield return null;
            }
        }
    }
}