using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyStateManager : MonoBehaviour
    {
        #region Constant Fields

        private const string PlayerGameObjectName = "Player";

        #endregion

        #region Fields

        private _EnemyBaseState _currentState;

        private GameObject _player;

        #endregion

        #region Properties

        public _AnimatorManager AnimatorManager { get; private set; }

        public _EnemyIdleState IdleState { get; private set; } = new();

        #endregion

        #region LifeCycle

        // Start is called before the first frame update.
        private void Start()
        {
            _player = GameObject.Find(PlayerGameObjectName);
            Physics2D.IgnoreCollision(_player.GetComponent<CapsuleCollider2D>(), GetComponent<CapsuleCollider2D>());
            AnimatorManager = GetComponent<_AnimatorManager>();
            _currentState = IdleState;
            _currentState.EnterState(this);
        }

        // Update is called once per frame.
        private void Update()
        {
            _currentState.UpdateState(this);
        }

        #endregion

        #region Public Methods

        public void SwitchState(_EnemyBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }

        #endregion

        #region Private Methods

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _currentState.OnTriggerEnter2D(this, collision);
        }

        #endregion
    }
}
