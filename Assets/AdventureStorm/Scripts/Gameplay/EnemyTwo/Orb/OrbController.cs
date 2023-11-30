using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyTwo.Orb
{
    public class OrbController : MonoBehaviour
    {
        #region Fields

        private GameObject _player;

        private IDamageable _playerDamageable;

        private IController _enemyController;

        private float _speed = 4.25f;

        private float _damage = 1.25f;

        private Vector3 _direction;

        private float _existenceDuration = 0.9f;

        #endregion

        #region Properties

        public GameObject Enemy { get; set; }

        #endregion

        #region Events / Delegates

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                _playerDamageable.Damage(_damage);
                Destroy(gameObject);
            }
        }

        #endregion

        #region LifeCycle

        private void Start()
        {
            if (_player == null)
            {
                _player = GameObject.Find("Player");
            }

            if (_player != null)
            {
                _direction = _player.transform.position - transform.position;
                _direction.y = 0;
                _direction.z = 0;

                _playerDamageable = _player.GetComponent<IDamageable>();
            }

            if (Enemy != null)
            {
                _enemyController = Enemy.GetComponent<IController>();
            }

            StartCoroutine(DestroyCoroutine());
        }

        private void Update()
        {
            if (_enemyController != null)
            {
                if (!_enemyController.IsAlive)
                {
                    Destroy(gameObject);
                }
            }

            transform.Translate(_direction * Time.deltaTime * _speed);
        }

        #endregion

        #region Private Methods

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(_existenceDuration);

            Destroy(gameObject);
        }

        #endregion
    }
}