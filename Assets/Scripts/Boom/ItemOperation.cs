using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ItemOperation : MonoBehaviour,IPointerDownHandler,IPointerUpHandler {

	private Item item;

	void Awake()
	{
		item = GetComponent<Item> ();
	}

	//按下的鼠标坐标
	private Vector3 downPos;
	//抬起的鼠标坐标
	private Vector3 upPos;

	public void OnPointerDown (PointerEventData eventData)
	{
		downPos = Input.mousePosition;
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		//如果其他人正在操作
		if (GameController.instance.isOperation)
			return;//返回
		//正在操作
		GameController.instance.isOperation = true;
		upPos = Input.mousePosition;
		//获取方向
		Vector2 dir = GetDirection ();
		//点击异常处理
		if (dir.magnitude != 1) {
			GameController.instance.isOperation = false;
			return;
		}
		//开启协程
		StartCoroutine (ItemExchange (dir));
	}

	/// <summary>
	/// Item交换
	/// </summary>
	/// <returns>The exchange.</returns>
	/// <param name="dir">Dir.</param>
	IEnumerator ItemExchange(Vector2 dir)
	{
		//获取目标行列
		int targetRow = item.itemRow + System.Convert.ToInt32(dir.y);
		int targetColumn = item.itemColumn + System.Convert.ToInt32(dir.x);
		//检测合法
		bool isLagal = GameController.instance.CheckRCLegal (targetRow, targetColumn);
		if (!isLagal) {
			GameController.instance.isOperation = false;
			//不合法跳出
			yield break;
		}
		//获取目标
		Item target = GameController.instance.allItems [targetRow, targetColumn];
		//从全局列表中获取当前item，查看是否已经被消除，被消除后不能再交换
		Item myItem = GameController.instance.allItems [item.itemRow, item.itemColumn];
		if (!target || !myItem) {
			GameController.instance.isOperation = false;
			//Item已经被消除
			yield break;
		}
		//相互移动
		target.GetComponent<ItemOperation> ().ItemMove (item.itemRow, item.itemColumn, transform.position);
		ItemMove (targetRow, targetColumn, target.transform.position);
		//还原标志位
		bool reduction = false;
		//消除处理
		item.CheckAroundBoom();
		if (GameController.instance.boomList.Count == 0) {
			reduction = true;
		}
		target.CheckAroundBoom ();
		if (GameController.instance.boomList.Count != 0) {
			reduction = false;
		}
		//还原
		if (reduction) {
			//延迟
			yield return new WaitForSeconds (0.2f);
			//临时行列
			int tempRow, tempColumn;
			tempRow = myItem.itemRow;
			tempColumn = myItem.itemColumn;
			//移动
			myItem.GetComponent<ItemOperation> ().ItemMove (target.itemRow,
				target.itemColumn, target.transform.position);
			target.GetComponent<ItemOperation> ().ItemMove (tempRow,
				tempColumn, myItem.transform.position);
			//延迟
			yield return new WaitForSeconds (0.2f);
			//操作完毕
			GameController.instance.isOperation = false;
		} 
	}
	/// <summary>
	/// Item的移动
	/// </summary>
	public void ItemMove(int targetRow,int targetColumn,Vector3 pos)
	{
		//改行列
		item.itemRow = targetRow;
		item.itemColumn = targetColumn;
		//改全局列表
		GameController.instance.allItems [targetRow, targetColumn] = item;
		//移动
		transform.DOMove (pos, 0.2f);
	}

	/// <summary>
	/// 获取鼠标滑动方向
	/// </summary>
	/// <returns>The direction.</returns>
	public Vector2 GetDirection()
	{
		//方向向量
		Vector3 dir = upPos - downPos;
		//如果是横向滑动
		if (Mathf.Abs (dir.x) > Mathf.Abs (dir.y)) {
			//返回横向坐标
			return new Vector2 (dir.x/Mathf.Abs(dir.x),0);
		} else {
			//返回纵向坐标
			return new Vector2 (0,dir.y/Mathf.Abs(dir.y));
		}
	}
	/// <summary>
	/// 下落
	/// </summary>
	/// <param name="pos">Position.</param>
	public void CurrentItemDrop(Vector3 pos)
	{
		//下落
		transform.DOMove (pos,0.2f);
	}
}