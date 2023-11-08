using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerUIManager : MonoBehaviour
    {
        #region Fields

        [Tooltip("Displays the player's UI in the game.")]
        [SerializeField]
        private GameObject _playerUI;

        [Tooltip("Displays the restart level UI in the game.")]
        [SerializeField]
        private GameObject _restartLevelUI;

        [Tooltip("Displays the level complete UI in the game.")]
        [SerializeField]
        private GameObject _levelCompleteUI;

        [Tooltip("The playable hero of the game.")]
        [SerializeField]
        private GameObject _player;

        [Tooltip("Contains the system scripts for the game.")]
        [SerializeField]
        private GameObject _system;

        private _PlayerStateManager _playerStateManager;

        private _LevelManager _levelManager;

        private Coroutine _displayRestartLevelUI;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _playerStateManager = null;
            _levelManager = null;
            _displayRestartLevelUI = null;
        }

        private void Update()
        {
            if (_player != null && _system != null)
            {
                if (_playerStateManager == null)
                {
                    if (_player.TryGetComponent<_PlayerStateManager>(out var playerStateManager))
                    {
                        _playerStateManager = playerStateManager;
                    }
                }

                if (_levelManager == null)
                {
                    if (_system.TryGetComponent<_LevelManager>(out var levelManager))
                    {
                        _levelManager = levelManager;
                    }
                }

                if (_playerStateManager != null && _levelManager != null)
                {
                    if (_playerStateManager.IsAlive)
                    {
                        if (_levelManager.IsLevelCompleted)
                        {
                            _levelCompleteUI.SetActive(true);
                            Time.timeScale = 0f;
                        }
                        else
                        {
                            _playerUI.SetActive(true);
                        }
                    }
                    else
                    {
#pragma warning disable IDE0074 // Use compound assignment
                        if (_displayRestartLevelUI == null)
                        {
                            _displayRestartLevelUI = StartCoroutine(DisplayRestartLevelUI());
                        }
#pragma warning restore IDE0074 // Use compound assignment
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator DisplayRestartLevelUI()
        {
            yield return new WaitForSeconds(0.1f);

            _restartLevelUI.SetActive(true);
        }

        #endregion
    }
}