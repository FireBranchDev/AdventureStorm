using AdventureStorm.Data;
using AdventureStorm.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureStorm.Gameplay.Level
{
    public class LevelManager : MonoBehaviour
    {
        #region Constant Fields

        private const string LevelOneScene = "LevelOneScene";
        private const string LevelTwoScene = "LevelTwoScene";
        private const string LevelThreeScene = "LevelThreeScene";

        private const string SaveFileName = "levels.json";

        #endregion

        #region Fields

        private LevelsData _levelsData;

        private string _saveFilePath;

        #endregion

        #region Properties

        public bool HasLoaded { get; private set; }

        public LevelData CurrentLevel => _levelsData.CurrentLevel;

        public List<LevelData> Levels => _levelsData.Levels;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            HasLoaded = false;

            LoadData();
        }

        private void Start()
        {
            StartCoroutine(SetCurrentLevelCoroutine());
        }

        #endregion

        #region Public Methods

        public void LoadFirstUncompletedLevel()
        {
            LevelData firstUncompletedLevel = _levelsData.Levels.FirstOrDefault(level => !level.LevelCompleted);

            if (firstUncompletedLevel == null)
            {
                return;
            }

            _levelsData.CurrentLevel = firstUncompletedLevel;

            SaveData();

            StartCoroutine(SceneHelper.LoadSceneCoroutine(firstUncompletedLevel.SceneName));
        }

        public void ReplayCompletedLevel(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return;
            }

            LevelData level = _levelsData.Levels.FirstOrDefault(level => level.SceneName == sceneName);

            if (level == null)
            {
                return;
            }

            _levelsData.CurrentLevel = level;

            SaveData();

            StartCoroutine(SceneHelper.LoadSceneCoroutine(sceneName));
        }

        public void MarkCurrentLevelAsComplete()
        {
            LevelData level = _levelsData.Levels.Find(level => level.ID == CurrentLevel.ID);
            level.LevelCompleted = true;
            CurrentLevel.LevelCompleted = true;

            SaveData();
        }

        public List<LevelData> GetUncompletedLevels()
        {
            List<LevelData> levels = _levelsData.Levels.FindAll(level => !level.LevelCompleted);

            return levels;
        }

        public List<LevelData> GetCompletedLevels()
        {
            List<LevelData> levels = _levelsData.Levels.FindAll(level => level.LevelCompleted);

            return levels;
        }

        public void LoadNextLevel()
        {
            int id = _levelsData.CurrentLevel.ID + 1;

            LevelData nextLevel = _levelsData.Levels.Find(level => level.ID == id);

            if (nextLevel == null)
            {
                return;
            }

            _levelsData.CurrentLevel = nextLevel;

            SaveData();

            StartCoroutine(SceneHelper.LoadSceneCoroutine(CurrentLevel.SceneName));
        }

        #endregion

        #region Private Methods

        private async void LoadData()
        {
            if (!File.Exists(_saveFilePath))
            {
                InitialiseData();
            }
            else
            {
                string contents = await File.ReadAllTextAsync(_saveFilePath);
                if (string.IsNullOrEmpty(contents))
                {
                    InitialiseData();
                }
                else
                {
                    var levelsData = JsonUtility.FromJson<LevelsData>(contents);
                    if (levelsData == null)
                    {
                        InitialiseData();
                    }
                    else
                    {
                        _levelsData = levelsData;
                        HasLoaded = true;
                    }
                }
            }
        }

        private void InitialiseData()
        {
            var levelOne = new LevelData
            {
                ID = 1,
                SceneName = LevelOneScene
            };

            var levelTwo = new LevelData
            {
                ID = 2,
                SceneName = LevelTwoScene
            };

            var levelThree = new LevelData
            {
                ID = 3,
                SceneName = LevelThreeScene
            };

            _levelsData = new LevelsData
            {
                Levels = new List<LevelData> { levelOne, levelTwo, levelThree },
                CurrentLevel = levelOne
            };

            SaveData();
        }

        private async void SaveData()
        {
            if (!File.Exists(_saveFilePath))
            {
                await File.Create(_saveFilePath).DisposeAsync();
            }

            while (!File.Exists(_saveFilePath))
            {
                break;
            }

            string saveContentJson = JsonUtility.ToJson(_levelsData);
            await File.WriteAllTextAsync(_saveFilePath, saveContentJson);
        }

        private IEnumerator SetCurrentLevelCoroutine()
        {
            while (!HasLoaded)
            {
                yield return null;
            }

            Scene scene = SceneManager.GetActiveScene();
            foreach (var level in Levels)
            {
                if (level.SceneName == scene.name)
                {
                    _levelsData.CurrentLevel = level;
                }
            }

            SaveData();
        }

        #endregion
    }
}