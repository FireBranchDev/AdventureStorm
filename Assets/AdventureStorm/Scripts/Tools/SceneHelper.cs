using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureStorm.Tools
{
    public static class SceneHelper
    {
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
    }
}