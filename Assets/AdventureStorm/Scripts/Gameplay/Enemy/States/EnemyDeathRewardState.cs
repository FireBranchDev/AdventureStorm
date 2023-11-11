using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyDeathRewardState : EnemyBaseState
    {
        #region Fields

        private const string _DynamicGameObjectName = "_Dynamic";

        private PlayerStateManager _playerStateManager;

        private bool _rewardGiven;

        private Coroutine _deleteEnemyCoroutine;

        #endregion

        #region Constructors

        public EnemyDeathRewardState()
        {
            _playerStateManager = null;

            _rewardGiven = false;

            _deleteEnemyCoroutine = null;

            HasKey = false;
        }

        #endregion

        #region Properties

        public bool HasKey { get; set; }

        #endregion

        #region Public Methods

        public override void EnterState(EnemyStateManager enemy)
        {

        }

        public override void ExitState(EnemyStateManager enemy)
        {
            if (_deleteEnemyCoroutine != null)
            {
                enemy.StopCoroutine(_deleteEnemyCoroutine);
                _deleteEnemyCoroutine = null;
            }
        }

        public override void FixedUpdateState(EnemyStateManager enemy)
        {

        }

        public override void UpdateState(EnemyStateManager enemy)
        {
            if (_playerStateManager == null)
            {
                var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;

                RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, float.MaxValue, enemy.PlayerLayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<PlayerStateManager>(out var player))
                    {
                        _playerStateManager = player;
                    }
                }
            }

            if (_playerStateManager != null && !_rewardGiven)
            {
                float result = Random.Range(1f, 100f);

                if (result <= 75f)
                {
                    float playerHealth = _playerStateManager.Health;
                    _playerStateManager.Heal(playerHealth * 0.25f);
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

        #endregion

        #region Private Methods

        private IEnumerator DeleteEnemyCoroutine(EnemyStateManager enemy)
        {
            yield return new WaitForSeconds(0.2f);
            Object.Destroy(enemy.gameObject);
        }

        #endregion
    }
}
