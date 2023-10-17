using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    /// <summary>
    /// Handles the enemy's dodge attack game mechanic.
    /// </summary>
    public class _EnemyDodgeAttackSystem : MonoBehaviour
    {
        #region Constant Fields

        private const string DodgeAnimation = "Dodge";
        private const string IdleAnimation = "Idle";

        #endregion

        #region Fields

        [Tooltip("How far is the distance that the enemy can dodge?")]
        /// <summary>
        /// How far is the distance that the enemy can dodge?
        /// </summary>
        [SerializeField] private float _dodgeDistance = 1.5f;

        [Tooltip("What height does the enemy reach when dodging?")]
        /// <summary>
        /// What height does the enemy reach when dodging?
        /// </summary>
        [SerializeField] private float _dodgeHeight = 0.25f;

        private _EnemyAIBehaviour _aiBehaviour;

        private _AnimatorManager _animatorManager;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _aiBehaviour = GetComponent<_EnemyAIBehaviour>();
            _animatorManager = GetComponent<_AnimatorManager>();
        }

        private void Update()
        {
            if (_aiBehaviour.IsDodging && _aiBehaviour.IsDodgeAttackFinished)
            {
                _animatorManager.ChangeAnimationState(DodgeAnimation);
                _aiBehaviour.IsDodgeAttackFinished = false;

                if (_aiBehaviour.IsFacingLeft)
                {   
                    transform.Translate(new(_dodgeDistance, _dodgeHeight));
                    StartCoroutine(DodgeFinishedCoroutine());
                }
                else
                {
                    transform.Translate(new(-_dodgeDistance, _dodgeHeight));
                    StartCoroutine(DodgeFinishedCoroutine());
                }
            }
        }

        #endregion

        #region Private Methods
        
        private IEnumerator DodgeFinishedCoroutine()
        {
            yield return new WaitForSeconds(0.3f);
            transform.Translate(new(0, -_dodgeHeight));
            _animatorManager.ChangeAnimationState(IdleAnimation);
            _aiBehaviour.IsDodgeAttackFinished = true;
        }

        #endregion
    }
}
