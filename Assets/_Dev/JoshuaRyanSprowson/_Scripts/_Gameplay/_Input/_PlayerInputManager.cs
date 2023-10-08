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
        #endregion

        #region Properties
        public bool IsMoving { get; private set; }
        public bool IsMovingLeft { get; private set; }
        public bool IsMovingRight { get; private set; }

        public bool IsAttacking { get; private set; }
        #endregion

        private void Update()
        {
            float horizontalAxis = Input.GetAxis(Horizontal);
            IsMoving = horizontalAxis != 0;
            IsMovingLeft = horizontalAxis < 0;
            IsMovingRight = horizontalAxis > 0;
            IsAttacking = !IsMoving && Input.GetMouseButtonDown(attackMouseButton);
        }
    }
}
