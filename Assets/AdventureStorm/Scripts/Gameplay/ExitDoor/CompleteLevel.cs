using AdventureStorm.Data;
using AdventureStorm.Gameplay.Level;
using AdventureStorm.Gameplay.Player;
using AdventureStorm.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureStorm.Gameplay.ExitDoor
{
    public class CompleteLevel : MonoBehaviour
    {
        #region Fields

        private GameObject _system;

        private LevelManager _levelManager;

        #endregion

        #region Events / Delegates

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if (playerController.WasKeyPickedUp && enemies.Length == 0)
                {
                    _levelManager.MarkCurrentLevelAsComplete();

                    LevelData currentLevel = _levelManager.CurrentLevel;
                    List<LevelData> levels = _levelManager.Levels;

                    // Last level
                    if (currentLevel.ID == levels[levels.Count - 1].ID)
                    {
                        StartCoroutine(SceneHelper.LoadSceneCoroutine("GameCompleteUIScene"));
                    }
                    else
                    {
                        StartCoroutine(SceneHelper.LoadSceneCoroutine("LevelCompleteUIScene"));
                    }
                }
            }
        }

        #endregion

        #region LifeCycle

        private void Start()
        {
            if (_system == null)
            {
                _system = GameObject.Find("@System");
            }

            if (_system != null)
            {
                _levelManager = _system.GetComponent<LevelManager>();
            }
        }

        #endregion
    }
}