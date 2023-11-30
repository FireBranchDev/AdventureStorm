using AdventureStorm.Gameplay.EnemyTwo.Orb;
using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyTwo
{
    public class EnemyTwoController : EnemyController, IHealable
    {
        #region Properties

        public override float MovementDistance => 7f;

        public override float AttackDistance => 4.5f;

        public override float AttackDelay => 1.25f;

        public override float MaxHealth => 7f;

        #endregion

        #region Fields

        [Tooltip("The orb is used as a projectile when attacking.")]
        public GameObject Orb;

        private float _fleeDistance = 6f;

        private Coroutine _attack;

        private Coroutine _flee;

        #endregion

        #region LifeCycle

        protected override void Start()
        {
            base.Start();

            Debug.Log("EnemyTwoController is starting...");
        }

        protected override void Update()
        {
            base.Update();

            if (!IsAlive)
            {
                if (Death == null)
                {
                    Death = StartCoroutine(DeathCoroutine());
                }
            }
            else
            {
                if (PlayerController.IsAlive)
                {
                    if (Hurt == null)
                    {
                        if (Health <= MaxHealth / 4
                            && _flee == null)
                        {
                            _flee = StartCoroutine(FleeCoroutine());
                        }
                        else if (_attack == null
                            && DistanceToPlayer <= AttackDistance)
                        {
                            _attack = StartCoroutine(AttackCoroutine());
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator AttackCoroutine()
        {
            AnimatorManager.ChangeAnimationState("Attacking");

            if (AnimatorManager.DidAnimationFinish("Attacking"))
            {
                AnimatorManager.ReplayAnimation("Attacking");
            }

            while (!AnimatorManager.DidAnimationFinish("Attacking"))
            {
                yield return null;
            }

            yield return new WaitForEndOfFrame();

            // Spawn orb projectile
            GameObject orb = Instantiate(Orb, Dynamic.transform);
            orb.transform.position = transform.position + new Vector3 { y = 1f };
            orb.GetComponent<OrbController>().Enemy = gameObject;

            yield return new WaitForSeconds(AttackDelay);

            _attack = null;
        }

        private IEnumerator FleeCoroutine()
        {
            Vector3 position = Vector3.zero;

            if (transform.localScale.x > 0f)
            {
                position.x = -_fleeDistance;
            }
            else
            {
                position.x = _fleeDistance;
            }

            transform.Translate(position);

            Heal(MaxHealth / 4);

            yield return null;
        }

        #endregion

        #region Public Methods

        public void Heal(float health)
        {
            if (Health + health > MaxHealth)
            {
                _health = MaxHealth;
            }
            else
            {
                _health += health;
            }
        }

        public void Heal(int health)
        {
            if (Health + health > MaxHealth)
            {
                _health = MaxHealth;
            }
            else
            {
                _health += health;
            }
        }

        #endregion
    }
}