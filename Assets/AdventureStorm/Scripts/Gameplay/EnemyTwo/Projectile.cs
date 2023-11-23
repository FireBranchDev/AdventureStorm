using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyTwo
{
    public class Projectile : MonoBehaviour
    {
        private const float Speed = 4f;
        private const float LaunchHeight = 2f;

        [HideInInspector]
        public GameObject Enemy;

        private GameObject _target;

        private float _targetX;
        private float _enemyX;

        private float _distance;

        private float _nextX;

        private float _baseY;

        private float _height;

        private Vector3 _movePosition;

        private void Start()
        {
            _target = GameObject.FindGameObjectWithTag("Player");

            if (_target != null)
            {
                _targetX = _target.transform.position.x;
            }

            if (Enemy != null)
            {
                transform.position = Enemy.transform.position;
            }
        }

        private void Update()
        {
            _enemyX = Enemy.transform.position.x;

            _distance = _enemyX - _targetX;

            _nextX = Mathf.MoveTowards(transform.position.x, _targetX, Speed * Time.deltaTime);
            _baseY = Mathf.Lerp(Enemy.transform.position.y, _target.transform.position.y, (_nextX - _targetX) / _distance);
            _height = LaunchHeight * (_nextX - _targetX) * (_nextX - _enemyX) / (-0.25f * _distance * _distance);

            _movePosition = new Vector3(_nextX, _baseY + _height, transform.position.z);

            transform.SetPositionAndRotation(_movePosition, LookAtTarget(_movePosition - transform.position));

            if (transform.position.y <= _baseY)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                if (Enemy.TryGetComponent<StateManager>(out var stateManager))
                {
                    if (_target.TryGetComponent<PlayerStateManager>(out var playerStateManager))
                    {
                        playerStateManager.Damage(stateManager.AttackDamage);
                    }
                }
                Destroy(gameObject);
            }
        }

        private Quaternion LookAtTarget(Vector2 r)
        {
            return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
        }
    }
}
