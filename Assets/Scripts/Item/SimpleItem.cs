using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum ItemType
{
    Normal,
    ClearRow,
    ClearCol,
    ClearCross,
    ClearRange,
    ClearSame
}

public class SimpleItem : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    /// <summary>
    /// 行号
    /// </summary>
    public int row;
    /// <summary>
    /// 列号
    /// </summary>
    public int col;

    public ItemType type = ItemType.Normal;

    public ItemType Type
    {
        set
        {
            type = value;
            switch (type)
            {
                case ItemType.Normal:
                    ani.SetBool("isClearRow", false);
                    ani.SetBool("isClearCol", false);
                    ani.SetBool("isClearCross", false);
                    ani.SetBool("isClearRange", false);
                    ani.SetBool("isClearSame", false);
                    break;
                case ItemType.ClearCol:
                    ani.SetBool("isClearRow", false);
                    ani.SetBool("isClearCol", true);
                    ani.SetBool("isClearCross", false);
                    ani.SetBool("isClearRange", false);
                    ani.SetBool("isClearSame", false);
                    break;
                case ItemType.ClearRow:
                    ani.SetBool("isClearRow", true);
                    ani.SetBool("isClearCol", false);
                    ani.SetBool("isClearCross", false);
                    ani.SetBool("isClearRange", false);
                    ani.SetBool("isClearSame", false);
                    break;
                case ItemType.ClearCross:
                    ani.SetBool("isClearRow", false);
                    ani.SetBool("isClearCol", false);
                    ani.SetBool("isClearCross", true);
                    ani.SetBool("isClearRange", false);
                    ani.SetBool("isClearSame", false);
                    break;
                case ItemType.ClearRange:
                    ani.SetBool("isClearRow", false);
                    ani.SetBool("isClearCol", false);
                    ani.SetBool("isClearCross", false);
                    ani.SetBool("isClearRange", true);
                    ani.SetBool("isClearSame", false);
                    break;
                case ItemType.ClearSame:
                    ani.SetBool("isClearRow", false);
                    ani.SetBool("isClearCol", false);
                    ani.SetBool("isClearCross", false);
                    ani.SetBool("isClearRange", false);
                    ani.SetBool("isClearSame", true);
                    break;
            }
        }
    }

    /// <summary>
    /// 开始拖拽的鼠标位置
    /// </summary>
    private Vector3 beginDragPos;
    /// <summary>
    /// 结束拖拽的鼠标位置
    /// </summary>
    private Vector3 endDragPos;

    private Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    /// <summary>
    /// 开始拖拽回调
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //如果是左键拖拽
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //记录位置
            beginDragPos = Input.mousePosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    /// <summary>
    /// 结束拖拽回调
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        endDragPos = Input.mousePosition;
        float x = endDragPos.x - beginDragPos.x;
        float y = endDragPos.y - beginDragPos.y;
        Dir dir = ItemsManager.instance.ChangeItemDir(x, y);
        int targetRow = -1;
        int targetCol = -1;
        switch (dir)
        {
            case Dir.Up:
                targetRow = row - 1;
                targetCol = col;
                break;
            case Dir.Down:
                targetRow = row + 1;
                targetCol = col;
                break;
            case Dir.Left:
                targetCol = col - 1;
                targetRow = row;
                break;
            case Dir.Right:
                targetCol = col + 1;
                targetRow = row;
                break;
            case Dir.No:
                return;
        }
        ItemsManager.instance.ChangeItem(this, targetRow, targetCol);
    }


}
