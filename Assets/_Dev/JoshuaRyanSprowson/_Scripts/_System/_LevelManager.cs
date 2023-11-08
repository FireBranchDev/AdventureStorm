using UnityEngine;

namespace AdventureStorm
{
    public class _LevelManager : MonoBehaviour
    {
        #region Properties

        public bool IsLevelCompleted { get; set; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            IsLevelCompleted = false;
        }

        #endregion
    }
}
