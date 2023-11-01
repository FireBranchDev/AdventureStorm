using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerStateManager : MonoBehaviour, _IDamageable
    {
        #region Constant Fields

        public const string HorizontalAxis = "Horizontal";

        public const string EnemyLayerMaskName = "Enemy";

        public const float MaximumHealth = 5f;

        #endregion

        #region Fields

        private _PlayerBaseState _currentState;

        #endregion

        #region Properties

        public Rigidbody2D Rb2D { get; private set; }

        public _AnimatorManager AnimatorManager { get; private set; }

        public bool IsFacingLeft { get; private set; }

        public LayerMask EnemyLayerMask { get; private set; }

        public _PlayerAliveState AliveState { get; private set; }

        public _PlayerDeathState DeathState { get; private set; }

        public float Health { get; private set; }

        public bool IsAlive { get => Health > 0f; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _currentState = null;

            Rb2D = GetComponent<Rigidbody2D>();
            AnimatorManager = GetComponent<_AnimatorManager>();

            IsFacingLeft = false;

            EnemyLayerMask = LayerMask.GetMask(EnemyLayerMaskName);

            AliveState = new _PlayerAliveState();
            DeathState = new _PlayerDeathState();

            Health = MaximumHealth;
        }

        private void Start()
        {
            _currentState = AliveState;
            _currentState.EnterState(this);
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
        }

        private void Update()
        {
            if (IsAlive)
            {
                FlipPlayerSprite();
            }

            _currentState.UpdateState(this);
        }

        #endregion

        #region Public Methods

        public void SwitchState(_PlayerBaseState state)
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
