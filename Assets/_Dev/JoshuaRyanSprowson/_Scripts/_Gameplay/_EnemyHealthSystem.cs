using AdventureStorm;
using UnityEngine;

public class _EnemyHealthSystem : MonoBehaviour, _IDamageable
{
    #region Fields

    [Tooltip("How much starting health does the enemy have?")]
    /// <summary>
    /// How much starting health does the enemy have
    /// </summary>
    [SerializeField] private float _startingHealth = 5f;

    #endregion

    #region Properties

    public float Health { get; private set; }
    public bool IsAlive { get; private set; }

    #endregion

    #region LifeCycle

    private void Start()
    {
        Health = _startingHealth;
        IsAlive = true;
    }

    private void Update()
    {
        IsAlive = Health > 0;

        if (!IsAlive)
        {
            Destroy(gameObject);
        }
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

    #endregion
}
