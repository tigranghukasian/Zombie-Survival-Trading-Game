using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{
    
    private Dictionary<int, ObjectPool<GameObject>> pools = new Dictionary<int, ObjectPool<GameObject>>();
    protected override void Awake()
    {
        base.Awake();

    }

    public ObjectPool<GameObject> CreatePool(GameObject _prefab, int _initialSize = 10, int _maxSize = 50)
    {
        int id = _prefab.GetInstanceID();
        if (pools.ContainsKey(id))
        {
            Debug.LogWarning($"PoolManager: A pool for ID {id} already exists. Ignoring new pool creation request.");
            return pools[id];
        }

        var pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                var obj = Instantiate(_prefab);
                var poolableComponent = obj.GetComponent<IPoolable>();
                if (poolableComponent != null)
                {
                    poolableComponent.GameObject = obj;
                    poolableComponent.PoolableId = id;
                }
                return obj;
            },
            actionOnGet: _obj =>
            {
                _obj.SetActive(true);
            },
            actionOnRelease: _obj =>
            {
                
                _obj.SetActive(false);
            },
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: _initialSize,
            maxSize: _maxSize
        );

        pools[id] = pool;
        return pool;
    }

    public GameObject GetPooledObject(GameObject _prefab, Vector3 _position, Quaternion _rotation)
    {
        var pooledObject = GetPool(_prefab).Get();
        pooledObject.transform.position = _position;
        pooledObject.transform.rotation = _rotation;
        var poolableComponent = pooledObject.GetComponent<IPoolable>();
        poolableComponent?.OnPoolableObjectGet();
        return pooledObject;
    }

    public void ReleasePooledObject(IPoolable _poolable)
    {
        _poolable?.OnPoolableObjectRelease();
        var pool = GetPool(_poolable.PoolableId);
        pool.Release(_poolable.GameObject);
    }


    private ObjectPool<GameObject> GetPool(int _id)
    {
        if (pools.TryGetValue(_id, out var pool))
        {
            return pool;
        }
        else
        {
            throw new InvalidOperationException($"Pool for prefab with Id {_id} does not exist.");
        }
    }
    private ObjectPool<GameObject> GetPool(GameObject prefab)
    {
        if (pools.TryGetValue(prefab.GetInstanceID(), out var pool))
        {
            return pool;
        }
        else
        {
            throw new InvalidOperationException($"Pool for prefab with Id {prefab.GetInstanceID()} does not exist.");
        }
    }
}
