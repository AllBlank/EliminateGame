using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

	public int itemRow;//行
	public int itemColumn;//列
	//当前图案
	public Sprite currentSpr;
	//图案
	public Image currentImg;

	private GameController controller;
	//被检测
	public bool hasCheck = false;

	void Awake()
	{
		currentImg = transform.GetChild (0).GetComponent<Image> ();
	}

	void OnEnable()
	{
		controller = GameController.instance;
	}

	/// <summary>
	/// 点击事件
	/// </summary>
	public void CheckAroundBoom()
	{
		controller.sameItemsList.Clear ();
		controller.boomList.Clear ();
		controller.randomColor = Color.white;
			//new Color (Random.Range (0.1f, 1f), Random.Range (0.1f, 1f), Random.Range (0.1f, 1f), 1);
		controller.FillSameItemsList (this);
		controller.FillBoomList (this);
	}
}