using AdventureStorm.Gameplay.Level;
using AdventureStorm.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Fields

        private GameObject _system;

        private LevelManager _levelManager;

        private Button _playButton;

        private Button _replayLevel;

        private Button _quitButton;

        #endregion

        #region LifeCycle

        private void Start()
        {
            if (_system == null)
            {
                _system = GameObject.Find("@System");
            }

            if (_system != null)
            {
                _levelManager = _system.GetComponent<LevelManager>();
            }
        }

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
            if (_levelManager != null)
            {
                _levelManager.LoadFirstUncompletedLevel();
            }
        }

        private void OnReplayLevelClicked(ClickEvent evt)
        {
            StartCoroutine(LoadReplayLevelScene());
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            Application.Quit();
        }

        private IEnumerator LoadReplayLevelScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneHelper.ReplayLevel);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        #endregion
    }
}
