using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyAliveState : EnemyBaseState
    {
        #region Constructors

        public EnemyAliveState()
        {
            CombatState = new EnemyCombatState();
            IdleState = new EnemyIdleState();
            MovementState = new EnemyMovementState();

            IsFacingLeft = true;
        }

        #endregion

        #region Properties

        public EnemyCombatState CombatState { get; private set; }
        public EnemyIdleState IdleState { get; private set; }
        public EnemyMovementState MovementState { get; private set; }

        public bool IsFacingLeft { get; private set; }

        #endregion

        #region Public Methods

        public override void EnterState(EnemyStateManager enemy)
        {
            enemy.SwitchState(IdleState);
        }

        public override void ExitState(EnemyStateManager enemy)
        {

        }

        public override void FixedUpdateState(EnemyStateManager enemy)
        {

        }

        public override void UpdateState(EnemyStateManager enemy)
        {

        }

        public void FacePlayer(EnemyStateManager enemy)
        {
            RaycastHit2D left = Physics2D.Raycast(enemy.transform.position, Vector2.left, float.MaxValue, enemy.PlayerLayerMask);
            RaycastHit2D right = Physics2D.Raycast(enemy.transform.position, Vector2.right, float.MaxValue, enemy.PlayerLayerMask);

            if (IsFacingLeft)
            {
                if (right.collider != null)
                {
                    IsFacingLeft = false;
                }
                else
                {
                    IsFacingLeft = true;
                }
            }
            else
            {
                if (left.collider != null)
                {
                    IsFacingLeft = true;
                }
                else
                {
                    IsFacingLeft = false;
                }
            }

            if (left.collider != null && right.collider != null)
            {
                IsFacingLeft = true;
            }
        }

        public void FlipEnemy(EnemyStateManager enemy)
        {
            Vector3 direction = enemy.transform.localScale;

            if (IsFacingLeft && enemy.transform.localScale.x < 0)
                return;

            if (!IsFacingLeft && enemy.transform.localScale.x > 0)
                return;

            if (IsFacingLeft)
            {
                direction.x = -enemy.transform.localScale.x;
            }
            else
            {
                direction.x = Mathf.Abs(enemy.transform.localScale.x);
            }

            enemy.transform.localScale = direction;
        }

        #endregion
    }
}
