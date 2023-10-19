using UnityEngine;

namespace AdventureStorm
{
    public abstract class _EnemyBaseState
    {
        public abstract void EnterState(_EnemyStateManager enemy);

        public abstract void OnTriggerEnter2D(_EnemyStateManager enemy, Collider2D collision);

        public abstract void UpdateState(_EnemyStateManager enemy);
    }
}
