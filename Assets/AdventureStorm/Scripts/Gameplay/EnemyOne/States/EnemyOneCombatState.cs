using AdventureStorm.Gameplay.Enemy.States;
using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneCombatState : EnemyBaseState<EnemyOneStateManager>
    {
        #region Constant Fields

        private const float CombatDistance = 2.5f;

        private const float DodgeChance = 0.35f;

        #endregion

        #region Constructors

        public EnemyOneCombatState()
        {
            AttackingState = new EnemyOneAttackingState();
            DodgingState = new EnemyOneDodgingState();
        }

        #endregion

        #region Properties

        public EnemyOneAttackingState AttackingState { get; private set; }

        public EnemyOneDodgingState DodgingState { get; private set; }

        #endregion

        #region Public Methods

        public override void EnterState(EnemyOneStateManager enemy)
        {

        }

        public override void ExitState(EnemyOneStateManager enemy)
        {

        }

        public override void FixedUpdateState(EnemyOneStateManager enemy)
        {

        }

        public override void UpdateState(EnemyOneStateManager enemy)
        {
            var direction = enemy.IsFacingLeft ? Vector2.left : Vector2.right;
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