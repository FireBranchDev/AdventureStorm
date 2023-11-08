using UnityEngine;
using UnityEngine.UIElements;

namespace AdventureStorm
{
    public class PlayerUI : MonoBehaviour
    {
        private ProgressBar _healthBar;

        private ProgressBar _staminaBar;

        private GameObject _player;

        private _PlayerStateManager _playerStateManager;

        private _PlayerDodgingState _playerDodgingState;

        private UIDocument _uiDocument;

        private void Awake()
        {
            _healthBar = null;

            _staminaBar = null;

            _player = null;

            _playerStateManager = null;

            _playerDodgingState = null;
        }

        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();

            _healthBar = _uiDocument.rootVisualElement.Q("health-bar") as ProgressBar;
            _staminaBar = _uiDocument.rootVisualElement.Q("stamina-bar") as ProgressBar;

            _healthBar.highValue = _PlayerStateManager.MaximumHealth;

            _staminaBar.highValue = _PlayerDodgingState.MaximumDodgeAttackStamina;

            _player = GameObject.FindWithTag("Player");

            if (_player != null)
            {
                if (_player.TryGetComponent<_PlayerStateManager>(out var x))
                {
                    _playerStateManager = x;
                }
            }
        }

        private void Update()
        {
            if (_playerStateManager != null)
            {
                if (_playerDodgingState == null)
                {
                    if (_playerStateManager.AliveState != null)
                    {
                        _playerDodgingState = _playerStateManager.AliveState.DodgingState;
                    }
                }

                _healthBar.value = _playerStateManager.Health;

                if (_playerDodgingState != null)
                {
                    _staminaBar.value = _playerDodgingState.DodgeStamina;
                }
            }
        }
    }
}