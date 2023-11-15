using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyCombatState : EnemyBaseState
    {
        #region Constant Fields

        private const float CombatDistance = 2.5f;

        private const float DodgeChance = 0.35f;

        #endregion

        #region Constructors

        public EnemyCombatState()
        {
            AttackingState = new EnemyAttackingState();
            DodgingState = new EnemyDodgingState();
        }

        #endregion

        #region Properties

        public EnemyAttackingState AttackingState { get; private set; }

        public EnemyDodgingState DodgingState { get; private set; }

        #endregion

        #region Public Methods

        public override void EnterState(EnemyStateManager enemy)
        {

        }

        public override void ExitState(EnemyStateManager enemy)
        {

        }

        public override void FixedUpdateState(EnemyStateManager enemy)
        {
            
        }

        public override void UpdateState(EnemyStateManager enemy)
        {
            var direction = enemy.AliveState.IsFacingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, CombatDistance, enemy.PlayerLayerMask);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent<PlayerStateManager>(out var playerStateManager))
                {
                    if (playerStateManager.IsAlive)
                    {
                        float distance = Mathf.Abs(enemy.transform.position.x - playerStateManager.transform.position.x);
                        if (distance <= CombatDistance)
                        {
                            float random = Random.Range(1f, 100f);
                            if (random / 100f <= DodgeChance)
                            {
                                enemy.SwitchState(enemy.AliveState.CombatState.DodgingState);
                            }
                            else
                            {
                                enemy.SwitchState(enemy.AliveState.CombatState.AttackingState);
                            }
                        }
                        else
                        {
                            enemy.SwitchState(enemy.AliveState.MovementState);
                        }
                    }
                    else
                    {
                        enemy.SwitchState(enemy.AliveState.IdleState);
                    }
                }
            }
            else
            {
                enemy.SwitchState(enemy.AliveState.MovementState);
            }
        }

        #endregion
    }
}