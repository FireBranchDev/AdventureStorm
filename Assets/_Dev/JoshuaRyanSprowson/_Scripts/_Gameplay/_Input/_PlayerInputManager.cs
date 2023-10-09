using UnityEngine;

namespace AdventureStorm
{
    /// <summary>
    /// Handles player's input
    /// </summary>
    public class _PlayerInputManager : MonoBehaviour
    {
        #region Constant Fields
        private const string Horizontal = "Horizontal";
        #endregion

        #region Fields
        [Tooltip("What mouse button to use for attacking?")]
        /// <summary>
        /// What mouse button to use for attacking
        /// </summary>
        [SerializeField] private int attackMouseButton = 0;

        [Tooltip("What button to use for dodging?")]
        /// <summary>
        /// What button to use for dodging
        /// </summary>
        [SerializeField] private KeyCode _dodgeButton = KeyCode.Space;
        #endregion

        #region Properties
        public bool IsDirectionKeysPressed { get; private set; }
        
        public bool IsMoving { get; private set; }
        public bool IsMovingLeft { get; private set; }
        public bool IsMovingRight { get; private set; }

        public bool IsAttacking { get; private set; }

        public bool IsDodging { get; private set; }
        public bool IsDodgingLeft { get; private set; }
        public bool IsDodgingRight { get; private set; }
        #endregion

        private void Update()
        {
            float horizontalAxis = Input.GetAxis(Horizontal);

            IsDirectionKeysPressed = horizontalAxis != 0;

            IsDodging = !IsMoving && !IsAttacking && IsDirectionKeysPressed && Input.GetKey(_dodgeButton);
            IsMoving = !IsAttacking && !IsDodging && IsDirectionKeysPressed;
            IsAttacking = !IsDodging && !IsMoving && Input.GetMouseButtonDown(attackMouseButton);

            IsDodgingLeft = !IsMoving && !IsAttacking && IsDodging && horizontalAxis < 0;
            IsDodgingRight = !IsMoving && !IsAttacking && IsDodging && horizontalAxis > 0;

            IsMovingLeft = !IsDodging && !IsAttacking && IsMoving && horizontalAxis < 0;
            IsMovingRight = !IsDodging && !IsAttacking && IsMoving && horizontalAxis > 0;
        }
    }
}
