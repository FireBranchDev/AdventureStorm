using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyMovementSystem : MonoBehaviour
    {
        #region Constant Fields

        private const string MovingAnimation = "Moving";

        private const string IdleAnimation = "Idle";

        #endregion

        #region Fields

        [Tooltip("How fast does the enemy move?")]
        /// <summary>
        /// How fast does the enemy move?
        /// </summary>
        [SerializeField] private float _movementSpeed = 5f;

        private _AnimatorManager _animatorManager;

        private _EnemyAIBehaviour _aiBehaviour;

        private _EnemyHealthSystem _healthSystem;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animatorManager = GetComponent<_AnimatorManager>();
            _aiBehaviour = GetComponent<_EnemyAIBehaviour>();
            _healthSystem = GetComponent<_EnemyHealthSystem>();
        }

        private void Update()
        {
            if (_aiBehaviour != null)
            {
                if (_healthSystem.IsAlive && !_aiBehaviour.IsAttacking)
                {
                    if (_aiBehaviour.IsMovingLeft)
                    {
                        MovingLeft();
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void MovingLeft()
        {
            _animatorManager.ChangeAnimationState(MovingAnimation);

            Vector3 position = new Vector3(-_movementSpeed, 0);
            position *= Time.deltaTime;
            transform.Translate(position);
        }

        private void Idle()
        {
            _animatorManager.ChangeAnimationState(IdleAnimation);
        }

        #endregion
    }
}
