using UnityEngine;

namespace AdventureStorm
{
        /// <summary>
        /// Handles player's input
        /// </summary>
    public class _PlayerCharacter : MonoBehaviour
    {
        #region Fields
        [Tooltip("How fast the character moves")]
            /// <summary>
            /// How fast the character moves
            /// </summary>
        public float MovementSpeed = 5f;
        
        private Animator _animator;

        #endregion

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput != 0)
            {
                Walk();
            }
            else if (Input.GetMouseButton(0))
            {
                Attack();
            }
            else
            {
                _animator.SetTrigger("TrIdle");
            }
        }

        /// <summary>
        /// Character is able to walk left and right
        /// </summary>
        private void Walk()
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            _animator.SetTrigger("TrWalking");
            Vector3 movement = new(MovementSpeed * horizontalInput, 0);

            movement *= Time.deltaTime;

            transform.Translate(movement);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Attack()
        {
            _animator.SetTrigger("TrAttacking");
        }
    }
}