using AdventureStorm._Data;
using AdventureStorm._Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdventureStorm
{
    public class _LevelManager : MonoBehaviour
    {
        #region Constant Fields

        private const string LevelOneScene = "_TestingScene";
        private const string LevelTwoScene = "_TestingScene2";

        private const string SaveFileName = "levels.json";

        #endregion

        #region Fields

        private _LevelsData _levelsData;

        private string _saveFilePath;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            LoadData();
        }

        #endregion

        #region Public Methods

        public void LoadFirstUncompletedLevel()
        {
            _LevelData currentLevel = _levelsData.Levels.FirstOrDefault(level => !level.LevelCompleted);

            if (currentLevel == null)
            {
                return;
            }

            StartCoroutine(_SceneHelper.LoadSceneCoroutine(currentLevel.SceneName));
        }

        public void MarkCurrentLevelAsComplete()
        {
            _LevelData currentLevel = _levelsData.Levels.FirstOrDefault(level => !level.LevelCompleted);

            if (currentLevel == null)
            {
                return;
            }

            currentLevel.LevelCompleted = true;

            SaveData();
        }

        public List<_LevelData> GetUncompletedLevels()
        {
            List<_LevelData> levels = _levelsData.Levels.FindAll(level => !level.LevelCompleted);
            return levels;
        }

        #endregion

        #region Private Methods

        private async void LoadData()
        {
            if (!File.Exists(_saveFilePath))
            {
                InitialiseData();
                return;
            }

            string contents = await File.ReadAllTextAsync(_saveFilePath);
            if (string.IsNullOrEmpty(contents))
            {
                InitialiseData();
                return;
            }

            var levelsData = JsonUtility.FromJson<_LevelsData>(contents);
            if (levelsData == null)
            {
                InitialiseData();
                return;
            }

            _levelsData = levelsData;
        }

        private void InitialiseData()
        {
            var levelOne = new _LevelData
            {
                SceneName = LevelOneScene,
                LevelCompleted = false,
            };

            var levelTwo = new _LevelData()
            {
                SceneName = LevelTwoScene,
                LevelCompleted = false,
            };

            _levelsData = new _LevelsData
            {
                Levels = new List<_LevelData> { levelOne, levelTwo }
            };

            SaveData();
        }

        private async void SaveData()
        {
            if (!File.Exists(_saveFilePath))
            {
                File.Create(_saveFilePath);
                return;
            }

            string levelsData = JsonUtility.ToJson(_levelsData);

            await File.WriteAllTextAsync(_saveFilePath, levelsData);
        }

        #endregion
    }
}
