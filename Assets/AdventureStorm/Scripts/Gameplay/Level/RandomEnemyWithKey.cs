using UnityEngine;

namespace AdventureStorm.Gameplay.Level
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

        [Tooltip("Key is spawned when the enemy holding the key dies.")]
        public GameObject Key;

        #endregion

        #region LifeCycle

        private void Start()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
            if (enemies.Length > 0)
            {
                int randomNumber = Random.Range(0, enemies.Length - 1);
                if (enemies[randomNumber].TryGetComponent<IKeyable>(out var keyable))
                {
                    keyable.HasKey = true;
                }
            }
        }

        #endregion
    }
}