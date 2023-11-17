using AdventureStorm.Gameplay.EnemyOne;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private StyleSheet _styleSheet;

    [SerializeField] private GameObject _enemy;

    private VisualElement _root;

    private ProgressBar _healthBar;

    private EnemyOneStateManager _enemyStateManager;

    private void Start()
    {
        if (_styleSheet == null && _enemy == null)
        {
            return;
        }

        _root = GetComponent<UIDocument>().rootVisualElement;

        if (_enemyStateManager == null)
        {
            _enemyStateManager = GetComponentInParent<EnemyOneStateManager>();
        }

        _root.AddToClassList("container");

        _root.styleSheets.Add(_styleSheet);

        _healthBar = new ProgressBar();
        _healthBar.AddToClassList("health-bar");

        _healthBar.highValue = _enemyStateManager.Health;

        _root.Add(_healthBar);

        SetPosition();
    }

    private void LateUpdate()
    {
        if (gameObject.transform != null)
        {
            SetPosition();
        }
    }

    private void Update()
    {
        if (_healthBar != null)
        {
            _healthBar.value = _enemyStateManager.Health;
        }
    }

    private void SetPosition()
    {
        if (_root != null)
        {
            Vector3 enemyPosition = _enemy.transform.position;
            enemyPosition.y += 3.6f;
            enemyPosition.x -= 0.2f;

            Vector2 newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(_root.panel, enemyPosition, Camera.allCameras[0]);
            newPosition.x = (newPosition.x - _root.layout.width / 2);
            _root.transform.position = newPosition;
        }
    }
}
