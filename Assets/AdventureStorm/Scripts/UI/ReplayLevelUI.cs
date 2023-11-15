using AdventureStorm.Data;
using AdventureStorm.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class ReplayLevelUI : MonoBehaviour
    {
        #region Constant Fields

        private const string LevelOneScene = "_TestingScene";

        private const string LevelTwoScene = "_TestingScene2";

        private const string MainMenuUIScene = "MainMenuUIScene";

        #endregion

        #region Fields  

        [Tooltip("Container for the scene's scripts")]
        [SerializeField]
        private GameObject _system;

        private string _selectedLevel;

        private Button _levelOne;

        private Button _levelTwo;

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
            _levelTwo = uiDocument.rootVisualElement.Q("level-two") as Button;

            _mainMenu = uiDocument.rootVisualElement.Q("main-menu") as Button;
            _mainMenu.RegisterCallback<ClickEvent>(OnMainMenuClicked);

            _selectLevel = uiDocument.rootVisualElement.Q("select-level") as Button;
            _selectLevel.RegisterCallback<ClickEvent>(OnSelectLevelClicked);

            if (_system != null)
            {
                if (_system.TryGetComponent<LevelManager>(out var levelManager))
                {
                    StartCoroutine(AllowUnlockedLevelsToBeClicked(levelManager));
                }
            }
        }

        #endregion

        #region Private Methods

        private void OnLevelOneClicked(ClickEvent evt)
        {
            _selectedLevel = LevelOneScene;
        }

        private void OnLevelTwoClicked(ClickEvent evt)
        {
            _selectedLevel = LevelTwoScene;
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
                if (_system != null)
                {
                    if (_system.TryGetComponent<LevelManager>(out var levelManager))
                    {
                        levelManager.ReplayCompletedLevel(_selectedLevel);
                    }
                }
            }
        }

        private IEnumerator AllowUnlockedLevelsToBeClicked(LevelManager levelManager)
        {
            while (!levelManager.HasLoaded)
            {
                yield return null;
            }

            List<LevelData> unlockedLevels = levelManager.GetCompletedLevels();

            foreach (var levelData in unlockedLevels)
            {
                switch (levelData.ID)
                {
                    case 1:
                        _levelOne.RegisterCallback<ClickEvent>(OnLevelOneClicked);
                        break;
                    case 2:
                        _levelTwo.RegisterCallback<ClickEvent>(OnLevelTwoClicked);
                        break;
                    case 3:
                        break;
                }
            }
        }

        #endregion
    }
}
