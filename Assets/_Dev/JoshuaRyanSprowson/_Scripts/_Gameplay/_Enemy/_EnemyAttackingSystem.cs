using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyAttackingSystem : MonoBehaviour, _IAttackCapable
    {
        #region Constant Fields

        private const string PlayerTag = "Player";

        private const string AttackAnimationTrigger = "TrAttack";

        #endregion

        #region Fields

        [Tooltip("What is the range of the attack?")]
        /// <summary>
        /// What is the range of the attack
        /// </summary>
        [SerializeField] private float _attackRange = 2.5f;

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

        private bool _facingLeft = true;

        private bool _isInAttackRange;

        private bool _isAttacking;

        private int _attackAnimationTriggerHash = Animator.StringToHash(AttackAnimationTrigger);

        private GameObject _player;

        #endregion

        #region Properties

        public bool IsInAttackRange => _isInAttackRange;

        public bool IsAttacking => _isAttacking;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(CheckPlayerInAttackRangeCoroutine());
            StartCoroutine(AttackCoroutine());
        }

        private void Update()
        {
            _facingLeft = transform.localScale.x < 0;
            CheckPlayerInAttackRange();
        }

        #endregion

        #region Public Methods

        public void AttackFinished()
        {
            if (IsInAttackRange)
            {
                if (_player != null)
                {
                    var component = _player.GetComponent<_IDamageable>();
                    component?.Damage(_attackDamage);
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator CheckPlayerInAttackRangeCoroutine()
        {
            for (;;)
            {
                CheckPlayerInAttackRange();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator AttackCoroutine()
        {
            for(;;)
            {
                if (IsInAttackRange)
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

        private void CheckPlayerInAttackRange()
        {
            if (_facingLeft)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag(PlayerTag))
                    {
                        float distance = Mathf.Abs(transform.position.x - hit.collider.transform.position.x);
                        _isInAttackRange = distance <= _attackRange;

                        if (IsInAttackRange && _player == null)
                        {
                            _player = hit.collider.gameObject;
                        }
                    }
                }
                else
                {
                    _isInAttackRange = false;
                }
            }
        }

        private void Attack()
        {
            _animator.SetTrigger(_attackAnimationTriggerHash);
        }

        #endregion
    }
}
