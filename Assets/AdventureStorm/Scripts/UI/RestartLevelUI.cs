using AdventureStorm.Gameplay.Level;
using AdventureStorm.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class RestartLevelUI : MonoBehaviour
    {
        #region Fields

        private GameObject _system;

        private LevelManager _levelManager;

        private Button _restartButton;
        private Button _quitButton;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _system = GameObject.Find("@System");

            if (_system != null)
            {
                _levelManager = _system.GetComponent<LevelManager>();
            }
        }

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            _restartButton = uiDocument.rootVisualElement.Q("restart-button") as Button;
            _quitButton = uiDocument.rootVisualElement.Q("quit-button") as Button;

            _restartButton.RegisterCallback<ClickEvent>(OnRestartButtonClicked);

            _quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
        }

        #endregion

        #region Private Methods

        private void OnRestartButtonClicked(ClickEvent evt)
        {
            if (_levelManager != null)
            {
                StartCoroutine(SceneHelper.LoadSceneCoroutine(_levelManager.CurrentLevel.SceneName));
                _restartButton.UnregisterCallback<ClickEvent>(OnRestartButtonClicked);
            }
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            Application.Quit();
        }

        #endregion
    }
}