using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerMovementState : _PlayerBaseState
    {
        #region Constant Fields

        private const string WalkingAnimation = "Walking";

        private const float WalkingSpeed = 5f;

        #endregion

        #region Fields

        private Coroutine _walkingCoroutine;

        private float _horizontalAxis;

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            _walkingCoroutine = player.StartCoroutine(WalkingCoroutine(player));

            player.RechargeDodgeAttackStaminaCoroutine ??=
                player.StartCoroutine(player.DodgeAttackState.RechargeDodgeAttackStaminaCoroutine());
        }

        public override void ExitState(_PlayerStateManager player)
        {
            if (_walkingCoroutine != null)
            {
                player.StopCoroutine(_walkingCoroutine);
                _walkingCoroutine = null;
            }
        }

        public override void FixedUpdateState(_PlayerStateManager player)
        {
            Move(player);
        }

        public override void UpdateState(_PlayerStateManager player)
        {
            _horizontalAxis = Input.GetAxis(_PlayerStateManager.HorizontalAxis);

            if (_horizontalAxis == 0f)
            {
                player.SwitchState(player.IdleState);
            }

            if (Input.GetMouseButtonDown(0))
            {
                player.SwitchState(player.AttackingState);
            }

            if (Input.GetKey(KeyCode.Space) && _horizontalAxis != 0f)
            {
                player.SwitchState(player.DodgeAttackState);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator WalkingCoroutine(_PlayerStateManager player)
        {
            if (player.AnimatorManager != null)
            {
                player.AnimatorManager.ChangeAnimationState(WalkingAnimation);
                for (;;)
                {
                    if (player.AnimatorManager.DidAnimationFinish(WalkingAnimation))
                    {
                        player.AnimatorManager.ReplayAnimation();
                    }

                    yield return null;
                }
            }
        }

        private void Move(_PlayerStateManager player)
        {
            Vector2 velocity = new(0, 0);

            // Move to the right.
            if (_horizontalAxis > 0f && _horizontalAxis != 0)
            {
                velocity.x = WalkingSpeed;    
            }
            // Move to the left.
            else
            {
                velocity.x = -WalkingSpeed;
            }

            player.Rb2D.MovePosition(player.Rb2D.position + velocity * Time.fixedDeltaTime);
        }

        #endregion
    }
}
