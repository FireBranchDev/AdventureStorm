using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyDeathState : _EnemyBaseState
    {
        public override void EnterState(_EnemyStateManager enemy)
        {
            Object.Destroy(enemy.gameObject);
        }

        public override void ExitState(_EnemyStateManager enemy)
        {

        }

        public override void FixedUpdateState(_EnemyStateManager enemy)
        {

        }

        public override void UpdateState(_EnemyStateManager enemy)
        {

        }
    }
}
