using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyMovementSystem : MonoBehaviour
    {
        #region Constant Fields

        private const string IsMovingAnimatorParameter = "IsMoving";

        #endregion

        #region Fields

        private Animator _animator;

        [Tooltip("How fast does the enemy move?")]
        /// <summary>
        /// How fast does the enemy move?
        /// </summary>
        [SerializeField] private float _movementSpeed = 5f;

        private readonly int _isMovingAnimatorParameterHash = Animator.StringToHash(IsMovingAnimatorParameter);

        private _EnemyAIBehaviour _aiBehaviour;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _aiBehaviour = GetComponent<_EnemyAIBehaviour>();
        }

        private void Update()
        {
            if (_aiBehaviour != null)
            {
                if (_aiBehaviour.IsMovingLeft)
                    MovingLeft();

                if (_aiBehaviour.IsIdle)
                    Idle();
            }
        }

        #endregion

        #region Private Methods

        private void MovingLeft()
        {
            _animator.SetBool(_isMovingAnimatorParameterHash, true);
            Vector3 position = new Vector3(-_movementSpeed, 0);
            position *= Time.deltaTime;
            transform.Translate(position);
        }

        private void Idle()
        {
            _animator.SetBool(_isMovingAnimatorParameterHash, false);
        }

        #endregion
    }
}
