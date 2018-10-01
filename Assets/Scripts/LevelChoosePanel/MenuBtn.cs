using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuBtn : MonoBehaviour{
    private Button cancelBtn;
    private Button shopBtn;
    private Button hornorBtn;
    private Button rankBtn;

    private void Awake()
    {
        cancelBtn = transform.GetChild(0).GetComponent<Button>();
        cancelBtn.onClick.AddListener(CancelBtnOnClick);
        shopBtn = transform.GetChild(1).GetComponent<Button>();
        shopBtn.onClick.AddListener(ShopBtnOnClick);
        hornorBtn = transform.GetChild(2).GetComponent<Button>();
        hornorBtn.onClick.AddListener(HornorBtnOnClick);
        rankBtn = transform.GetChild(3).GetComponent<Button>();
        rankBtn.onClick.AddListener(RankBtnOnClick);
    }
    
    /// <summary>
    /// 按下返回按钮的回调事件
    /// </summary>
    public void CancelBtnOnClick()
    {
        Debug.Log("返回主菜单");
        //TODO:返回主菜单
    }

    /// <summary>
    /// 按下商店按钮的回调事件
    /// </summary>
    public void ShopBtnOnClick() {
        Debug.Log("进入商店");
        //TODO:进入商店页面
    }

    /// <summary>
    /// 点击荣誉按钮的回调事件
    /// </summary>
    public void HornorBtnOnClick()
    {
        Debug.Log("进入成就系统");
        //TODO:进入成就页面
    }

    /// <summary>
    /// 点击排行榜按钮的回调事件
    /// </summary>
    public void RankBtnOnClick()
    {
        Debug.Log("进入排行榜界面");
        //TODO:进入排行榜界面
    }

}

