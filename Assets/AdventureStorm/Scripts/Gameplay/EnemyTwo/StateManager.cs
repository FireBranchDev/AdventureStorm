using AdventureStorm.Gameplay.Enemy;
using AdventureStorm.Gameplay.EnemyTwo.States;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyTwo
{
    public class StateManager : EnemyStateManager
    {
        #region Constant Fields

        private const string DynamicGameObjectName = "_Dynamic";

        #endregion

        #region Fields

        public GameObject Dynamic;

        [Tooltip("Is used when the enemy initiates an attack.")]
        public GameObject Projectile;

        private readonly State _state = new();
        private GameObject _player;

        #endregion

        #region Properties

        public float MovementSpeed => 4.25f;
        public float DistanceForMovement => 10f;
        public float AttackDistance => 4f;
        public float AttackDamage = 1.25f;

        public State States => _state;
        public GameObject Player => _player;

        #endregion

        #region Constructors

        public StateManager()
        {
            Self = this;
        }

        #endregion

        #region LifeCycle

        protected override void Awake()
        {
            base.Awake();

            _player = GameObject.FindGameObjectWithTag(PlayerTag);

            Dynamic = GameObject.Find(DynamicGameObjectName);
        }

        protected override void Start()
        {
            base.Start();

            SwitchState(States.Idle);
        }

        protected override void Update()
        {
            if (CurrentState != null && Self != null)
            {
                CurrentState.UpdateState(Self);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the distance between the enemy and the player.
        /// </summary>
        /// <returns>The distance between the enemy and the player.</returns>
        public float DistanceToPlayer()
        {
            if (Player != null)
            {
                return Mathf.Abs(transform.position.x - Player.transform.position.x);
            }

            return -1;
        }

        /// <summary>
        /// Flips the sprite, to the opposite of the current enemys faced direction.
        /// </summary>
        public void FlipSprite()
        {
            var localScale = transform.localScale;
            if (IsFacingLeft)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }
            else
            {
                localScale.x = -localScale.x;
            }
            transform.localScale = localScale;
            IsFacingLeft = !IsFacingLeft;
        }

        #endregion
    }
}
