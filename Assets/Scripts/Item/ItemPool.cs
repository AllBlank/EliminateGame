using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{

    public static ItemPool instance;

    public List<GameObject> pool;

    private GameObject itemPre;

    private void Awake()
    {
        instance = this;
        pool = new List<GameObject>();
        itemPre = Resources.Load<GameObject>(Util.ResourcesPrefab + Util.SimpleItem);
    }

    public void SetItem(GameObject item)
    {
        item.SetActive(false);
        pool.Add(item);
    }

    public GameObject GetItem(Transform parent = null)
    {
        if (pool.Count > 0)
        {
            GameObject go = pool[0];
            go.SetActive(true);
            pool.RemoveAt(0);
            return go;
        }
        else
        {
            GameObject go = Instantiate(itemPre);
            go.transform.SetParent(parent);
            return go;
        }
    }
}