using UnityEngine;

namespace AdventureStorm
{
    /// <summary>
    /// In charge of making sure the animations are being played
    /// </summary>
    public class _AnimatorManager : MonoBehaviour
    {
        #region Fields

        private Animator _animator;
        private string _currentState;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles changing animation state so that it can be played.
        /// </summary>
        /// <param name="newState">The new state to play next.</param>
        public void ChangeAnimationState(string newState)
        {
            // Stop the same animation from interrupting itself.
            if (_currentState == newState) return;

            // Play the animation.
            _animator.Play(newState);

            // Reassign the current state.
            _currentState = newState;
        }

        #endregion
    }
}
