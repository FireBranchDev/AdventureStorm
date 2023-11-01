using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyDeathRewardState : _EnemyBaseState
    {
        #region Fields

        private _PlayerStateManager _playerStateManager;

        private bool _rewardGiven;

        private Coroutine _deleteEnemyCoroutine;

        #endregion

        #region Constructors

        public _EnemyDeathRewardState()
        {
            _playerStateManager = null;

            _rewardGiven = false;

            _deleteEnemyCoroutine = null;
        }

        #endregion

        #region Public Methods

        public override void EnterState(_EnemyStateManager enemy)
        {
            Debug.Log("Enemy death reward state");
        }

        public override void ExitState(_EnemyStateManager enemy)
        {
            if (_deleteEnemyCoroutine != null)
            {
                enemy.StopCoroutine(_deleteEnemyCoroutine);
                _deleteEnemyCoroutine = null;
            }
        }

        public override void FixedUpdateState(_EnemyStateManager enemy)
        {
            
        }

        public override void UpdateState(_EnemyStateManager enemy)
        {
            if (_playerStateManager == null)
            {
                var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;

                RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, float.MaxValue, enemy.PlayerLayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<_PlayerStateManager>(out var player))
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

                _rewardGiven = true;

                _deleteEnemyCoroutine = enemy.StartCoroutine(DeleteEnemyCoroutine(enemy));
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator DeleteEnemyCoroutine(_EnemyStateManager enemy)
        {
            yield return new WaitForSeconds(0.2f);
            Object.Destroy(enemy.gameObject);
        }

        #endregion
    }
}
