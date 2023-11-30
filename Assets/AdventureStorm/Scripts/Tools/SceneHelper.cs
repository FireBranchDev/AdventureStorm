using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureStorm.Tools
{
    public static class SceneHelper
    {
        #region Properties

        public static string MainMenu => "MainMenuUIScene";
        public static string ReplayLevel => "ReplayLevelUIScene";
        public static string RestartLevel => "RestartLevelUIScene";
        public static string LevelComplete => "LevelCompleteUIScene";
        public static string GameComplete => "GameCompleteUIScene";
        public static string LevelOne => "LevelOneScene";
        public static string LevelThree => "LevelThreeScene";
        public static string LevelTwo => "LevelTwoScene";

        #endregion

        #region Public Methods

        public static IEnumerator LoadSceneCoroutine(string sceneName)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
        }

        #endregion
    }
}