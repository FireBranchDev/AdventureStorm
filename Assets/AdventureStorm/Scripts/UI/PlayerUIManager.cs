using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureStorm.UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        #region Constant Fields

        private const string RestartLevelUIScene = "RestartLevelUIScene";

        private const float RestartLevelUIDelay = 0.45f;

        #endregion

        #region Fields

        [Tooltip("Displays the player's UI in the game.")]
        [SerializeField]
        private GameObject _playerUI;

        [Tooltip("The playable hero of the game.")]
        [SerializeField]
        private GameObject _player;

        [Tooltip("Contains the system scripts for the game.")]
        [SerializeField]
        private GameObject _system;

        private _PlayerStateManager _playerStateManager;

        private _LevelManager _levelManager;

        private Coroutine _loadRestartLevelUIScene;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _playerStateManager = null;
            _levelManager = null;

            _loadRestartLevelUIScene = null;
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
                        _playerUI.SetActive(true);
                    }
                    else
                    {
                        if (_loadRestartLevelUIScene == null)
                        {
                            _loadRestartLevelUIScene = StartCoroutine(LoadRestartLevelUIScene());
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator LoadRestartLevelUIScene()
        {
            yield return new WaitForSeconds(RestartLevelUIDelay);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(RestartLevelUIScene, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        #endregion
    }
}