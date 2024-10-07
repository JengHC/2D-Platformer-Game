using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject Prefab;
    public int initialObjectNumber = 10 ;

    List<GameObject> objs;

    void Start()
    {
        objs = new List<GameObject>();

        for(int i=0; i<initialObjectNumber; i++)
        {
            GameObject go = Instantiate(Prefab, transform);
            go.SetActive(false);
            objs.Add(go);
        }

    }

    public GameObject GetObject()
    {
        foreach(GameObject go in objs)
        {
            if(!go.activeSelf)
            {
                go.SetActive(true);
                return go;
            }
        }

        GameObject obj = Instantiate(Prefab,transform);
        objs.Add(obj);
        return obj;
    }
}
