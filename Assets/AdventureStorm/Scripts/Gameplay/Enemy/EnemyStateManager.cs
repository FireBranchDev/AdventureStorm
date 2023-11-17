using AdventureStorm.Gameplay.Enemy.States;
using AdventureStorm.Tools;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyStateManager<T> : MonoBehaviour, IDamageable
    {
        #region Constant Fields

        public const string PlayerTag = "Player";

        public const string PlayerLayerMaskName = "Player";

        public const float MaximumHealth = 5f;

        #endregion

        #region Fields

        [Tooltip("Used when the randomly selected enemy dies.")]
        [SerializeField] private GameObject _keyPrefab;

        private EnemyBaseState<T> _currentState;

        #endregion

        #region Properties

        public Rigidbody2D RB2D { get; private set; }

        public AnimatorManager AnimatorManager { get; private set; }

        public LayerMask PlayerLayerMask { get; private set; }

        public float Health { get; private set; }

        public bool IsAlive { get => Health > 0; }

        public bool IsFacingLeft { get; set; }

        public GameObject KeyPrefab { get => _keyPrefab; }

        protected T StateManager { get; set; }

        #endregion

        #region LifeCycle

        protected virtual void Awake()
        {
            RB2D = GetComponent<Rigidbody2D>();

            AnimatorManager = GetComponent<AnimatorManager>();

            PlayerLayerMask = LayerMask.GetMask(PlayerLayerMaskName);

            Health = MaximumHealth;

            IsFacingLeft = true;
        }

        protected virtual void Start()
        {

        }

        protected virtual void FixedUpdate()
        {
            _currentState.FixedUpdateState(StateManager);
        }

        protected virtual void Update()
        {
            if (IsAlive)
            {
                FacePlayer(this);
                FlipEnemy(this);
            }

            _currentState.UpdateState(StateManager);
        }

        #endregion

        #region Public Methods

        public void SwitchState(EnemyBaseState<T> state)
        {
            if (_currentState != null)
            {
                _currentState.ExitState(StateManager);
            }

            _currentState = state;
            _currentState.EnterState(StateManager);
        }

        public void Damage(float damage)
        {
            if (IsAlive)
            {
                Health -= damage;
            }
        }

        public void FinishedDyingAnimation()
        {
            throw new System.NotImplementedException();
        }

        public void FacePlayer(EnemyStateManager<T> enemy)
        {
            RaycastHit2D left = Physics2D.Raycast(enemy.transform.position, Vector2.left, float.MaxValue, enemy.PlayerLayerMask);
            RaycastHit2D right = Physics2D.Raycast(enemy.transform.position, Vector2.right, float.MaxValue, enemy.PlayerLayerMask);

            if (IsFacingLeft)
            {
                if (right.collider != null)
                {
                    IsFacingLeft = false;
                }
                else
                {
                    IsFacingLeft = true;
                }
            }
            else
            {
                if (left.collider != null)
                {
                    IsFacingLeft = true;
                }
                else
                {
                    IsFacingLeft = false;
                }
            }

            if (left.collider != null && right.collider != null)
            {
                IsFacingLeft = true;
            }
        }

        public void FlipEnemy(EnemyStateManager<T> enemy)
        {
            Vector3 direction = enemy.transform.localScale;

            if (IsFacingLeft && enemy.transform.localScale.x < 0)
                return;

            if (!IsFacingLeft && enemy.transform.localScale.x > 0)
                return;

            if (IsFacingLeft)
            {
                direction.x = -enemy.transform.localScale.x;
            }
            else
            {
                direction.x = Mathf.Abs(enemy.transform.localScale.x);
            }

            enemy.transform.localScale = direction;
        }

        #endregion
    }
}
