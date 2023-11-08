using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AdventureStorm
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Constant Fields

        private const string LastLevelPlayedScene = "_TestingScene";

        #endregion

        #region Fields

        private Button _playButton;

        private Button _quitButton;

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            _playButton = uiDocument.rootVisualElement.Q("play") as Button;
            _quitButton = uiDocument.rootVisualElement.Q("quit") as Button;

            _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
            _quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
        }

        #endregion

        #region Private Methods

        private void OnPlayButtonClicked(ClickEvent evt)
        {
            StartCoroutine(LoadLastLevelPlayedScene());
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            Application.Quit();
        }

        private IEnumerator LoadLastLevelPlayedScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LastLevelPlayedScene);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        #endregion
    }
}
