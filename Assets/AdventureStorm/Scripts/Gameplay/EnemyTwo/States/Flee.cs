using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyTwo.States
{
    public class Flee : BaseState
    {
        private const float MaximumFleeDistance = 3f;

        private GameObject _leftBoundary;
        private GameObject _rightBoundary;

        private float _leftBoundaryX;
        private float _rightBoundaryX;

        private float _randomX;

        private Vector3 _position;

        public override void EnterState(Gameplay.StateManager stateManager)
        {
            if (stateManager.TryGetComponent<StateManager>(out var enemyTwoStateManager))
            {
                _leftBoundary = GameObject.FindGameObjectWithTag("LeftBoundary");
                _rightBoundary = GameObject.FindGameObjectWithTag("RightBoundary");

                _leftBoundaryX = _leftBoundary.transform.position.x;
                _rightBoundaryX = _rightBoundary.transform.position.x;

                _randomX = Random.Range(-MaximumFleeDistance, MaximumFleeDistance);

                if (_randomX > _leftBoundaryX && _randomX < _rightBoundaryX)
                {
                    _position = Vector3.zero;

                    _position.x = _randomX + enemyTwoStateManager.Player.transform.position.x;

                    stateManager.transform.Translate(_position);
                }

                stateManager.SwitchState(enemyTwoStateManager.States.Idle);
            }
        }

        public override void ExitState(Gameplay.StateManager stateManager)
        {

        }

        public override void FixedUpdateState(Gameplay.StateManager stateManager)
        {

        }

        public override void UpdateState(Gameplay.StateManager stateManager)
        {
            if (stateManager.TryGetComponent<StateManager>(out var enemyTwoStateManager))
            {
                if (stateManager.IsAlive)
                {
                    enemyTwoStateManager.FacePlayer(enemyTwoStateManager);
                    enemyTwoStateManager.FlipEnemy(enemyTwoStateManager);
                }
                else
                {
                    stateManager.SwitchState(enemyTwoStateManager.States.Death);
                }
            }
        }
    }
}