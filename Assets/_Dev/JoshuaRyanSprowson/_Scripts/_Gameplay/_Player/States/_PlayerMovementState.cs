using AdventureStorm._Data;
using AdventureStorm._Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureStorm
{
    public class _PlayerMovementState : _PlayerBaseState
    {
        #region Constant Fields

        private const string WalkingAnimation = "Walking";
        private const float WalkingSpeed = 5f;

        private const string KeyTag = "Key";

        private const string ExitDoorTag = "ExitDoor";

        private const string SystemGameObjectName = "@System";

        private const string GameCompleteUIScene = "_GameCompleteUIScene";

        private const string LevelCompleteUIScene = "_LevelCompleteUIScene";

        #endregion

        #region Fields

        private Coroutine _walkingCoroutine;
        private float _horizontalAxis;

        #endregion

        #region Constructors

        public _PlayerMovementState()
        {
            _walkingCoroutine = null;
            _horizontalAxis = 0f;
        }

        #endregion

        #region Public Methods

        public override void EnterState(_PlayerStateManager player)
        {
            _walkingCoroutine = player.StartCoroutine(WalkingCoroutine(player));

            player.AliveState.RechargeDodgeAttackStaminaCoroutine ??=
                player.StartCoroutine(player.AliveState.DodgingState.RechargeDodgeAttackStaminaCoroutine());
        }

        public override void ExitState(_PlayerStateManager player)
        {
            if (_walkingCoroutine != null)
            {
                player.StopCoroutine(_walkingCoroutine);
                _walkingCoroutine = null;
            }
        }

        public override void FixedUpdateState(_PlayerStateManager player)
        {
            Move(player);
        }

        public override void OnTriggerEnter2D(_PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerExit2D(_PlayerStateManager player, Collider2D collision)
        {

        }

        public override void OnTriggerStay2D(_PlayerStateManager player, Collider2D collision)
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
                        if (system.TryGetComponent<_LevelManager>(out var levelManager))
                        {
                            levelManager.MarkCurrentLevelAsComplete();

                            List<_LevelData> levels = levelManager.GetUncompletedLevels();

                            if (levels.Count >= 1)
                            {
                                player.StartCoroutine(_SceneHelper.LoadSceneCoroutine(LevelCompleteUIScene));
                            }
                        }
                    }
                }
            }
        }

        public override void UpdateState(_PlayerStateManager player)
        {
            _horizontalAxis = Input.GetAxis(_PlayerStateManager.HorizontalAxis);

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

        private IEnumerator WalkingCoroutine(_PlayerStateManager player)
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

        private void Move(_PlayerStateManager player)
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
