using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne
{
    public class EnemyOneController : EnemyController
    {
        #region Fields

        private Coroutine _attack;

        #endregion

        #region LifeCycle

        protected override void Start()
        {
            base.Start();

            Debug.Log("EnemyOneController is starting...");
        }

        protected override void Update()
        {
            base.Update();

            if (IsAlive)
            {
                if (DistanceToPlayer <= AttackDistance &&
                    _attack == null &&
                    PlayerController.IsAlive)
                {
                    _attack = StartCoroutine(AttackCoroutine());
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator AttackCoroutine()
        {
            AnimatorManager.ChangeAnimationState(EnemyAnimation.Attacking);

            if (AnimatorManager.DidAnimationFinish(EnemyAnimation.Attacking))
            {
                AnimatorManager.ReplayAnimation(EnemyAnimation.Attacking);
            }

            while (!AnimatorManager.DidAnimationFinish(EnemyAnimation.Attacking))
            {
                yield return null;
            }

            if (PlayerDamageable != null)
            {
                PlayerDamageable.Damage(AttackDamage);
            }

            yield return new WaitForSeconds(AttackDelay);

            _attack = null;
        }

        #endregion
    }
}