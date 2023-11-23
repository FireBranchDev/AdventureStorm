using AdventureStorm.Gameplay.Enemy;
using AdventureStorm.Gameplay.EnemyOne;
using UnityEngine;

namespace AdventureStorm.Systems
{
    /// <summary>
    /// Select a random enemy to drop the level exit key.
    /// </summary>
    public class RandomEnemyWithKey : MonoBehaviour
    {
        #region Constant Fields

        private const string EnemyTag = "Enemy";

        #endregion

        #region Fields

        [Tooltip("Key to be used when the random enemy is killed!")]
        public GameObject Key;

        #endregion

        #region LifeCycle

        private void Start()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

            if (enemies.Length > 0)
            {
                int result = Random.Range(1, enemies.Length + 1);

                GameObject enemy = enemies[result - 1];

                if (enemy.TryGetComponent<EnemyStateManager>(out var enemyStateManager))
                {
                    enemyStateManager.HasKey = true;
                }
            }
        }

        #endregion
    }
}
