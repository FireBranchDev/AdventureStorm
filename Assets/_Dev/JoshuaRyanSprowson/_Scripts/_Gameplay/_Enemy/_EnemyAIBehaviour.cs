using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyAIBehaviour : MonoBehaviour
    {
        #region Constant Fields

        private const string PlayerTag = "Player";

        #endregion

        #region Fields

        [Tooltip("What is the dodge chance?")]
        /// <summary>
        /// What is the dodge chance?
        /// </summary>
        [SerializeField] private float _dodgeChance = 0.33f;

        [Tooltip("What is the range for combat interaction?")]
        /// <summary>
        /// What is the range for combat interaction?
        /// </summary>
        [SerializeField] private float _combatInteractionRange = 2.5f;

        [Tooltip("What is the minimum inclusive to determine the combat interaction?")]
        /// <summary>
        /// What is the minimum inclusive to determine the combat interaction?
        /// </summary>
        [SerializeField] private float _minimumInclusiveToDetermineCombatInteraction = 0f;

        [Tooltip("What is the maximum inclusive to determine the combat interaction?")]
        /// <summary>
        /// What is the maximum inclusive to determine the combat interaction?
        /// </summary>
        [SerializeField] private float _maximumInclusiveToDetermineCombatInteraction = 100f;

        #endregion

        #region Properties

        public bool IsFacingLeft { get; private set; }

        public bool IsMoving { get; private set; }

        public bool IsMovingLeft { get; private set; }

        public bool IsAttacking { get; private set; }
        public bool IsAttackingLeft { get; private set; }
        public bool IsAttackingRight { get; private set; }

        public bool IsAttackFinished { get; set; } = true;

        public bool IsDodging { get; private set; }
        public bool IsDodgingLeft { get; private set; }
        public bool IsDodgingRight { get; private set; }

        public bool IsDodgeAttackFinished { get; set; } = true;

        public bool IsIdle { get; private set; }

        public bool IsInCombatInteractionRange { get; private set; }

        #endregion

        #region LifeCycle

        private void Start()
        {
            StartCoroutine(CheckIsPlayerInRangeCoroutine());
        }

        private void Update()
        {
            IsFacingLeft = transform.localScale.x < 0;

            IsMoving = !IsAttacking && !IsDodging && IsDodgeAttackFinished && IsAttackFinished;

            IsMovingLeft = IsFacingLeft && IsMoving;

            if (IsInCombatInteractionRange)
            {
                if (IsAttackFinished && IsDodgeAttackFinished)
                {
                    var combatInteractionRNG = Random.Range(_minimumInclusiveToDetermineCombatInteraction, _maximumInclusiveToDetermineCombatInteraction);
                    IsDodging = combatInteractionRNG <= _dodgeChance;
                    IsAttacking = !IsDodging;

                    if (IsFacingLeft)
                    {
                        IsDodgingLeft = IsDodging;
                        IsAttackingLeft = IsAttacking;
                    }
                    else
                    {
                        IsDodgingRight = IsDodging;
                        IsAttackingRight = IsAttacking;
                    }
                }
            }
            else
            {
                IsAttacking = IsDodging = false;
            }

            IsIdle = !IsMoving && !IsAttacking && !IsDodging && IsDodgeAttackFinished && IsAttackFinished;
        }

        #endregion

        #region Public Methods

        public GameObject CastRaycastToCollideWithPlayer(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

            if (hit.collider == null)
            {
                return null;
            }

            if (hit.collider.CompareTag(PlayerTag))
            {
                return hit.collider.gameObject;
            }

            return null;
        }

        #endregion 

        #region Private Methods

        private IEnumerator CheckIsPlayerInRangeCoroutine()
        {
            for (; ; )
            {
                IsInCombatInteractionRange = CheckIsPlayerInRange();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private bool CheckIsPlayerInRange()
        {
            GameObject player;

            if (IsFacingLeft)
            {
                player = CastRaycastToCollideWithPlayer(-Vector2.right);
            }
            else
            {
                player = CastRaycastToCollideWithPlayer(Vector2.right);
            }

            if (player)
            {
                var distance = Mathf.Abs(player.transform.position.x - transform.position.x);
                return distance <= _combatInteractionRange;
            }

            return false;
        }

        #endregion
    }
}
