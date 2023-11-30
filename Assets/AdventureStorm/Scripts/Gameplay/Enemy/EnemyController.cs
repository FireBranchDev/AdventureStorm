using AdventureStorm.Gameplay.Level;
using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyController : Controller, IDamageable, IKeyable
    {
        #region Properties

        public override float MovementSpeed => 2.85f;

        public bool HasKey { get; set; }

        public virtual float AttackDelay => 0.25f;

        public virtual float MovementDistance => 5f;

        public override float MaxHealth => 6f;

        #endregion

        #region Fields

        protected GameObject Dynamic;

        protected Coroutine Death;

        protected Coroutine Hurt;

        protected float DistanceToPlayer;

        protected IController PlayerController;

        protected IDamageable PlayerDamageable;

        private RandomEnemyWithKey _randomEnemyWithKey;

        private GameObject _system;

        private IKeyable _keyable;

        private IHealable _playerHealable;

        #endregion

        #region LifeCycle

        protected override void Start()
        {
            base.Start();

            _health = MaxHealth;

            if (_system == null)
            {
                _system = GameObject.Find("@System");
            }

            if (Dynamic == null)
            {
                Dynamic = GameObject.Find("_Dynamic");
            }

            if (_system != null)
            {
                if (_system.TryGetComponent<RandomEnemyWithKey>(out var randomEnemyWithKey))
                {
                    _randomEnemyWithKey = randomEnemyWithKey;
                }
            }

            AnimatorManager.ChangeAnimationState(EnemyAnimation.Idle);

            if (_keyable == null)
            {
                _keyable = GetComponent<IKeyable>();
            }

            if (Player != null)
            {
                if (PlayerDamageable == null)
                {
                    PlayerDamageable = Player.GetComponent<IDamageable>();
                }

                if (PlayerController == null)
                {
                    PlayerController = Player.GetComponent<IController>();
                }

                if (_playerHealable == null)
                {
                    _playerHealable = Player.GetComponent<IHealable>();
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if (Player != null)
            {
                DistanceToPlayer = Mathf.Abs(Player.transform.position.x - transform.position.x);

                if (IsAlive)
                {
                    FacePlayer();
                }
            }

            if (IsAlive)
            {
                if (!PlayerController.IsAlive)
                {
                    AnimatorManager.ChangeAnimationState(EnemyAnimation.Idle);
                    AnimatorManager.ReplayAnimation(EnemyAnimation.Idle);
                }
                else
                {
                    if (DistanceToPlayer > MovementDistance && Hurt == null)
                    {
                        AnimatorManager.ChangeAnimationState(EnemyAnimation.Idle);
                        AnimatorManager.ReplayAnimation(EnemyAnimation.Idle);
                    }
                    else if (DistanceToPlayer > AttackDistance && Hurt == null)
                    {
                        AnimatorManager.ChangeAnimationState(EnemyAnimation.Walking);
                        AnimatorManager.ReplayAnimation(EnemyAnimation.Walking);

                        Vector3 position = Vector3.zero;
                        if (transform.position.x > Player.transform.position.x)
                        {
                            position.x = -MovementSpeed;
                        }
                        else
                        {
                            position.x = MovementSpeed;
                        }

                        position *= Time.deltaTime;

                        transform.Translate(position);
                    }
                }
            }
            else
            {
                if (Death == null)
                {
                    Death = StartCoroutine(DeathCoroutine());
                }
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        #endregion

        #region Protected Methods

        protected IEnumerator DeathCoroutine()
        {
            AnimatorManager.ChangeAnimationState(EnemyAnimation.Death);
            while (!AnimatorManager.DidAnimationFinish(EnemyAnimation.Death))
            {
                yield return null;
            }

            if (_playerHealable != null)
            {
                _playerHealable.Heal(PlayerController.MaxHealth * 0.5f);
            }

            if (_keyable != null)
            {
                if (_keyable.HasKey)
                {
                    if (_randomEnemyWithKey != null && Dynamic)
                    {
                        GameObject key = Instantiate(_randomEnemyWithKey.Key, Dynamic.transform);
                        Vector3 keyPosition = transform.position;

                        keyPosition.y += 0.5f;

                        if (transform.localScale.x > 0f)
                        {
                            keyPosition.x -= 1.2f;
                        }
                        else
                        {
                            keyPosition.x += 1.2f;
                        }

                        key.transform.position = keyPosition;
                    }
                }
            }

            Destroy(gameObject);
        }

        protected void FacePlayer()
        {
            Vector3 localScale = transform.localScale;
            if (transform.position.x > Player.transform.position.x)
            {
                localScale.x = -Mathf.Abs(localScale.x);
            }
            else
            {
                localScale.x = Mathf.Abs(localScale.x);
            }

            transform.localScale = localScale;
        }

        #endregion

        #region Public Methods

        public void Damage(float damage)
        {
            if (!IsAlive) return;

            Hurt = StartCoroutine(HurtCoroutine());

            _health -= damage;

            if (!IsAlive)
            {
                AnimatorManager.ChangeAnimationState(EnemyAnimation.Death);
            }
        }

        public void Damage(int damage)
        {
            if (!IsAlive) return;

            Hurt = StartCoroutine(HurtCoroutine());

            _health -= damage;

            if (!IsAlive)
            {
                AnimatorManager.ChangeAnimationState(EnemyAnimation.Death);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator HurtCoroutine()
        {
            AnimatorManager.ChangeAnimationState(EnemyAnimation.Hurt);

            if (AnimatorManager.DidAnimationFinish(EnemyAnimation.Hurt))
            {
                AnimatorManager.ReplayAnimation(EnemyAnimation.Hurt);
            }

            while (!AnimatorManager.DidAnimationFinish(EnemyAnimation.Hurt))
            {
                yield return null;
            }

            Hurt = null;
        }

        #endregion
    }
}