using UnityEngine;

namespace AdventureStorm
{
    public class _EnemyMovingState : _EnemyBaseState
    {
        public override void EnterState(_EnemyStateManager enemy)
        {
            
        }

        public override void OnTriggerEnter2D(_EnemyStateManager enemy, Collider2D collision)
        {
            Debug.Log("Player has entered the moving trigger");
        }

        public override void UpdateState(_EnemyStateManager enemy)
        {
            
        }
    }
}
