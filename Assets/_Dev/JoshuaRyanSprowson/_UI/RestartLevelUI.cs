using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm
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
            StartCoroutine(ReloadActiveScene());
        }

        private IEnumerator ReloadActiveScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            Application.Quit();
        }

        #endregion
    }
}
