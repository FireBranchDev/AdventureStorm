using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerStateManager : MonoBehaviour
    {
        #region Constant Fields

        public const string HorizontalAxis = "Horizontal";

        #endregion

        #region Fields

        private _PlayerBaseState _currentState;

        #endregion

        #region Properties

        public Rigidbody2D Rb2D { get; private set; }

        public _AnimatorManager AnimatorManager { get; private set; }

        public _PlayerIdleState IdleState { get; private set; }

        public _PlayerMovementState MovementState { get; private set; }

        public _PlayerAttackingState AttackingState { get; private set; }

        public _PlayerDodgeAttackState DodgeAttackState { get; private set; }

        public bool IsFacingLeft { get; private set; }

        public Coroutine RechargeDodgeAttackStaminaCoroutine { get; set; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            Rb2D = GetComponent<Rigidbody2D>();
            AnimatorManager = GetComponent<_AnimatorManager>();

            IdleState = new _PlayerIdleState();
            MovementState = new _PlayerMovementState();
            AttackingState = new _PlayerAttackingState();
            DodgeAttackState = new _PlayerDodgeAttackState();

            IsFacingLeft = false;

            _currentState = IdleState;
        }

        private void Start()
        {
            _currentState.EnterState(this);
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
        }

        private void Update()
        {
            FlipPlayerSprite();

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
