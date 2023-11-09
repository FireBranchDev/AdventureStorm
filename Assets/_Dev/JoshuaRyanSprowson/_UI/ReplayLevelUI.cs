using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm
{
    public class ReplayLevelUI : MonoBehaviour
    {
        #region Constant Fields

        private const string LevelOneScene = "_TestingScene";

        private const string MainMenuUIScene = "_MainMenuUIScene";

        #endregion

        #region Fields

        private string _selectedLevel;

        private Button _levelOne;

        private Button _mainMenu;

        private Button _selectLevel;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _selectedLevel = string.Empty;
        }

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            _levelOne = uiDocument.rootVisualElement.Q("level-one") as Button;
            _levelOne.RegisterCallback<ClickEvent>(OnLevelOneClicked);

            _mainMenu = uiDocument.rootVisualElement.Q("main-menu") as Button;
            _mainMenu.RegisterCallback<ClickEvent>(OnMainMenuClicked);

            _selectLevel = uiDocument.rootVisualElement.Q("select-level") as Button;
            _selectLevel.RegisterCallback<ClickEvent>(OnSelectLevelClicked);
        }

        #endregion

        #region Private Methods

        private void OnLevelOneClicked(ClickEvent evt)
        {
            _selectedLevel = LevelOneScene;
        }

        private void OnMainMenuClicked(ClickEvent evt)
        {
            StartCoroutine(LoadMainMenuUIScene());
        }

        private IEnumerator LoadMainMenuUIScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MainMenuUIScene);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        private void OnSelectLevelClicked(ClickEvent evt)
        {
            if (!string.IsNullOrEmpty(_selectedLevel))
            {
                StartCoroutine(LoadSelectedLevel(_selectedLevel));
            }
        }

        private IEnumerator LoadSelectedLevel(string level)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        #endregion
    }
}
