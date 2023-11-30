using AdventureStorm.Data;
using AdventureStorm.Gameplay.Level;
using AdventureStorm.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class ReplayLevelUI : MonoBehaviour
    {
        #region Fields  

        private GameObject _system;

        private LevelManager _levelManager;

        private string _selectedLevel;

        private Button _levelOne;

        private Button _levelTwo;

        private Button _levelThree;

        private Button _mainMenu;

        private Button _selectLevel;

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            _levelOne = uiDocument.rootVisualElement.Q("level-one") as Button;
            _levelTwo = uiDocument.rootVisualElement.Q("level-two") as Button;
            _levelThree = uiDocument.rootVisualElement.Q("level-three") as Button;

            _mainMenu = uiDocument.rootVisualElement.Q("main-menu") as Button;
            _mainMenu.RegisterCallback<ClickEvent>(OnMainMenuClicked);

            _selectLevel = uiDocument.rootVisualElement.Q("select-level") as Button;
            _selectLevel.RegisterCallback<ClickEvent>(OnSelectLevelClicked);
        }

        private void Start()
        {
            if (_system == null)
            {
                _system = GameObject.Find("@System");
            }

            if (_levelManager == null)
            {
                if (_system != null)
                {
                    _levelManager = _system.GetComponent<LevelManager>();
                    StartCoroutine(UnlockedLevelsClickableCoroutine());
                }
            }
        }

        #endregion

        #region Private Methods

        private void OnLevelOneClicked(ClickEvent evt)
        {
            if (_levelManager != null)
            {
                foreach (var level in _levelManager.GetCompletedLevels())
                {
                    if (level.ID == 1)
                    {
                        _selectedLevel = level.SceneName;
                    }
                }
            }
        }

        private void OnLevelTwoClicked(ClickEvent evt)
        {
            if (_levelManager != null)
            {
                foreach (var level in _levelManager.GetCompletedLevels())
                {
                    if (level.ID == 2)
                    {
                        _selectedLevel = level.SceneName;
                    }
                }
            }
        }

        private void OnLevelThreeClicked(ClickEvent evt)
        {
            if (_levelManager != null)
            {
                foreach (var level in _levelManager.GetCompletedLevels())
                {
                    if (level.ID == 3)
                    {
                        _selectedLevel = level.SceneName;
                    }
                }
            }
        }


        private void OnMainMenuClicked(ClickEvent evt)
        {
            StartCoroutine(SceneHelper.LoadSceneCoroutine(SceneHelper.MainMenu));
        }

        private void OnSelectLevelClicked(ClickEvent evt)
        {
            if (!string.IsNullOrEmpty(_selectedLevel))
            {
                if (_levelManager != null)
                {
                    _levelManager.ReplayCompletedLevel(_selectedLevel);
                }
            }
        }

        private IEnumerator UnlockedLevelsClickableCoroutine()
        {
            while (_levelManager == null)
            {
                yield return null;
            }

            while (!_levelManager.HasLoaded)
            {
                yield return null;
            }

            List<LevelData> unlockedLevels = _levelManager.GetCompletedLevels();
            foreach (var levelData in unlockedLevels)
            {
                switch (levelData.ID)
                {
                    case 1:
                        _levelOne.AddToClassList("unlocked-level");
                        _levelOne.RegisterCallback<ClickEvent>(OnLevelOneClicked);
                        break;
                    case 2:
                        _levelTwo.AddToClassList("unlocked-level");
                        _levelTwo.RegisterCallback<ClickEvent>(OnLevelTwoClicked);
                        break;
                    case 3:
                        _levelThree.AddToClassList("unlocked-level");
                        _levelThree.RegisterCallback<ClickEvent>(OnLevelThreeClicked);
                        break;
                }
            }
        }

        #endregion
    }
}
