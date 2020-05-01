using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour, IGameModule
{
    [Serializable]
    public class PooledObject
    {
        public string name;
        public GameObject prefab;
        public int poolSize;
    }

    //
    public List<PooledObject> objectsToPool = new List<PooledObject>();
    private readonly Dictionary<string, List<GameObject>> _objectPoolByName = new Dictionary<string, List<GameObject>>();
    //


    public bool IsInitialized { get { return _isInitialized; } }
    private bool _isInitialized = false;

    //
    public IEnumerator LoadModule()
    {
        Debug.Log("[ObjectPool] init ");
        InitPool();
        yield return new WaitUntil(()=> { return IsInitialized; }  );

        ServiceLocator.Register<ObjectPoolManager>(this);

    }
    public void ResetObjsInPool(string objTypeName)
    {
        List<GameObject> objpool = _objectPoolByName[objTypeName];
        foreach (var obj in objpool)
        {
            obj.SetActive(false);
        }
    }
    public void ResetAllPools()
    {
        foreach (var pool in _objectPoolByName)
        {
            foreach (var obj in pool.Value)
            {
                obj.SetActive(false);
            }

        }

    }


    private void InitPool()
    {
        GameObject PoolManagerGO = new GameObject("Object Pool");
        PoolManagerGO.transform.SetParent(GameObject.FindWithTag("Services").transform);

        foreach (PooledObject pooledObject in objectsToPool)
        {
            if (!_objectPoolByName.ContainsKey(pooledObject.name))
            {
                GameObject poolGO = new GameObject(pooledObject.name);// [obj pool GO]
                poolGO.transform.SetParent(PoolManagerGO.transform);


                _objectPoolByName.Add(pooledObject.name, new List<GameObject>());

                for (int i = 0; i < pooledObject.poolSize; ++i)
                {
                    GameObject go = Instantiate(pooledObject.prefab); // [objs] in  [obj pool GO]

                    go.name = string.Format("{0}_{1:000}", pooledObject.name, i);//{x:000} format -> 00x ... 0xx ... xxx
                    go.transform.SetParent(poolGO.transform);
                    go.SetActive(false);

                    _objectPoolByName[pooledObject.name].Add(go);
                }
            }
            else
            {
                Debug.Log("[ObjectPoolManager] attemping to create pool with same name ");
                continue;
            }
        }


        _isInitialized = true;
    }

    public List<GameObject> GetObjPoolByName(string poolName)
    {
        return _objectPoolByName[poolName];
    }

    public GameObject GetObjectFromPool(string poolName)
    {
        GameObject ret = null;
        if (_objectPoolByName.ContainsKey(poolName))
        {
            ret = GetNextObject(poolName);
        }
        else
        {
            Debug.Log($"[ObjectPoolManager] No pool exist with name : {poolName}");
        }

        return ret;
    }

    private GameObject GetNextObject(string poolName)
    {
        List<GameObject> pooledObjs = _objectPoolByName[poolName];
        foreach (var go in pooledObjs)
        {
            if (go == null)
            {
                Debug.Log("[ObjectPoolManager] pool object is Null");
                continue;
            }

            if (go.activeInHierarchy)
            {
                continue;
            }
            else
            {
                return go;
            }

        }
        //TODO dynamic resize pool
        Debug.Log("[ObjectPoolManager]  objectpool depleted, no unused objects to return");
 
        return null;
    }

    public void RecycleObject(GameObject go)
    {
        go.SetActive(false);
    }


}
