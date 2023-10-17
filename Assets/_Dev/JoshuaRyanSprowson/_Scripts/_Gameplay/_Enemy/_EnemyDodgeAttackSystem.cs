using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyDodgeAttackSystem : MonoBehaviour
    {
        #region Constant Fields

        private const string IsDodgingAnimation = "IsDodging";

        #endregion

        #region Fields

        [Tooltip("How far is the distance that the enemy can dodge?")]
        /// <summary>
        /// How far is the distance that the enemy can dodge?
        /// </summary>
        [SerializeField] private float _dodgeDistance = 0.75f;

        [Tooltip("What height does the enemy reach when dodging?")]
        /// <summary>
        /// What height does the enemy reach when dodging?
        /// </summary>
        [SerializeField] private float _dodgeHeight = 0.25f;

        private Animator _animator;

        private _EnemyAIBehaviour _aiBehaviour;

        private readonly int _isDodgingAnimationHash = Animator.StringToHash(IsDodgingAnimation);

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _aiBehaviour = GetComponent<_EnemyAIBehaviour>();
        }

        private void Update()
        {
            if (_aiBehaviour.IsDodging && _aiBehaviour.IsFacingLeft && _aiBehaviour.IsDodgeAttackFinished)
            {
                _aiBehaviour.IsDodgeAttackFinished = false;
                PlayDodgeAnimation();
                transform.Translate(new(_dodgeDistance, _dodgeHeight));
                StartCoroutine(DodgeFinishedCoroutine());
            }
        }

        #endregion

        #region Private Methods

        private void PlayDodgeAnimation()
        {
            _animator.SetBool(_isDodgingAnimationHash, true);
        }
        
        private IEnumerator DodgeFinishedCoroutine()
        {
            yield return new WaitForSeconds(0.3f);
            transform.Translate(new(0, -_dodgeHeight));
            _animator.SetBool(_isDodgingAnimationHash, false);
            _aiBehaviour.IsDodgeAttackFinished = true;
        }

        #endregion
    }
}
