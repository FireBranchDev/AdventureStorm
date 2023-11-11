using AdventureStorm.Data;
using AdventureStorm.Systems;
using AdventureStorm.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class PlayerMovementState : PlayerBaseState
    {
        #region Constant Fields

        private const string WalkingAnimation = "Walking";
        private const float WalkingSpeed = 5f;

        private const string KeyTag = "Key";

        private const string ExitDoorTag = "ExitDoor";

        private const string SystemGameObjectName = "@System";

        private const string GameCompleteUIScene = "GameCompleteUIScene";

        private const string LevelCompleteUIScene = "LevelCompleteUIScene";

        #endregion

        #region Fields

        private Coroutine _walkingCoroutine;
        private float _horizontalAxis;

        #endregion

        #region Constructors

        public PlayerMovementState()
        {
            _walkingCoroutine = null;
            _horizontalAxis = 0f;
        }

        #endregion

        #region Public Methods

        public override void EnterState(PlayerStateManager player)
        {
            _walkingCoroutine = player.StartCoroutine(WalkingCoroutine(player));

            player.AliveState.RechargeDodgeAttackStaminaCoroutine ??=
                player.StartCoroutine(player.AliveState.DodgingState.RechargeDodgeAttackStaminaCoroutine());
        }

        public override void ExitState(PlayerStateManager player)
        {
            if (_walkingCoroutine != null)
            {
                player.StopCoroutine(_walkingCoroutine);
                _walkingCoroutine = null;
            }
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            Move(player);
        }

        public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerExit2D(PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerStay2D(PlayerStateManager player, Collider2D collision)
        {
            if (collision.CompareTag(KeyTag))
            {
                GameObject key = collision.gameObject;
                Object.Destroy(key);
                player.HasKey = true;
            }

            if (collision.CompareTag(ExitDoorTag))
            {
                if (player.HasKey)
                {
                    GameObject system = GameObject.Find(SystemGameObjectName);

                    if (system != null)
                    {
                        if (system.TryGetComponent<LevelManager>(out var levelManager))
                        {
                            levelManager.MarkCurrentLevelAsComplete();

                            List<LevelData> completedLevels = levelManager.GetCompletedLevels();
                            List<LevelData> uncompletedLevels = levelManager.GetUncompletedLevels();

                            // Last level of the game.
                            if (levelManager.CurrentLevel.ID == completedLevels.Count + uncompletedLevels.Count)
                            {
                                player.StartCoroutine(SceneHelper.LoadSceneCoroutine(GameCompleteUIScene));
                            }
                            else
                            {
                                player.StartCoroutine(SceneHelper.LoadSceneCoroutine(LevelCompleteUIScene));
                            }
                        }
                    }
                }
            }
        }

        public override void UpdateState(PlayerStateManager player)
        {
            _horizontalAxis = Input.GetAxis(PlayerStateManager.HorizontalAxis);

            if (_horizontalAxis == 0f)
            {
                player.SwitchState(player.AliveState.IdleState);
            }

            if (Input.GetMouseButtonDown(0))
            {
                player.SwitchState(player.AliveState.AttackingState);
            }

            if (Input.GetKey(KeyCode.Space) && _horizontalAxis != 0f)
            {
                player.SwitchState(player.AliveState.DodgingState);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator WalkingCoroutine(PlayerStateManager player)
        {
            if (player.AnimatorManager != null)
            {
                player.AnimatorManager.ChangeAnimationState(WalkingAnimation);
                for (; ; )
                {
                    if (player.AnimatorManager.DidAnimationFinish(WalkingAnimation))
                    {
                        player.AnimatorManager.ReplayAnimation();
                    }

                    yield return null;
                }
            }
        }

        private void Move(PlayerStateManager player)
        {
            Vector2 velocity = Vector2.zero;

            if (_horizontalAxis > 0f)
            {
                velocity.x = WalkingSpeed;
            }
            else if (_horizontalAxis < 0f)
            {
                velocity.x = -WalkingSpeed;
            }

            player.Rb2D.MovePosition(player.Rb2D.position + velocity * Time.fixedDeltaTime);
        }

        #endregion
    }
}
