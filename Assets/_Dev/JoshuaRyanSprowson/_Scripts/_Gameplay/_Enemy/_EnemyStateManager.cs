using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyStateManager : MonoBehaviour
    {
        #region Constant Fields

        public const string PlayerTag = "Player";

        public const string PlayerLayerMaskName = "Player";

        #endregion

        #region Fields

        private _EnemyBaseState _currentState;

        #endregion

        #region Properties

        public Rigidbody2D RB2D { get; private set; }

        public _AnimatorManager AnimatorManager { get; private set; }

        public LayerMask PlayerLayerMask { get; private set; }

        public bool IsFacingLeft { get; private set; }

        public _EnemyAttackingState AttackingState { get; private set; }
        public _EnemyIdleState IdleState { get; private set; }
        public _EnemyMovementState MovementState { get; private set; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            RB2D = GetComponent<Rigidbody2D>();

            AnimatorManager = GetComponent<_AnimatorManager>();

            PlayerLayerMask = LayerMask.GetMask(PlayerLayerMaskName);

            IsFacingLeft = true;

            AttackingState = new _EnemyAttackingState();
            IdleState = new _EnemyIdleState();
            MovementState = new _EnemyMovementState();

            _currentState = IdleState;
        }

        // Start is called before the first frame update.
        private void Start()
        {
            _currentState.EnterState(this);
        }

        private void FixedUpdate()
        {
            RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, float.MaxValue, PlayerLayerMask);
            RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, float.MaxValue, PlayerLayerMask);

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

            Debug.DrawRay(transform.position, Vector2.left * float.MaxValue, Color.magenta);
            Debug.DrawRay(transform.position, Vector2.right * float.MaxValue, Color.magenta);

            _currentState.FixedUpdateState(this);
        }

        // Update is called once per frame.
        private void Update()
        {
            FlipEnemy();

            _currentState.UpdateState(this);
        }

        #endregion

        #region Public Methods

        public void SwitchState(_EnemyBaseState state)
        {
            _currentState.ExitState(this);
            _currentState = state;
            _currentState.EnterState(this);        
        }

        #endregion

        #region Private Methods

        private void FlipEnemy()
        {
            Vector3 direction = transform.localScale;

            if (IsFacingLeft && transform.localScale.x < 0)
                return;

            if (!IsFacingLeft && transform.localScale.x > 0)
                return;

            if (IsFacingLeft)
            {
                direction.x = -transform.localScale.x;
            }
            else
            {
                direction.x = Mathf.Abs(transform.localScale.x);
            }

            transform.localScale = direction;
        }

        #endregion
    }
}
