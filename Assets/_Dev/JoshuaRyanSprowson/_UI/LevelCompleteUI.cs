using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm
{
    public class LevelCompleteUI : MonoBehaviour
    {
        #region Constant Fields

        private const string MainMenuUIScene = "_MainMenuUIScene";

        #endregion

        #region Fields

        private Button _mainMenuButton;

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            _mainMenuButton = uiDocument.rootVisualElement.Q("main-menu") as Button;
            _mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);
        }

        #endregion

        #region Private Methods

        private void OnMainMenuButtonClicked(ClickEvent evt)
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

        #endregion
    }
}
