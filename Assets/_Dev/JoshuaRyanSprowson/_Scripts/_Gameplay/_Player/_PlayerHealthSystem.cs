using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerHealthSystem : MonoBehaviour, _IDamageable
    {
        #region Constant Fields

        private const string DyingAnimationTrigger = "TrDying";

        #endregion

        #region Fields

        [Tooltip("How much health does the player have?")]
        /// <summary>
        /// How much health does the player have?
        /// </summary>
        [SerializeField] private float _health = 5f;

        private Animator _animator;

        private readonly int _dyingAnimationTriggerHash = Animator.StringToHash(DyingAnimationTrigger);

        #endregion

        #region Properties

        public bool IsAlive => _health > 0;

        public float Health => _health;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!IsAlive)
            {
                Dead();
            }
        }

        #endregion

        #region Public Methods

        public void Damage(float damage)
        {
            if (Health - damage > 0)
            {
                _health -= damage;
            }
            else
            {
                _health = 0;
            }
        }

        /// <summary>
        /// Method to be executed once the dying animation has finished
        /// </summary>
        public void FinishedDyingAnimation()
        {
            Destroy(gameObject);
        }

        #endregion

        #region Private Methods

        private void Dead()
        {
            if (IsAlive)
            {
                return;
            }

            _animator.SetTrigger(_dyingAnimationTriggerHash);
        }

        #endregion
    }
}
