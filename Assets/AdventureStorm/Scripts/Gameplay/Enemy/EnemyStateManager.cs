using UnityEngine;

namespace AdventureStorm.Gameplay.Enemy
{
    public class EnemyStateManager : StateManager
    {
        #region Properties

        public override float MaximumHealth { get => 5f; }

        public bool HasKey { get; set; }

        #endregion

        #region LifeCycle

        #endregion

        #region Public Methods

        public void FacePlayer(EnemyStateManager stateManager)
        {
            RaycastHit2D left = Physics2D.Raycast(stateManager.transform.position, Vector2.left, float.MaxValue, stateManager.PlayerLayerMask);
            RaycastHit2D right = Physics2D.Raycast(stateManager.transform.position, Vector2.right, float.MaxValue, stateManager.PlayerLayerMask);

            if (stateManager.IsFacingLeft)
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
