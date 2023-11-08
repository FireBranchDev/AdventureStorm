using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerDodgingState : _PlayerBaseState
    {
        #region Constant Fields

        public const float MaximumDodgeAttackStamina = 4f;

        private const string StartJumpAnimation = "Start Jump";

        private const float SecondsForDodgeAttackCooldown = 0.65f;

        private const float SecondsForRechargingDodgeAttackStamina = 1.5f;

        private const float DodgeAttackJumpHeight = 0.25f;

        #endregion

        #region Fields

        private Coroutine _dodgeAttackCoroutine;

        private float _dodgeAttackStamina;

        private bool _isDodgeAttackInProgress;

        private bool _isDodgeAttackCooldownFinished;

        #endregion

        #region Constructors

        public _PlayerDodgingState()
        {
            _dodgeAttackStamina = MaximumDodgeAttackStamina;
            _isDodgeAttackInProgress = false;
            _isDodgeAttackCooldownFinished = true;
        }

        #endregion

        #region Properties

        public float DodgeStamina => _dodgeAttackStamina;

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            if (CanPerformDodgeAttack())
            {
                player.StopCoroutine(player.AliveState.RechargeDodgeAttackStaminaCoroutine);
                player.AliveState.RechargeDodgeAttackStaminaCoroutine = null;

                _dodgeAttackCoroutine ??= player.StartCoroutine(DodgeAttackCoroutine(player));
            }
            else
            {
                player.SwitchState(player.AliveState.MovementState);
            }
        }

        public override void ExitState(_PlayerStateManager player)
        {
            _isDodgeAttackInProgress = false;

            if (_dodgeAttackCoroutine != null)
            {
                player.StopCoroutine(_dodgeAttackCoroutine);
                _dodgeAttackCoroutine = null;
            }
        }

        public override void FixedUpdateState(_PlayerStateManager player)
        {

        }

        public override void OnTriggerEnter2D(_PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerExit2D(_PlayerStateManager player, Collider2D collision)
        {

        }

        public override void UpdateState(_PlayerStateManager player)
        {

        }

        public IEnumerator RechargeDodgeAttackStaminaCoroutine()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(SecondsForRechargingDodgeAttackStamina);
                if (_dodgeAttackStamina < MaximumDodgeAttackStamina)
                {
                    _dodgeAttackStamina++;
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator DodgeAttackCoroutine(_PlayerStateManager player)
        {
            _isDodgeAttackInProgress = true;
            player.AnimatorManager.ChangeAnimationState(StartJumpAnimation);

            while (!player.AnimatorManager.DidAnimationFinish(StartJumpAnimation))
            {
                yield return null;
            }

            Vector2 direction = new();

            if (player.IsFacingLeft)
            {
                direction.x = -1f;
                direction.y = DodgeAttackJumpHeight;
            }
            else
            {
                direction.x = 1f;
                direction.y = DodgeAttackJumpHeight;
            }

            player.Rb2D.position = player.Rb2D.position + direction;
            yield return new WaitForSeconds(0.1f);
            player.Rb2D.position = player.Rb2D.position + new Vector2(0, -DodgeAttackJumpHeight);

            _isDodgeAttackInProgress = false;
            _dodgeAttackStamina--;
            player.StartCoroutine(DodgeAttackCooldownTimerCoroutine());
            player.SwitchState(player.AliveState.IdleState);
        }

        private IEnumerator DodgeAttackCooldownTimerCoroutine()
        {
            _isDodgeAttackCooldownFinished = false;
            yield return new WaitForSeconds(SecondsForDodgeAttackCooldown);
            _isDodgeAttackCooldownFinished = true;
        }

        private bool CanPerformDodgeAttack()
        {
            return _dodgeAttackStamina > 0f
                   && !_isDodgeAttackInProgress
                   && _isDodgeAttackCooldownFinished;
        }

        #endregion
    }
}
