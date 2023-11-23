using AdventureStorm.Tools;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public abstract class StateManager : MonoBehaviour, IStateManager
    {
        #region Constant Fields

        public const string PlayerTag = "Player";

        public const string PlayerLayerMaskName = "Player";

        #endregion

        #region Fields

        private BaseState _currentState;

        #endregion

        #region Properties

        public Rigidbody2D Rb2D { get; private set; }

        public AnimatorManager AnimatorManager { get; private set; }

        public bool IsFacingLeft { get; set; }

        public LayerMask PlayerLayerMask { get; private set; }

        public bool IsAlive { get => Health > 0; }
        public float Health { get; set; }

        public abstract float MaximumHealth { get; }

        protected StateManager Self { get; set; }

        protected BaseState CurrentState => _currentState;

        #endregion

        #region LifeCycle

        protected virtual void Awake()
        {
            IsFacingLeft = true;
            Health = MaximumHealth;
            Rb2D = GetComponent<Rigidbody2D>();
            AnimatorManager = GetComponent<AnimatorManager>();
            PlayerLayerMask = LayerMask.GetMask(PlayerLayerMaskName);
        }

        protected virtual void Start()
        {

        }

        protected virtual void FixedUpdate()
        {
            if (_currentState != null && Self != null)
            {
                _currentState.FixedUpdateState(Self);
            }
        }

        protected virtual void Update()
        {
            if (_currentState != null && Self != null)
            {
                _currentState.UpdateState(Self);
            }
        }

        #endregion

        #region Public Methods

        public void Damage(float damage)
        {
            if (IsAlive)
            {
                Health -= damage;
            }
        }

        public void SwitchState(BaseState state)
        {
            if (state != null)
            {
                if (_currentState != null)
                {
                    _currentState.ExitState(Self);
                }

                _currentState = state;
                _currentState.EnterState(Self);
            }
        }

        #endregion
    }
}
