using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public abstract class PlayerBaseState
    {
        public abstract void EnterState(PlayerStateManager player);

        public abstract void ExitState(PlayerStateManager player);

        public abstract void FixedUpdateState(PlayerStateManager player);

        public abstract void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision);

        public abstract void OnTriggerStay2D(PlayerStateManager player, Collider2D collision);

        public abstract void OnTriggerExit2D(PlayerStateManager player, Collider2D collision);

        public abstract void UpdateState(PlayerStateManager player);
    }
}
