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

        #region Properties

        public bool HasLoaded { get; private set; }

        public _LevelData CurrentLevel { get => _levelsData.CurrentLevel; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            HasLoaded = false;

            LoadData();
        }

        #endregion

        #region Public Methods

        public void LoadFirstUncompletedLevel()
        {
            _LevelData firstUncompletedLevel = _levelsData.Levels.FirstOrDefault(level => !level.LevelCompleted);

            if (firstUncompletedLevel == null)
            {
                return;
            }

            _levelsData.CurrentLevel = firstUncompletedLevel;

            SaveData();

            StartCoroutine(_SceneHelper.LoadSceneCoroutine(firstUncompletedLevel.SceneName));
        }

        public void ReplayCompletedLevel(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return;
            }

            _LevelData level = _levelsData.Levels.FirstOrDefault(level => level.SceneName == sceneName);

            if (level == null)
            {
                return;
            }

            _levelsData.CurrentLevel = level;

            SaveData();

            StartCoroutine(_SceneHelper.LoadSceneCoroutine(sceneName));
        }

        public void MarkCurrentLevelAsComplete()
        {
            _LevelData level = _levelsData.Levels.Find(level => level.ID == CurrentLevel.ID);
            level.LevelCompleted = true;
            CurrentLevel.LevelCompleted = true;

            SaveData();
        }

        public List<_LevelData> GetUncompletedLevels()
        {
            List<_LevelData> levels = _levelsData.Levels.FindAll(level => !level.LevelCompleted);

            return levels;
        }

        public List<_LevelData> GetCompletedLevels()
        {
            List<_LevelData> levels = _levelsData.Levels.FindAll(level => level.LevelCompleted);

            return levels;
        }

        public void LoadNextLevel()
        {
            int id = _levelsData.CurrentLevel.ID + 1;

            _LevelData nextLevel = _levelsData.Levels.Find(level => level.ID == id);

            if (nextLevel == null)
            {
                return;
            }

            _levelsData.CurrentLevel = nextLevel;

            SaveData();

            StartCoroutine(_SceneHelper.LoadSceneCoroutine(CurrentLevel.SceneName));
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

            HasLoaded = true;
        }

        private void InitialiseData()
        {
            var levelOne = new _LevelData
            {
                ID = 1,
                SceneName = LevelOneScene,
                LevelCompleted = false
            };

            var levelTwo = new _LevelData()
            {
                ID = 2,
                SceneName = LevelTwoScene,
                LevelCompleted = false
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
