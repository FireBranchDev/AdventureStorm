using AdventureStorm.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class GameCompleteUI : MonoBehaviour
    {
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
            StartCoroutine(SceneHelper.LoadSceneCoroutine(SceneHelper.MainMenu));
        }

        #endregion
    }
}