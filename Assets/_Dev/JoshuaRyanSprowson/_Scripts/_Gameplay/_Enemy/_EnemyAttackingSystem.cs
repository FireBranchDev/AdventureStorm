using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyAttackingSystem : MonoBehaviour, _IAttackCapable
    {
        #region Constant Fields

        private const string AttackAnimationTrigger = "TrAttack";

        #endregion

        #region Fields

        [Tooltip("What is the attack delay?")]
        /// <summary>
        /// What is the attack delay
        /// </summary>
        [SerializeField] private float _attackDelayInSeconds = 0.95f;

        [Tooltip("How much damage does the enemy deal?")]
        /// <summary>
        /// How much damage does the enemy deal
        /// </summary>
        [SerializeField] private float _attackDamage = 1.75f;

        private Animator _animator;

        private _EnemyAIBehaviour _aiBehaviour;

        private int _attackAnimationTriggerHash = Animator.StringToHash(AttackAnimationTrigger);

        #endregion

        #region Properties

        public bool IsInAttackRange => _aiBehaviour.IsInCombatInteractionRange;

        public bool IsAttacking => _aiBehaviour.IsAttacking;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _aiBehaviour = GetComponent<_EnemyAIBehaviour>();
            StartCoroutine(AttackCoroutine());
        }

        #endregion

        #region Public Methods

        public void AttackFinished()
        {
            if (_aiBehaviour.IsInCombatInteractionRange && _aiBehaviour.IsAttacking)
            {
                GameObject player;
                if (_aiBehaviour.IsFacingLeft)
                {
                    player = _aiBehaviour.CastRaycastToCollideWithPlayer(-Vector2.right);  
                }
                else
                {
                    player = _aiBehaviour.CastRaycastToCollideWithPlayer(Vector2.right);
                }

                if (player != null)
                {
                    _IDamageable damageable = player.GetComponent<_IDamageable>();
                    damageable?.Damage(_attackDamage);
                }

                _aiBehaviour.IsAttackFinished = true;
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator AttackCoroutine()
        {
            for(;;)
            {
                if (_aiBehaviour.IsInCombatInteractionRange && _aiBehaviour.IsAttacking)
                {
                    Attack();
                    yield return new WaitForSecondsRealtime(_attackDelayInSeconds);
                }
                else
                {
                    _animator.ResetTrigger(_attackAnimationTriggerHash);
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private void Attack()
        {
            _aiBehaviour.IsAttackFinished = false;
            _animator.SetTrigger(_attackAnimationTriggerHash);
        }

        #endregion
    }
}
