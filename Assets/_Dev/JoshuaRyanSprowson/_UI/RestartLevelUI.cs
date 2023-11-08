using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            Application.Quit();
        }

        #endregion
    }
}
