using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyTwo.States
{
    public class Attack : BaseState
    {
        #region Constant Fields

        private const float FleeDistance = 2.5f;
        private const float FleeChance = 25f;

        #endregion

        #region Instance Fields

        private Coroutine _attacking;
        private Coroutine _playIdlingAnimation;

        #endregion

        #region Fields

        private readonly float _attackDelay = 1.45f;
        private readonly float _spriteWidth = 0.35f;

        private bool _canFlee;

        #endregion

        #region Public Methods

        public override void EnterState(Gameplay.StateManager stateManager)
        {
            if (stateManager.TryGetComponent<StateManager>(out var enemyTwoStateManager))
            {
                float randomValue = Random.Range(1f, 100f);

                if (randomValue <= FleeChance)
                {
                    _canFlee = true;
                }

                _attacking = stateManager.StartCoroutine(Attacking(enemyTwoStateManager));
            }
        }

        public override void ExitState(Gameplay.StateManager stateManager)
        {
            if (_attacking != null)
            {
                stateManager.StopCoroutine(_attacking);
                _attacking = null;
            }

            if (_playIdlingAnimation != null)
            {
                stateManager.StopCoroutine(_playIdlingAnimation);
                _playIdlingAnimation = null;
            }
        }

        public override void FixedUpdateState(Gameplay.StateManager stateManager)
        {

        }

        public override void UpdateState(Gameplay.StateManager stateManager)
        {
            if (stateManager.TryGetComponent<StateManager>(out var enemyTwoStateManager))
            {
                if (!stateManager.IsAlive)
                {
                    stateManager.SwitchState(enemyTwoStateManager.States.Death);
                }

                if (enemyTwoStateManager.DistanceToPlayer() > enemyTwoStateManager.AttackDistance + _spriteWidth)
                {
                    stateManager.SwitchState(enemyTwoStateManager.States.Movement);
                }

                if (enemyTwoStateManager.DistanceToPlayer() <= FleeDistance && _canFlee)
                {
                    stateManager.SwitchState(enemyTwoStateManager.States.Flee);
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator Attacking(StateManager stateManager)
        {
            while (true)
            {
                if (_playIdlingAnimation == null)
                {
                    stateManager.AnimatorManager.ChangeAnimationState(Animation.Attacking);

                    if (stateManager.Dynamic != null)
                    {
                        GameObject spawnedProjectile = Object.Instantiate(stateManager.Projectile, stateManager.Dynamic.transform);
                        if (spawnedProjectile.TryGetComponent<Projectile>(out var projectile))
                        {
                            projectile.Enemy = stateManager.gameObject;
                        }
                    }

                    while (!stateManager.AnimatorManager.DidAnimationFinish(Animation.Attacking))
                    {
                        break;
                    }

                    _playIdlingAnimation = stateManager.StartCoroutine(PlayIdlingAnimation(stateManager));
                    yield return new WaitForSeconds(_attackDelay);
                }

                stateManager.StopCoroutine(PlayIdlingAnimation(stateManager));
                _playIdlingAnimation = null;
            }
        }

        private IEnumerator PlayIdlingAnimation(StateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(Animation.Idle);
            while (true)
            {
                if (enemy.AnimatorManager.DidAnimationFinish(Animation.Idle))
                {
                    enemy.AnimatorManager.ReplayAnimation();
                }
                yield return null;
            }
        }

        #endregion
    }
}
