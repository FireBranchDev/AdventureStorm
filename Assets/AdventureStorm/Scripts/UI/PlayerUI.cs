using AdventureStorm.Gameplay.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class PlayerUI : MonoBehaviour
    {
        private ProgressBar _healthBar;

        private ProgressBar _staminaBar;

        private Label _collectedKey;

        private GameObject _player;

        private UIDocument _uiDocument;

        private PlayerController _playerController;

        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();

            _healthBar = _uiDocument.rootVisualElement.Q("health-bar") as ProgressBar;
            _staminaBar = _uiDocument.rootVisualElement.Q("stamina-bar") as ProgressBar;

            _collectedKey = _uiDocument.rootVisualElement.Q("collected-key") as Label;

            _player = GameObject.FindWithTag("Player");

            if (_player != null)
            {
                _playerController = _player.GetComponent<PlayerController>();
                _healthBar.highValue = _playerController.MaxHealth;
                _staminaBar.highValue = _playerController.MaxDodgeStamina;
            }
        }

        private void Update()
        {
            if (_playerController != null)
            {
                _healthBar.value = _playerController.Health;
                _staminaBar.value = _playerController.DodgeStamina;

                if (_playerController.WasKeyPickedUp)
                {
                    _collectedKey.text = "1/1 Key";
                }
                else
                {
                    _collectedKey.text = "0/1 Key";
                }
            }
        }
    }
}