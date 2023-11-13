using AdventureStorm.Tools;
using UnityEngine;

namespace AdventureStorm.Gameplay
{
    public class EnemyStateManager : MonoBehaviour, IDamageable
    {
        #region Constant Fields

        public const string PlayerTag = "Player";

        public const string PlayerLayerMaskName = "Player";

        public const float MaximumHealth = 5f;

        #endregion

        #region Fields

        [Tooltip("Used when the randomly selected enemy dies.")]
        public GameObject KeyPrefab;

        private EnemyBaseState _currentState;

        #endregion

        #region Properties

        public Rigidbody2D RB2D { get; private set; }

        public AnimatorManager AnimatorManager { get; private set; }

        public LayerMask PlayerLayerMask { get; private set; }

        public Coroutine RechargeDodgeAttackStaminaCoroutine { get; set; }

        public EnemyAliveState AliveState { get; private set; }

        public EnemyDeathState DeathState { get; private set; }

        public float Health { get; private set; }

        public bool IsAlive { get => Health > 0f; }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            RB2D = GetComponent<Rigidbody2D>();

            AnimatorManager = GetComponent<AnimatorManager>();

            PlayerLayerMask = LayerMask.GetMask(PlayerLayerMaskName);

            RechargeDodgeAttackStaminaCoroutine = null;

            AliveState = new EnemyAliveState();
            DeathState = new EnemyDeathState();

            Health = MaximumHealth;
        }

        // Start is called before the first frame update.
        private void Start()
        {
            _currentState = AliveState;
            _currentState.EnterState(this);
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
        }

        // Update is called once per frame.
        private void Update()
        {
            if (IsAlive)
            {
                AliveState.FacePlayer(this);
                AliveState.FlipEnemy(this);
            }
            else
            {
                if (_currentState.ToString() != DeathState.ToString())
                {
                    SwitchState(DeathState);
                }
            }

            _currentState.UpdateState(this);
        }

        #endregion

        #region Public Methods
        
        public void SwitchState(EnemyBaseState state)
        {
            _currentState.ExitState(this);
            
            _currentState = state;
            _currentState.EnterState(this);
        }

        public void Damage(float damage)
        {
            if (IsAlive)
            {
                Health -= damage;
            }
        }

        public void FinishedDyingAnimation()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
