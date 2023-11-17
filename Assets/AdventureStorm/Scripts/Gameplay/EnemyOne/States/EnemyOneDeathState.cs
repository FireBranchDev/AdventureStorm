using AdventureStorm.Gameplay.Enemy.States;
using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneDeathState : EnemyBaseState<EnemyOneStateManager>
    {
        #region Constant Fields

        private const string DyingAnimation = "Dying";

        private const string _DynamicGameObjectName = "_Dynamic";

        #endregion

        #region Fields

        private Coroutine _deathCoroutine;

        private bool _rewardGiven;

        private Coroutine _deleteEnemyCoroutine;

        #endregion

        #region Constructors

        public EnemyOneDeathState()
        {
            _deathCoroutine = null;

            _rewardGiven = false;

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

            if (!_rewardGiven)
            {
                PlayerStateManager playerStateManager = null;

                RaycastHit2D left = Physics2D.Raycast(enemy.transform.position, Vector2.left, float.MaxValue, enemy.PlayerLayerMask);
                if (left.collider != null)
                {
                    if (left.collider.TryGetComponent<PlayerStateManager>(out var player))
                    {
                        playerStateManager = player;
                    }
                }

                RaycastHit2D right = Physics2D.Raycast(enemy.transform.position, Vector2.right, float.MaxValue, enemy.PlayerLayerMask);
                if (right.collider != null)
                {
                    if (right.collider.TryGetComponent<PlayerStateManager>(out var player))
                    {
                        playerStateManager = player;
                    }
                }

                if (playerStateManager != null)
                {
                    float result = Random.Range(1f, 100f);

                    if (result <= 75f)
                    {
                        float playerHealth = playerStateManager.Health;
                        playerStateManager.Heal(playerHealth * 0.25f);
                    }

                    if (HasKey)
                    {
                        var keySpawnPosition = Vector3.zero;

                        keySpawnPosition.x = enemy.transform.position.x + 2.5f;
                        keySpawnPosition.y = enemy.transform.position.y + 1.5f;

                        GameObject key = Object.Instantiate(enemy.KeyPrefab, keySpawnPosition, Quaternion.Euler(0, 0, 90));

                        key.transform.parent = GameObject.Find(_DynamicGameObjectName).transform;
                    }

                    _rewardGiven = true;

                    _deleteEnemyCoroutine = enemy.StartCoroutine(DeleteEnemyCoroutine(enemy));
                }
            }
        }

        private IEnumerator DeleteEnemyCoroutine(EnemyOneStateManager enemy)
        {
            yield return new WaitForSeconds(0.2f);
            Object.Destroy(enemy.gameObject);
        }

        #endregion
    }
}
