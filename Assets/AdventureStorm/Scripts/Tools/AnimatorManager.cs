using UnityEngine;

namespace AdventureStorm.Tools
{
    /// <summary>
    /// A tool to make it simpler to play and manage animations.
    /// </summary>
    public class AnimatorManager : MonoBehaviour
    {
        #region Fields

        private string _currentState;

        #endregion

        #region Properties

        public Animator Animator { get; private set; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            Animator = GetComponent<Animator>();
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
            Animator.Play(newState);

            // Reassign the current state.
            _currentState = newState;
        }

        /// <summary>
        /// To check that the current playing animation has finished.
        /// </summary>
        /// <param name="name">The name of the animation.</param>
        /// <returns></returns>
        public bool DidAnimationFinish(string name)
        {
            AnimatorStateInfo animationState = Animator.GetCurrentAnimatorStateInfo(0);
            // Matching the name of the state and checking that the animation clip has finished.
            return animationState.IsName(name) && animationState.normalizedTime >= 1f;
        }

        /// <summary>
        /// Replays the last played animation.
        /// </summary>
        public void ReplayAnimation()
        {
            // Only allowing to play previous animations.
            if (string.IsNullOrEmpty(_currentState)) return;

            // Needs to be a different state in order for the animation to play again.
            Animator.Play(string.Empty);

            // Play the last animation played.
            Animator.Play(_currentState);
        }

        #endregion
    }
}
