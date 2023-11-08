using UnityEngine;

namespace AdventureStorm
{
    public abstract class _PlayerBaseState
    {
        public abstract void EnterState(_PlayerStateManager player);

        public abstract void ExitState(_PlayerStateManager player);

        public abstract void FixedUpdateState(_PlayerStateManager player);

        public abstract void OnTriggerEnter2D(_PlayerStateManager player, Collider2D collision);

        public abstract void OnTriggerStay2D(_PlayerStateManager player, Collider2D collision);

        public abstract void OnTriggerExit2D(_PlayerStateManager player, Collider2D collision);

        public abstract void UpdateState(_PlayerStateManager player);
    }
}
