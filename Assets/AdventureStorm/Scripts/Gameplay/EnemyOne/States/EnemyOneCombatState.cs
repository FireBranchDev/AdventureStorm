using UnityEngine;

namespace AdventureStorm.Gameplay.EnemyOne.States
{
    public class EnemyOneCombatState : BaseState
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

        public override void EnterState(StateManager stateManager)
        {

        }

        public override void ExitState(StateManager stateManager)
        {

        }

        public override void FixedUpdateState(StateManager stateManager)
        {

        }

        public override void UpdateState(StateManager stateManager)
        {
            if (stateManager.TryGetComponent<EnemyOneStateManager>(out var enemyOneStateManager))
            {
                var direction = stateManager.IsFacingLeft ? Vector2.left : Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(stateManager.transform.position, direction, CombatDistance, stateManager.PlayerLayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent<PlayerStateManager>(out var playerStateManager))
                    {
                        if (playerStateManager.IsAlive)
                        {
                            float distance = Mathf.Abs(stateManager.transform.position.x - playerStateManager.transform.position.x);
                            if (distance <= CombatDistance)
                            {
                                float random = Random.Range(1f, 100f);
                                if (random / 100f <= DodgeChance)
                                {
                                    stateManager.SwitchState(DodgingState);
                                }
                                else
                                {
                                    stateManager.SwitchState(AttackingState);
                                }
                            }
                            else
                            {
                                stateManager.SwitchState(enemyOneStateManager.AliveState.MovementState);
                            }
                        }
                        else
                        {
                            stateManager.SwitchState(enemyOneStateManager.AliveState.IdleState);
                        }
                    }
                }
                else
                {
                    stateManager.SwitchState(enemyOneStateManager.AliveState.MovementState);
                }
            }
        }

        #endregion
    }
}