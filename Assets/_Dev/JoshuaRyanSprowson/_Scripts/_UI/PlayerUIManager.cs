using System.Collections;
using UnityEngine;

namespace AdventureStorm
{
    public class PlayerUIManager : MonoBehaviour
    {
        #region Fields

        [Tooltip("Displays the player's UI in the game.")]
        [SerializeField]
        private GameObject _playerUI;

        [Tooltip("Displays the restart level UI in the game.")]
        [SerializeField]
        private GameObject _restartLevelUI;

        [Tooltip("The playable hero of the game.")]
        [SerializeField]
        private GameObject _player;

        private _PlayerStateManager _playerStateManager;

        private Coroutine _displayRestartLevelUI;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _playerStateManager = null;
            _displayRestartLevelUI = null;
        }

        private void Update()
        {
            if (_player != null)
            {
                if (_playerStateManager == null)
                {
                    if (_player.TryGetComponent<_PlayerStateManager>(out var playerStateManager))
                    {
                        _playerStateManager = playerStateManager;
                    }
                }
                else
                {
                    if (_playerStateManager.IsAlive)
                    {
                        _playerUI.SetActive(true);
                        _restartLevelUI.SetActive(false);
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
            yield return new WaitForSeconds(0.25f);

            _playerUI.SetActive(false);
            _restartLevelUI.SetActive(true);
        }

        #endregion
    }
}