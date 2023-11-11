using AdventureStorm.Systems;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class LevelCompleteUI : MonoBehaviour
    {
        #region Constant Fields

        private const string MainMenuUIScene = "MainMenuUIScene";

        #endregion

        #region Fields

        [SerializeField]
        private GameObject _system;

        private Button _mainMenuButton;

        private Button _nextLevel;

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            _mainMenuButton = uiDocument.rootVisualElement.Q("main-menu") as Button;
            _mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);

            _nextLevel = uiDocument.rootVisualElement.Q("next-level") as Button;
            _nextLevel.RegisterCallback<ClickEvent>(OnNextLevelClicked);
        }

        #endregion

        #region Private Methods

        private void OnMainMenuButtonClicked(ClickEvent evt)
        {
            StartCoroutine(LoadMainMenuUIScene());
        }

        private void OnNextLevelClicked(ClickEvent evt)
        {
            if (_system != null)
            {
                if (_system.TryGetComponent<LevelManager>(out var levelManager))
                {
                    levelManager.LoadNextLevel();
                }
            }
        }

        private IEnumerator LoadMainMenuUIScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MainMenuUIScene);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        #endregion
    }
}
