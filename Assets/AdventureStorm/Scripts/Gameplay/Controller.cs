using AdventureStorm.Tools;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public abstract class Controller : MonoBehaviour, IController
    {
        #region Fields

        protected float _health;

        private GameObject _leftBoundary;
        private GameObject _rightBoundary;

        private Vector3 _position;

        #endregion

        #region Properties

        public AnimatorManager AnimatorManager { get; private set; }

        public GameObject Player { get; private set; }

        public float Health => _health;

        public bool IsAlive => Health > 0;
        public abstract float MovementSpeed { get; }

        public virtual float MaxHealth => 5f;

        public virtual float AttackDistance => 1.8f;

        public virtual float AttackDamage => 1.25f;

        #endregion

        #region LifeCycle

        protected virtual void Start()
        {
            AnimatorManager = GetComponent<AnimatorManager>();
            Player = GameObject.Find("Player");
            _leftBoundary = GameObject.FindGameObjectWithTag("LeftBoundary");
            _rightBoundary = GameObject.FindGameObjectWithTag("RightBoundary");
            _health = MaxHealth;
        }

        protected virtual void Update()
        {
            if (_leftBoundary != null)
            {
                if (transform.position.x <= _leftBoundary.transform.position.x)
                {
                    _position = transform.position;
                    _position.x = _leftBoundary.transform.position.x;
                    transform.position = _position;
                }
            }

            if (_rightBoundary != null)
            {
                if (transform.position.x >= _rightBoundary.transform.position.x)
                {
                    _position = transform.position;
                    _position.x = _rightBoundary.transform.position.x;
                    transform.position = _position;
                }
            }
        }

        protected virtual void FixedUpdate()
        {

        }

        #endregion
    }
}