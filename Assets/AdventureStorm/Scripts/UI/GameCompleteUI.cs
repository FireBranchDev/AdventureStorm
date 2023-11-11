using AdventureStorm._Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class GameCompleteUI : MonoBehaviour
    {
        #region Constant Fields

        private const string MainMenuUIScene = "MainMenuUIScene";

        #endregion

        #region Fields

        private Button _mainMenu;

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            _mainMenu = uiDocument.rootVisualElement.Q("main-menu") as Button;

            _mainMenu.RegisterCallback<ClickEvent>(OnMenuMenuClicked);
        }

        #endregion

        #region Private Methods

        private void OnMenuMenuClicked(ClickEvent evt)
        {
            StartCoroutine(_SceneHelper.LoadSceneCoroutine(MainMenuUIScene));
        }

        #endregion
    }
}