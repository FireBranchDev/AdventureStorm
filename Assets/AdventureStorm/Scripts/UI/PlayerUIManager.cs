using AdventureStorm.Gameplay.Level;
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

        private LevelManager _levelManager;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _levelManager = null;
        }

        private void Update()
        {
            if (_player != null && _system != null)
            {
                if (_levelManager == null)
                {
                    if (_system.TryGetComponent<LevelManager>(out var levelManager))
                    {
                        _levelManager = levelManager;
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