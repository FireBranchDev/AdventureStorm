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

        private _EnemyAttackingSystem _enemyAttackingSystem;

        [Tooltip("How fast does the enemy move?")]
        /// <summary>
        /// How fast does the enemy move?
        /// </summary>
        [SerializeField] private float _movementSpeed = 5f;

        private bool _facingLeft = true;

        private readonly int _isMovingAnimatorParameterHash = Animator.StringToHash(IsMovingAnimatorParameter);

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _enemyAttackingSystem = GetComponent<_EnemyAttackingSystem>();
        }

        private void Update()
        {
            _facingLeft = transform.localScale.x < 0;

            if (_enemyAttackingSystem != null)
            {
                if (!_enemyAttackingSystem.IsAttacking && _facingLeft)
                {
                    MovingLeft();
                }
                else
                {
                    _animator.SetBool(_isMovingAnimatorParameterHash, true);
                }
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

        #endregion
    }
}
