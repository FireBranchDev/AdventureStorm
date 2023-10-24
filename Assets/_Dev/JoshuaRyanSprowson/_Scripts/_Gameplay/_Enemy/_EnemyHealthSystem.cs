using AdventureStorm;
using UnityEngine;

public class _EnemyHealthSystem : MonoBehaviour, _IDamageable
{
    #region Constant Fields

    private const string DyingAnimation = "Dying";

    #endregion

    #region Fields

    [Tooltip("How much starting health does the enemy have?")]
    /// <summary>
    /// How much starting health does the enemy have
    /// </summary>
    [SerializeField] private float _startingHealth = 5f;

    private _AnimatorManager _animatorManager;

    #endregion

    #region Properties

    public float Health { get; private set; }
    public bool IsAlive { get; private set; }

    #endregion

    #region LifeCycle

    private void Start()
    {
        Health = _startingHealth;

        _animatorManager = GetComponent<_AnimatorManager>();

        IsAlive = true;
    }

    private void Update()
    {
        IsAlive = Health > 0;

        if (!IsAlive)
            Dying();
    }

    #endregion

    #region Public Methods

    public void Damage(float damage)
    {
        if (Health - damage > 0)
        {
            Health -= damage;
        }
        else
        {
            Health = 0;
            IsAlive = false;
        }
    }

    /// <summary>
    /// Executed once the dying animation has finished.
    /// </summary>
    public void FinishedDyingAnimation()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Private Methods

    private void Dying()
    {
        if (IsAlive)
            return;

        _animatorManager.ChangeAnimationState(DyingAnimation);
    }

    #endregion
}
