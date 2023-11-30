using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.Boss
{
    public class BossController : EnemyController
    {
        #region Fields

        public GameObject EnemyOne;
        public GameObject EnemyTwo;

        private Coroutine _attack;

        private Coroutine _taunt;

        private float _tauntJumpDistance = 8f;

        private float _enemyOneChance = 75f;

        private bool _hasTaunted;

        #endregion

        #region Properties

        public override float AttackDamage => 2.5f;

        public override float MaxHealth => 12f;


        public override float AttackDelay => 0.95f;

        #endregion

        #region LifeCycle

        protected override void Start()
        {
            base.Start();

            Debug.Log("BossController starting...");
        }

        protected override void Update()
        {
            base.Update();

            if (IsAlive)
            {
                if (DistanceToPlayer <= AttackDistance)
                {
                    if (!_hasTaunted)
                    {
                        _taunt = StartCoroutine(TauntCoroutine());
                        _hasTaunted = true;
                    }
                    else if (_attack == null && _taunt == null)
                    {
                        _attack = StartCoroutine(AttackCoroutine());
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator AttackCoroutine()
        {
            AnimatorManager.ChangeAnimationState(BossAnimation.Attacking);

            if (AnimatorManager.DidAnimationFinish(BossAnimation.Attacking))
            {
                AnimatorManager.ReplayAnimation(BossAnimation.Attacking);
            }

            while (!AnimatorManager.DidAnimationFinish(BossAnimation.Attacking))
            {
                yield return null;
            }

            yield return new WaitForSeconds(AttackDelay);

            if (PlayerDamageable != null)
            {
                PlayerDamageable.Damage(AttackDamage);
            }

            _attack = null;
        }

        private IEnumerator TauntCoroutine()
        {
            AnimatorManager.ChangeAnimationState(BossAnimation.Taunt);

            if (AnimatorManager.DidAnimationFinish(BossAnimation.Taunt))
            {
                AnimatorManager.ReplayAnimation(BossAnimation.Taunt);
            }

            while (!AnimatorManager.DidAnimationFinish(BossAnimation.Taunt))
            {
                yield return null;
            }
            Vector3 position = Vector3.zero;
            if (transform.localScale.x < 0f)
            {
                position.x = _tauntJumpDistance;
            }
            else
            {
                position.x = -_tauntJumpDistance;
            }
            transform.Translate(position);

            Vector3 futurePositionOfSpawnedEnemy = transform.position;
            if (transform.localScale.x < 0f)
            {
                futurePositionOfSpawnedEnemy.x -= _tauntJumpDistance / 2;
            }
            else
            {
                futurePositionOfSpawnedEnemy.x += _tauntJumpDistance / 2;
            }

            float randomChance = Random.Range(1, 100f);
            if (randomChance <= _enemyOneChance)
            {
                if (EnemyOne != null)
                {
                    GameObject spawnedEnemy = Instantiate(EnemyOne, Dynamic.transform);
                    spawnedEnemy.transform.position = futurePositionOfSpawnedEnemy;
                }
            }
            else
            {
                if (EnemyTwo != null)
                {
                    GameObject spawnedEnemy = Instantiate(EnemyTwo, Dynamic.transform);
                    spawnedEnemy.transform.position = futurePositionOfSpawnedEnemy;
                }
            }

            yield return null;

            _taunt = null;
        }

        #endregion
    }
}