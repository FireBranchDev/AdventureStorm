using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyAttackingSystem : MonoBehaviour, _IAttackCapable
    {
        #region Constant Fields

        private const string AttackAnimation = "Attack";

        private const string IdleAnimation = "Idle";

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

        private _AnimatorManager _animatorManager;

        private _EnemyAIBehaviour _aiBehaviour;

        private _EnemyHealthSystem _healthSystem;

        #endregion

        #region Properties

        public bool IsInAttackRange => _aiBehaviour.IsInCombatInteractionRange;

        public bool IsAttacking => _aiBehaviour.IsAttacking;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animatorManager = GetComponent<_AnimatorManager>();
            _aiBehaviour = GetComponent<_EnemyAIBehaviour>();
            _healthSystem = GetComponent<_EnemyHealthSystem>();
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
            }

            _aiBehaviour.IsAttackFinished = true;
            _animatorManager.ChangeAnimationState(IdleAnimation);
        }

        #endregion

        #region Private Methods

        private IEnumerator AttackCoroutine()
        {
            for (; ; )
            {
                if (_healthSystem.IsAlive && _aiBehaviour.IsInCombatInteractionRange && _aiBehaviour.IsAttacking)
                {
                    Attack();
                    yield return new WaitForSecondsRealtime(_attackDelayInSeconds);
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Attack()
        {
            _aiBehaviour.IsAttackFinished = false;
            _animatorManager.ChangeAnimationState(AttackAnimation);
        }

        #endregion
    }
}
