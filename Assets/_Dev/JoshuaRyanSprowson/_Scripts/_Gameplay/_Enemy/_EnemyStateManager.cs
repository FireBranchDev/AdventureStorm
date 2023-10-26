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

        public _EnemyAliveState AliveState { get; private set; }

        public _EnemyDeathState DeathState { get; private set; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            RB2D = GetComponent<Rigidbody2D>();

            AnimatorManager = GetComponent<_AnimatorManager>();

            PlayerLayerMask = LayerMask.GetMask(PlayerLayerMaskName);

            AliveState = new _EnemyAliveState();
        }

        // Start is called before the first frame update.
        private void Start()
        {
            _currentState = AliveState;
            _currentState.EnterState(this);
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
        }

        // Update is called once per frame.
        private void Update()
        {
            if (AliveState.Health > 0f)
            {
                AliveState.FacePlayer(this);
                AliveState.FlipEnemy(this);
            }

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
    }
}
