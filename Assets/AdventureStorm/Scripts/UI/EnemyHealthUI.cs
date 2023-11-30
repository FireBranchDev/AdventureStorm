using AdventureStorm.Gameplay;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private StyleSheet _styleSheet;

    private VisualElement _root;

    private ProgressBar _healthBar;

    private EnemyController _enemyController;

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _root.AddToClassList("container");

        _root.styleSheets.Add(_styleSheet);

        _healthBar = new ProgressBar();
        _healthBar.AddToClassList("health-bar");

        _root.Add(_healthBar);

        _enemyController = GetComponentInParent<EnemyController>();

        if (_enemyController != null)
        {
            _healthBar.highValue = _enemyController.MaxHealth;
        }

        SetPosition();
    }

    private void LateUpdate()
    {
        if (gameObject.transform != null)
        {
            if (_enemyController != null)
            {
                _healthBar.value = _enemyController.Health;
            }

            SetPosition();
        }
    }

    private void SetPosition()
    {
        if (_root != null)
        {
            Vector3 enemyPosition = gameObject.transform.position;
            enemyPosition.y += 3.75f;
            enemyPosition.x -= 0.2f;

            Vector2 newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(_root.panel, enemyPosition, Camera.allCameras[0]);
            newPosition.x = (newPosition.x - _root.layout.width / 2);
            _root.transform.position = newPosition;
        }
    }
}
