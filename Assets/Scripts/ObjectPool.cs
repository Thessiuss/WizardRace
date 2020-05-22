using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : ScriptableObject {

    private int pooledAmount;
    private bool willGrow;
    private GameObject parent;
    private GameObject prefab;
    private List<GameObject> gameObjects;


    // TODO: Add a reference to the parent object here
    public void Populate(int pooledAmount, GameObject prefab, GameObject parent, bool willGrow) {
        this.pooledAmount = pooledAmount;
        this.prefab = prefab;
        this.parent = parent;
        this.willGrow = willGrow;
        PopulatePool();
    }
    
    // TODO: Instantiate the object and set correct parent
	private void PopulatePool ()
    {
        gameObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            InstantiateObject(parent);
        }
	}
    
    public GameObject GetPooledObject()
    {
        return GetObjectInPool();
    }

    public GameObject GetPooledObject(GameObject parent)
    {
        GameObject go = GetObjectInPool();
        go.transform.parent = parent.transform;
        return go;
    }

    private GameObject InstantiateObject(GameObject parent) {
        GameObject obj = (GameObject)Instantiate(prefab);
        if (parent != null)
        {
            obj.transform.SetParent(parent.transform, false);
        }
        obj.SetActive(false);
        gameObjects.Add(obj);
        return obj;
    }

    private GameObject GetObjectInPool()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (!gameObjects[i].activeInHierarchy)
            {
                return gameObjects[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = InstantiateObject(parent);
            return obj;
        }
        return null;
    }
}
