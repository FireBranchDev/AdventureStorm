using AdventureStorm.Gameplay.Enemy.States;
using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneDeathState : EnemyBaseState<EnemyOneStateManager>
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

        public EnemyOneDeathState()
        {
            _deathCoroutine = null;

            _deleteEnemyCoroutine = null;

            HasKey = false;
        }

        #endregion

        #region Properties

        public bool HasKey { get; set; }

        #endregion

        #region Public Methods

        public override void EnterState(EnemyOneStateManager enemy)
        {
            if (_deathCoroutine == null)
            {
                _deathCoroutine = enemy.StartCoroutine(DeathCoroutine(enemy));
            }
        }

        public override void ExitState(EnemyOneStateManager enemy)
        {
            if (_deathCoroutine != null)
            {
                enemy.StopCoroutine(_deathCoroutine);
                _deathCoroutine = null;
            }

            if (_deleteEnemyCoroutine != null)
            {
                enemy.StopCoroutine(_deleteEnemyCoroutine);
                _deleteEnemyCoroutine = null;
            }
        }

        public override void FixedUpdateState(EnemyOneStateManager enemy)
        {

        }

        public override void UpdateState(EnemyOneStateManager enemy)
        {

        }

        #endregion

        #region Private Methods

        private IEnumerator DeathCoroutine(EnemyOneStateManager enemy)
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
                    playerStateManager.Heal(PlayerStateManager.MaximumHealth * HealMultiplier);

                    if (HasKey)
                    {
                        var keySpawnPosition = Vector3.zero;

                        keySpawnPosition.x = enemy.transform.position.x + 2.5f;
                        keySpawnPosition.y = enemy.transform.position.y + 1.5f;

                        GameObject key = Object.Instantiate(enemy.KeyPrefab, keySpawnPosition, Quaternion.Euler(0, 0, 90));

                        key.transform.parent = GameObject.Find(DynamicGameObjectName).transform;
                    }
                }
            }

            _deleteEnemyCoroutine = enemy.StartCoroutine(DeleteEnemyCoroutine(enemy));
        }

        private IEnumerator DeleteEnemyCoroutine(EnemyOneStateManager enemy)
        {
            yield return new WaitForSeconds(0.2f);
            Object.Destroy(enemy.gameObject);
        }

        #endregion
    }
}
