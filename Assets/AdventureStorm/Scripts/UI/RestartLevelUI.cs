using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm.UI
{
    public class RestartLevelUI : MonoBehaviour
    {
        #region Fields

        private Button _restartButton;
        private Button _quitButton;

        #endregion

        #region LifeCycle

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
            StartCoroutine(ReloadPreviousScene());
        }

        private IEnumerator ReloadPreviousScene()
        {
            if (SceneManager.sceneCount > 1)
            {
                // The previous level would be the first scene loaded by the scene manager.
                Scene previousLevel = SceneManager.GetSceneAt(0);

                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(previousLevel.buildIndex);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            Application.Quit();
        }

        #endregion
    }
}
