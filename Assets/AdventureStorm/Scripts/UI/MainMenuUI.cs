using AdventureStorm.Systems;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Constant Fields

        private const string ReplayLevelUIScene = "ReplayLevelUIScene";

        #endregion

        #region Fields

        [Tooltip("The gameobject which contains all the level's scripts")]
        [SerializeField]
        private GameObject _system;

        private Button _playButton;

        private Button _replayLevel;

        private Button _quitButton;

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            _playButton = uiDocument.rootVisualElement.Q("play") as Button;
            _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);

            _replayLevel = uiDocument.rootVisualElement.Q("replay-level") as Button;
            _replayLevel.RegisterCallback<ClickEvent>(OnReplayLevelClicked);

            _quitButton = uiDocument.rootVisualElement.Q("quit") as Button;
            _quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
        }

        #endregion

        #region Private Methods

        private void OnPlayButtonClicked(ClickEvent evt)
        {
            if (_system != null)
            {
                if (_system.TryGetComponent<LevelManager>(out var levelManager))
                {
                    levelManager.LoadFirstUncompletedLevel();
                }
            }
        }

        private void OnReplayLevelClicked(ClickEvent evt)
        {
            StartCoroutine(LoadReplayLevelUIScene());
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            Application.Quit();
        }

        private IEnumerator LoadReplayLevelUIScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(ReplayLevelUIScene);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        #endregion
    }
}
