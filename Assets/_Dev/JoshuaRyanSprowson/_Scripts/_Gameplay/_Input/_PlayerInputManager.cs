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
            
            IsMoving = horizontalAxis != 0 && !Input.GetKey(_dodgeButton);
            IsMovingLeft = horizontalAxis < 0 && IsMoving;
            IsMovingRight = horizontalAxis > 0 && IsMoving;
            
            IsAttacking = !IsMoving && Input.GetMouseButtonDown(attackMouseButton);

            IsDodging = !IsMoving && Input.GetKey(_dodgeButton);

            IsDodgingLeft = IsDodging && horizontalAxis < 0;
            IsDodgingRight = IsDodging && horizontalAxis > 0;
        }
    }
}
