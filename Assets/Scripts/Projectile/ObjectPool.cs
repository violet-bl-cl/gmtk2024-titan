using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool Instance { get; private set; }
    public int NumOfObjects;
    public GameObject ObjectToPool;
    private GameObject _poolParent;
    private List<GameObject> _poolObjects;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(Instance);
        }
    }

    void Start()
    {
        if (NumOfObjects < 0) return;
        _poolParent = Instantiate(new GameObject());
        _poolParent.name = "Bullets";
        _poolObjects = new List<GameObject>(NumOfObjects);
        GameObject poolObject;
        for (int i = 0; i < NumOfObjects; i++)
        {
            poolObject = Instantiate(ObjectToPool, Vector2.zero, Quaternion.identity);
            poolObject.transform.SetParent(_poolParent.transform);
            poolObject.transform.name = $"BulletInstance{i}";
            poolObject.SetActive(false);
            _poolObjects.Add(poolObject);
        }
    }
    public GameObject GetObjectPool()
    {
        for (int i = 0; i < NumOfObjects; i++)
        {
            if (!_poolObjects[i].activeInHierarchy)
            {
                return _poolObjects[i];
            }
        }
        return null;
    }
}