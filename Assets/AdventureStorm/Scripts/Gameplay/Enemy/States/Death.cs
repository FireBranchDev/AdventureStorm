using AdventureStorm.Systems;
using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.Enemy.States
{
    public class Death : BaseState
    {
        #region Constant Fields

        private const string DyingAnimation = "Dying";

        private const string DynamicGameObjectName = "_Dynamic";

        private const float HealMultiplier = 0.4f;

        private const string PlayerTag = "Player";

        #endregion

        #region Fields

        private Coroutine _deathCoroutine;

        private Coroutine _deleteEnemyCoroutine;

        #endregion

        #region Constructors

        public Death()
        {
            _deathCoroutine = null;

            _deleteEnemyCoroutine = null;
        }

        #endregion

        #region Public Methods

        public override void EnterState(StateManager stateManager)
        {
            if (_deathCoroutine == null)
            {
                if (stateManager.TryGetComponent<EnemyStateManager>(out var enemyStateManager))
                {
                    _deathCoroutine = stateManager.StartCoroutine(DeathCoroutine(enemyStateManager));
                }
            }
        }

        public override void ExitState(StateManager stateManager)
        {
            if (_deathCoroutine != null)
            {
                stateManager.StopCoroutine(_deathCoroutine);
                _deathCoroutine = null;
            }

            if (_deleteEnemyCoroutine != null)
            {
                stateManager.StopCoroutine(_deleteEnemyCoroutine);
                _deleteEnemyCoroutine = null;
            }
        }

        public override void FixedUpdateState(StateManager stateManager)
        {

        }

        public override void UpdateState(StateManager stateManager)
        {

        }

        #endregion

        #region Private Methods

        private IEnumerator DeathCoroutine(EnemyStateManager enemy)
        {
            enemy.AnimatorManager.ChangeAnimationState(DyingAnimation);

            while (!enemy.AnimatorManager.DidAnimationFinish(DyingAnimation))
            {
                yield return null;
            }

            GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
            if (player != null)
            {
                if (player.TryGetComponent<PlayerStateManager>(out var playerStateManager))
                {
                    playerStateManager.Heal(playerStateManager.MaximumHealth * HealMultiplier);

                    if (enemy.HasKey)
                    {
                        var keySpawnPosition = Vector3.zero;

                        keySpawnPosition.x = enemy.transform.position.x + 2.5f;
                        keySpawnPosition.y = enemy.transform.position.y + 1.5f;

                        var system = GameObject.Find("@System");
                        if (system != null)
                        {
                            if (system.TryGetComponent<RandomEnemyWithKey>(out var randomEnemyWithKey))
                            {
                                GameObject key = Object.Instantiate(randomEnemyWithKey.Key, keySpawnPosition, Quaternion.Euler(0, 0, 90));
                                key.transform.parent = GameObject.Find(DynamicGameObjectName).transform;
                            }
                        }
                    }
                }
            }

            _deleteEnemyCoroutine = enemy.StartCoroutine(DeleteEnemyCoroutine(enemy));
        }

        private IEnumerator DeleteEnemyCoroutine(EnemyStateManager enemy)
        {
            yield return new WaitForSeconds(0.2f);
            Object.Destroy(enemy.gameObject);
        }

        #endregion
    }
}
