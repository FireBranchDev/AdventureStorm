using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureStorm
{
    public class _LevelManager : MonoBehaviour
    {
        #region Private Methods

        private IEnumerator LoadSceneCoroutine(string sceneName)
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
