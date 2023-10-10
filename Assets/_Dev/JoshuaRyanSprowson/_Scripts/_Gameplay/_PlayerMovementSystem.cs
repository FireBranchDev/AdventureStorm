using UnityEngine;

namespace AdventureStorm
{
    /// <summary>
    /// Player movement system
    /// </summary>
    public class _PlayerMovementSystem : MonoBehaviour
    {
        #region Fields

        [Tooltip("How fast the character moves")]
        /// <summary>
        /// How fast the character moves
        /// </summary>
        public float MovementSpeed = 5f;

        private Animator _animator;

        private _PlayerInputManager _playerInputManager;

        private readonly int _isIdleHash = Animator.StringToHash("IsIdle");
        private readonly int _isMovingHash = Animator.StringToHash("IsMoving");

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInputManager = GetComponent<_PlayerInputManager>();
        }

        private void Update()
        {
            if (_playerInputManager != null)
            {
                if (!_playerInputManager.IsAttacking && !_playerInputManager.IsMoving)
                {
                    Idle();
                }
                else
                {
                    if (_playerInputManager.IsMovingRight)
                    {
                        MoveRight();
                    }
                    else if (_playerInputManager.IsMovingLeft)
                    {
                        MoveLeft();
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void Idle()
        {
            _animator.SetBool(_isMovingHash, false);
            _animator.SetBool(_isIdleHash, true);
        }

        private void MoveRight()
        {
            _animator.SetBool(_isIdleHash, false);
            transform.localScale = new Vector3(1, 1, 1);
            _animator.SetBool(_isMovingHash, true);
            Vector3 movement = new(MovementSpeed, 0);
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }

        private void MoveLeft()
        {
            _animator.SetBool(_isIdleHash, false);
            transform.localScale = new Vector3(-1, 1, 1);
            _animator.SetBool(_isMovingHash, true);
            Vector3 movement = new(-MovementSpeed, 0);
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }

        #endregion
    }
}
