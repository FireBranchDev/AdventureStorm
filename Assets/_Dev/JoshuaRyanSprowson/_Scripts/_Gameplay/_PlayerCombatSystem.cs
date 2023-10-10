
using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerCombatSystem : MonoBehaviour
    {
        #region Fields
        [Tooltip("How far is the distance that the character can dodge?")]
        /// <summary>
        /// How far is the distance that the character can dodge
        ///</summary>
        [SerializeField] private float _dodgeDistance = 0.75f;

        [Tooltip("How much stamina for dodging does the character have?")]
        /// <summary>
        /// How much stamina for dodging does the character have
        /// </summary>
        [SerializeField] private int _dodgeStamina = 4;

        [Tooltip("What height does the character reach when dodging?")]
        /// <summary>
        /// What height does the character reach when dodging
        /// </summary>
        [SerializeField] private float _dodgeHeight = 0.25f;

        [Tooltip("How long in seconds should it take for the stamina required for dodging to recharge?")]
        /// <summary>
        /// How long in seconds should it take for the stamina required for dodging to recharge
        /// </summary>
        [SerializeField] private float _dodgingStaminaRechargeDurationInSeconds = 3f;

        private Animator _animator;

        private _PlayerInputManager _playerInputManager;

        private readonly int _attackingTriggerHash = Animator.StringToHash("TrAttacking");
        private readonly int _isDodgingHash = Animator.StringToHash("IsDodging");
        private readonly int _isIdleHash = Animator.StringToHash("IsIdle");
        private readonly int _isMovingHash = Animator.StringToHash("IsMoving");

        private int _maximumDodgeStamina;
        private float _maximumDodgeHeight = 0f;
        private bool _canDodge = true;
        #endregion

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInputManager = GetComponent<_PlayerInputManager>();

            _maximumDodgeStamina = _dodgeStamina;

            _maximumDodgeHeight = transform.position.y + _dodgeHeight;

            StartCoroutine(RechargeDodgeStamina());
        }

        private IEnumerator RechargeDodgeStamina()
        {
            for (;;)
            {
                yield return new WaitForSecondsRealtime(_dodgingStaminaRechargeDurationInSeconds);

                if (_dodgeStamina < _maximumDodgeStamina && !_playerInputManager.IsDodging)
                {
                    _dodgeStamina++;
                }
            }
        }

        private void Update()
        {
            if (_playerInputManager != null)
            {
                if (_playerInputManager.IsAttacking)
                {
                    Attack();
                }
                else if (_playerInputManager.IsDodging)
                {
                    if (_playerInputManager.IsDodgingLeft && _canDodge && _dodgeStamina > 0)
                    {
                        DodgeLeft();
                    }
                    else if (_playerInputManager.IsDodgingRight && _canDodge && _dodgeStamina > 0)
                    {
                        DodgeRight();
                    }
                }
            }
        }
        
        private void Attack()
        {
            _animator.SetTrigger(_attackingTriggerHash);
        }

        private void DodgeLeft()
        {
            _dodgeStamina--;
            PlayDodgingAnimation();
            Vector3 movement = new(-_dodgeDistance, _dodgeHeight);
            transform.Translate(movement);

            if (transform.position.y >= _maximumDodgeHeight)
            {
                _canDodge = false;
            }

            StartCoroutine(DodgeFinished());
        }

        private void DodgeRight()
        {
            _dodgeStamina--;
            PlayDodgingAnimation();
            Vector3 movement = new(_dodgeDistance, _dodgeHeight);
            transform.Translate(movement);

            if (transform.position.y >= _maximumDodgeHeight)
            {
                _canDodge = false;
            }

            StartCoroutine(DodgeFinished());
        }

        private void PlayDodgingAnimation()
        {
            _animator.SetBool(_isIdleHash, false);
            _animator.SetBool(_isMovingHash, false);
            _animator.SetBool(_isDodgingHash, true);
        }

        private IEnumerator DodgeFinished()
        {
            yield return new WaitForSeconds(0.3f);
            Vector3 movement = new(0, -_dodgeHeight);
            transform.Translate(movement);
            _animator.SetBool(_isDodgingHash, false);
            _animator.SetBool(_isIdleHash, true);
            StartCoroutine(DodgeReset());
        }

        private IEnumerator DodgeReset()
        {
            yield return new WaitForSeconds(0.5f);
            _canDodge = true;
        }
    }
}