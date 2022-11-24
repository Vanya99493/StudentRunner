using System.Collections.Generic;
using UnityEngine;

public class PoolMono<T> where T : MonoBehaviour
{
    private T[] _prefabs;
    private Transform _container;
    private List<T> _pool;
    private int _lastPrefabIndex;
    private int _numberOFFreeElements;

    public int PoolLenth { get { return _pool.Count; } }
    public T this[int i] { get { return _pool[i]; } }

    public PoolMono(T[] prefabs, int count, Transform container)
    {
        _prefabs = new T[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            _prefabs[i] = prefabs[i];
        }
        _container = container;

        CreatePool(count);
    }

    public T GetFreeElement()
    {
        if(_numberOFFreeElements <= 0)
        {
            _lastPrefabIndex = (1 + _lastPrefabIndex) % _prefabs.Length;
            return CreateObject(_lastPrefabIndex, true);
        }

        foreach (var mono in _pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                T element = mono;
                mono.gameObject.SetActive(true);
                _numberOFFreeElements--;
                return element;
            }
        }

        return null;
    }

    public T GetRandomFreeElement()
    {
        if (_numberOFFreeElements <= 0)
        {
            _lastPrefabIndex = (1 + _lastPrefabIndex) % _prefabs.Length;
            return CreateObject(_lastPrefabIndex, true);
        }

        while(true)
        {
            int randomIndex = Random.Range(0, _pool.Count);
            if (!_pool[randomIndex].gameObject.activeInHierarchy)
            {
                T element = _pool[randomIndex];
                element.gameObject.SetActive(true);
                _numberOFFreeElements--;
                return element;
            }
        }
    }

    public void HideElement(int index)
    {
        if (!_pool[index].gameObject.activeInHierarchy)
        {
            return;
        }
        _pool[index].gameObject.SetActive(false);
        _numberOFFreeElements++;
    }

    public void HideAllElements()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            HideElement(i);
        }
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();

        for (int i = 0; i < count; i++)
        {
            _lastPrefabIndex = i % _prefabs.Length;
            CreateObject(_lastPrefabIndex);
        }
        _numberOFFreeElements = _pool.Count;
    }

    private T CreateObject(int prefabIndex, bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(_prefabs[prefabIndex], _container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createdObject);
        return createdObject;
    }
}