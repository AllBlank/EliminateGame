using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectPool : MonoBehaviour {

	//单例
	public static ObjectPool instance;
	//对象池
	private Dictionary<string,List<GameObject>> pool;

	void Awake()
	{
		instance = this;
		pool = new Dictionary<string, List<GameObject>> ();
	}

	public void SetGameObject(GameObject current)
	{
		//设置成非激活状态
		current.SetActive (false);
		//清空父对象
//		current.transform.parent = null;
		//是否有该类型的对象池
		if (pool.ContainsKey (current.tag)) {
			//添加到对象池
			pool [current.tag].Add (current);
		} else {
			pool [current.tag] = new List<GameObject> (){ current };
		}
	}

	public GameObject GetGameObject(string objName,Transform parent = null)
	{
		GameObject current;
		//包含此对象池,且有对象
		if (pool.ContainsKey (objName) && pool[objName].Count > 0) {
			//获取对象
			current = pool [objName] [0];
		} else {
			//加载预设体
			GameObject prefab = Resources.Load<GameObject> (Util.ResourcesPrefab + objName);
			//生成
			current = Instantiate(prefab) as GameObject;
		}
		//设置激活状态
		current.SetActive (true);
		//设置父物体
		current.transform.parent = parent;

		current.transform.DOScale (Vector3.one, 0.1f);

		//返回
		return current;
	}
}