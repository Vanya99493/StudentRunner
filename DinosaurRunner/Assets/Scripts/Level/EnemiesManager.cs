using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private Transform _parentToAllEnemies;

    [Header("Prefabs")]
    [SerializeField] private EnemyController[] _pedestrianEnemiesPrefabs;
    [SerializeField] private EnemyController[] _flyingEnemiesPrefabs;

    [Header("Spawn positions")]
    [SerializeField] private Vector3 _spawnPedestrianPosition;
    [SerializeField] private Vector3 _spawnFlyingPosition;

    private PoolMono<EnemyController> _pedestrianEnemiesPool;
    private PoolMono<EnemyController> _flyingEnemiesPool;
    private float _xPositionToHideEnemy;

    public void Initialize(int numberOfPedestrianEnemiesInPool, int numberOfFluingEnemiesInPool, float xPositionToHideEnemy)
    {
        _pedestrianEnemiesPool = new PoolMono<EnemyController>(_pedestrianEnemiesPrefabs, numberOfPedestrianEnemiesInPool, _parentToAllEnemies);
        _flyingEnemiesPool = new PoolMono<EnemyController>(_flyingEnemiesPrefabs, numberOfFluingEnemiesInPool, _parentToAllEnemies);
        _xPositionToHideEnemy = xPositionToHideEnemy;
    }

    public void MoveEnemies(float speed, float deltaTime)
    {
        for (int i = 0; i < _pedestrianEnemiesPool.PoolLenth; i++)
        {
            if (_pedestrianEnemiesPool[i].gameObject.activeInHierarchy)
            {
                _pedestrianEnemiesPool[i].Move(speed, deltaTime);
                if (_pedestrianEnemiesPool[i].gameObject.transform.position.x <= _xPositionToHideEnemy)
                {
                    _pedestrianEnemiesPool.HideElement(i);
                }
            }
        }
        for (int i = 0; i < _flyingEnemiesPool.PoolLenth; i++)
        {
            if (_flyingEnemiesPool[i].gameObject.activeInHierarchy)
            {
                _flyingEnemiesPool[i].Move(speed, deltaTime);
                if (_flyingEnemiesPool[i].gameObject.transform.position.x <= _xPositionToHideEnemy)
                {
                    _flyingEnemiesPool.HideElement(i);
                }
            }
        }
    }

    public EnemyController InstantiatePedestrianEnemy()
    {
        var newElement = _pedestrianEnemiesPool.GetRandomFreeElement();
        newElement.gameObject.transform.position = _spawnPedestrianPosition;
        return newElement;
    }

    public EnemyController InstantiateFlyingEnemy()
    {
        var newElement = _flyingEnemiesPool.GetRandomFreeElement();
        newElement.gameObject.transform.position = _spawnFlyingPosition;
        return newElement;
    }

    public void HideAllEnemies()
    {
        _pedestrianEnemiesPool.HideAllElements();
        _flyingEnemiesPool.HideAllElements();
    }
}