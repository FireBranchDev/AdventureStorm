using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerHealthSystem : MonoBehaviour, _IDamageable
    {
        #region Fields

        [Tooltip("How much health does the player have?")]
        /// <summary>
        /// How much health does the player have?
        /// </summary>
        [SerializeField] private float _health = 5f;

        #endregion

        #region Properties

        public bool IsAlive => _health > 0;

        public float Health => _health;

        #endregion

        #region LifeCycle

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

        #endregion

        #region Private Methods

        private void Dead()
        {
            if (IsAlive)
            {
                return;
            }

            Destroy(gameObject);
        }

        #endregion
    }
}
