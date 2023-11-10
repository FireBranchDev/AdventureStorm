using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureStorm._Tools
{
    public static class _SceneHelper
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