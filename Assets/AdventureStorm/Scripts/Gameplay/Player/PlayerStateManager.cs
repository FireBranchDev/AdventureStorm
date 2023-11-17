using AdventureStorm.Tools;
using Spriter2UnityDX;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class PlayerStateManager : MonoBehaviour, IDamageable
    {
        #region Constant Fields

        public const string HorizontalAxis = "Horizontal";

        public const string EnemyLayerMaskName = "Enemy";

        public const float MaximumHealth = 5f;

        private const string LeftBoundaryTag = "LeftBoundary";

        private const string RightBoundaryTag = "RightBoundary";

        private const string GroundBoundaryTag = "GroundBoundary";

        private const float BoundaryWallMargin = 3.6f;

        #endregion

        #region Fields

        private EntityRenderer _entityRenderer;

        private PlayerBaseState _currentState;

        private GameObject _leftBoundary;
        private GameObject _rightBoundary;
        private GameObject _groundBoundary;

        private float _spriteWidth;

        #endregion

        #region Properties

        public Rigidbody2D Rb2D { get; private set; }

        public AnimatorManager AnimatorManager { get; private set; }

        public bool IsFacingLeft { get; private set; }

        public LayerMask EnemyLayerMask { get; private set; }

        public PlayerAliveState AliveState { get; private set; }

        public PlayerDeathState DeathState { get; private set; }

        public bool HasKey { get; set; }

        public float Health { get; private set; }

        public bool IsAlive { get => Health > 0f; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            if (TryGetComponent<EntityRenderer>(out var entityRenderer))
            {
                _entityRenderer = entityRenderer;
            }

            _currentState = null;

            _leftBoundary = GameObject.FindWithTag(LeftBoundaryTag);
            _rightBoundary = GameObject.FindWithTag(RightBoundaryTag);
            _groundBoundary = GameObject.FindWithTag(GroundBoundaryTag);

            _spriteWidth = 0f;

            Rb2D = GetComponent<Rigidbody2D>();

            IsFacingLeft = false;

            EnemyLayerMask = LayerMask.GetMask(EnemyLayerMaskName);

            AliveState = new PlayerAliveState();
            DeathState = new PlayerDeathState();

            HasKey = false;

            Health = MaximumHealth;
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _currentState.OnTriggerEnter2D(this, collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _currentState.OnTriggerExit2D(this, collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            _currentState.OnTriggerStay2D(this, collision);
        }

        private void Start()
        {
            AnimatorManager = GetComponent<AnimatorManager>();

            if (_entityRenderer != null)
            {
                _spriteWidth = _entityRenderer.SpriteRenderer.size.x;
            }

            _currentState = AliveState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            if (_groundBoundary != null)
            {
                if (_leftBoundary != null)
                {
                    PreventPassingThroughLeftWallBoundary();
                }

                if (_rightBoundary != null)
                {
                    PreventPassingThroughRightWallBoundary();
                }
            }

            if (IsAlive)
            {
                FlipPlayerSprite();
            }

            _currentState.UpdateState(this);
        }

        #endregion

        #region Public Methods

        public void SwitchState(PlayerBaseState state)
        {
            if (_currentState == state) return;

            _currentState.ExitState(this);
            _currentState = state;
            _currentState.EnterState(this);
        }

        public void Damage(float damage)
        {
            Health -= damage;
        }

        public void Heal(float health)
        {
            if (Health + health > MaximumHealth)
            {
                Health = MaximumHealth;
            }
            else
            {
                Health += health;
            }
        }

        public void FinishedDyingAnimation()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void PreventPassingThroughLeftWallBoundary()
        {
            Vector3 playerPos = transform.position;
            Vector3 leftWallPos = _leftBoundary.transform.position;

            if (playerPos.x <= leftWallPos.x + _spriteWidth * BoundaryWallMargin)
            {
                Vector3 newPosition = Vector3.zero;

                newPosition.x = leftWallPos.x + _spriteWidth * BoundaryWallMargin;
                newPosition.y = _groundBoundary.transform.position.y;

                transform.position = newPosition;
            }
        }

        private void PreventPassingThroughRightWallBoundary()
        {
            Vector3 playerPos = transform.position;
            Vector3 rightWallPos = _rightBoundary.transform.position;

            if (playerPos.x >= rightWallPos.x - _spriteWidth * BoundaryWallMargin)
            {
                Vector3 newPosition = Vector3.zero;

                newPosition.x = rightWallPos.x - _spriteWidth * BoundaryWallMargin;
                newPosition.y = _groundBoundary.transform.position.y;

                transform.position = newPosition;
            }
        }

        private void FlipPlayerSprite()
        {
            float horizontal = Input.GetAxis(HorizontalAxis);

            Vector3 localScale = transform.localScale;

            if (!IsFacingLeft && horizontal < 0f)
            {
                IsFacingLeft = true;
                localScale.x = -localScale.x;
            }
            else if (IsFacingLeft && horizontal > 0f)
            {
                IsFacingLeft = false;
                localScale.x = Mathf.Abs(localScale.x);
            }

            transform.localScale = localScale;
        }

        #endregion
    }
}
