using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerMovementState : _PlayerBaseState
    {
        #region Constant Fields

        private const string WalkingAnimation = "Walking";
        private const float WalkingSpeed = 5f;
        private const string LeftWallTag = "LeftWall";
        private const string RightWallTag = "RightWall";

        #endregion

        #region Fields

        private Coroutine _walkingCoroutine;
        private float _horizontalAxis;
        private bool _canMoveLeft;
        private bool _canMoveRight;

        #endregion

        #region Constructors

        public _PlayerMovementState()
        {
            _walkingCoroutine = null;
            _horizontalAxis = 0f;
            _canMoveLeft = true;
            _canMoveRight = true;
        }

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            _walkingCoroutine = player.StartCoroutine(WalkingCoroutine(player));

            player.AliveState.RechargeDodgeAttackStaminaCoroutine ??=
                player.StartCoroutine(player.AliveState.DodgingState.RechargeDodgeAttackStaminaCoroutine());
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

        public override void OnTriggerEnter2D(_PlayerStateManager player, Collider2D collision)
        {
            if (collision.CompareTag(LeftWallTag))
            {
                _canMoveLeft = false;
            }

            if (collision.CompareTag(RightWallTag))
            {
                _canMoveRight = false;
            }
        }

        public override void OnTriggerExit2D(_PlayerStateManager player, Collider2D collision)
        {
            if (collision.CompareTag(LeftWallTag))
            {
                _canMoveLeft = true;
            }

            if (collision.CompareTag(RightWallTag))
            {
                _canMoveRight = true;
            }
        }

        public override void UpdateState(_PlayerStateManager player)
        {
            _horizontalAxis = Input.GetAxis(_PlayerStateManager.HorizontalAxis);

            if (_horizontalAxis == 0f)
            {
                player.SwitchState(player.AliveState.IdleState);
            }

            if (Input.GetMouseButtonDown(0))
            {
                player.SwitchState(player.AliveState.AttackingState);
            }

            if (Input.GetKey(KeyCode.Space) && _horizontalAxis != 0f)
            {
                player.SwitchState(player.AliveState.DodgingState);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator WalkingCoroutine(_PlayerStateManager player)
        {
            if (player.AnimatorManager != null)
            {
                player.AnimatorManager.ChangeAnimationState(WalkingAnimation);
                for (; ; )
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
            Vector2 velocity = Vector2.zero;

            if (_horizontalAxis > 0f && _canMoveRight)
            {
                velocity.x = WalkingSpeed;
            }
            else if (_horizontalAxis < 0f && _canMoveLeft)
            {
                velocity.x = -WalkingSpeed;
            }

            player.Rb2D.MovePosition(player.Rb2D.position + velocity * Time.fixedDeltaTime);
        }

        #endregion
    }
}
